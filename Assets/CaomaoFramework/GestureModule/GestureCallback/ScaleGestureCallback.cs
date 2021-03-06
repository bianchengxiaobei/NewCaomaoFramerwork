using System;
using System.Collections.Generic;
namespace CaomaoFramework
{
    public class ScaleGestureCallback : IGestureActionCallbackBase
    {
        // these five constants to determine the hysteresis to reduce or eliminate wobble as a zoom is done,
        //  think of a two finger pan and zoom at the same time, tiny changes in scale direction are ignored
        private const float minimumScaleResolutionSquared = 1.005f; // incremental changes
        private const float stationaryScaleResolutionSquared = 1.05f; // change from idle
        private const float stationaryTimeSeconds = 0.1f; // if stationary for this long, use stationaryScaleResolutionSquared else minimumScaleResolutionSquared
        // higher values resist scaling in the opposite direction more
        //private const float hysteresisScaleResolutionSquared = 1.15f; 
        private const float hysteresisScaleResolutionSquared = 1.6f;
        private const int resetDirectionMilliseconds = 300;

        // the min amount that can scale down each update
        private const float minimumScaleDownPerUpdate = 0.25f;

        // the max amount that can scale up each update
        private const float maximumScaleUpPerUpdate = 4.0f;

        private float previousDistanceDirection;
        private float previousDistance;
        private float previousDistanceX;
        private float previousDistanceY;

        private readonly System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();

        /// <summary>
        /// Constructor
        /// </summary>
        public ScaleGestureCallback()
        {
            ScaleMultiplier = ScaleMultiplierX = ScaleMultiplierY = 1.0f;

#if UNITY_2017_4_OR_NEWER

            ZoomSpeed = (UnityEngine.Input.mousePresent ? 3.0f : 1.0f);

#else

            ZoomSpeed = 1.0f;

#endif

            ThresholdUnits = 0.15f;
            MinimumNumberOfTouchesToTrack = MaximumNumberOfTouchesToTrack = 2;
            timer.Start();
        }

        private void SetPreviousDistance(float distance, float distanceX, float distanceY)
        {
            previousDistance = distance;
            previousDistanceX = distanceX;
            previousDistanceY = distanceY;
        }

        private float ClampScale(float rawScale)
        {
            return (rawScale > maximumScaleUpPerUpdate ? maximumScaleUpPerUpdate : (rawScale < minimumScaleDownPerUpdate ? minimumScaleDownPerUpdate : rawScale));
        }

        private float GetScale(float rawScale)
        {
            rawScale = ClampScale(rawScale);
            if (ZoomSpeed != 1.0f)
            {
                if (rawScale < 1.0f)
                {
                    rawScale -= ((1.0f - rawScale) * ZoomSpeed);
                }
                else if (rawScale > 1.0f)
                {
                    rawScale += ((rawScale - 1.0f) * ZoomSpeed);
                }

                // clamp again to account for zoom speed modifiers
                rawScale = (rawScale > maximumScaleUpPerUpdate ? maximumScaleUpPerUpdate : (rawScale < minimumScaleDownPerUpdate ? minimumScaleDownPerUpdate : rawScale));
            }
            return rawScale;
        }

        private void ProcessTouches()
        {
            CalculateFocus(this.m_CurrentTrackGestrueList);
            if (!TrackedTouchCountIsWithinRange)
            {
                return;
            }

            float distance = DistanceBetweenPoints(this.m_CurrentTrackGestrueList[0].x, this.m_CurrentTrackGestrueList[0].y,
                this.m_CurrentTrackGestrueList[1].x, this.m_CurrentTrackGestrueList[1].y);
            float distanceX = DistanceVector(this.m_CurrentTrackGestrueList[0].x - this.m_CurrentTrackGestrueList[1].x);
            float distanceY = DistanceVector(this.m_CurrentTrackGestrueList[0].y - this.m_CurrentTrackGestrueList[1].y);
            if (State == EGestureActionCallbackState.Possible)
            {
                if (previousDistance == 0.0f)
                {
                    SetPreviousDistance(distance, distanceX, distanceY);
                }
                float diff = Math.Abs(previousDistance - distance);
                if (diff >= ThresholdUnits)
                {
                    SetPreviousDistance(distance, distanceX, distanceY);
                    SetState(EGestureActionCallbackState.Began);
                }
            }
            else if (State == EGestureActionCallbackState.Executing)
            {
                // must have a change in distance to execute
                if (distance != previousDistance)
                {
                    // line 3300: https://chromium.googlesource.com/chromiumos/platform/gestures/+/master/src/immediate_interpreter.cc

                    // get jitter threshold based on stationary movement or not
                    float jitterThreshold = (float)timer.Elapsed.TotalSeconds <= stationaryTimeSeconds ? minimumScaleResolutionSquared : stationaryScaleResolutionSquared;

                    // calculate distance suqared
                    float currentDistanceSquared = distance * distance;
                    float previousDistanceSquared = previousDistance * previousDistance;

                    // if a change in direction, the jitter threshold can be increased to determine whether the change in direction is significant enough
                    if ((currentDistanceSquared - previousDistanceSquared) * previousDistanceDirection < 0.0f)
                    {
                        jitterThreshold = Math.Max(jitterThreshold, hysteresisScaleResolutionSquared);
                    }

                    // check if we are above the jitter threshold - will always be true if moving in the same direction as last time
                    bool aboveJitterThreshold = ((previousDistanceSquared > jitterThreshold * currentDistanceSquared) ||
                        (currentDistanceSquared > jitterThreshold * previousDistanceSquared));

                    // must be above jitter threshold to execute
                    if (aboveJitterThreshold)
                    {
                        if (previousDistanceDirection == 0.0f)
                        {
                            SetPreviousDistance(distance, distanceX, distanceY);
                        }
                        timer.Reset();
                        timer.Start();
                        float newDistanceDirection = (currentDistanceSquared - previousDistanceSquared >= 0.0f ? 1.0f : -1.0f);
                        if (previousDistanceDirection == 0 || newDistanceDirection == previousDistanceDirection)
                        {
                            ScaleMultiplier = GetScale(distance / previousDistance);
                            ScaleMultiplierX = GetScale(distanceX / previousDistanceX);
                            ScaleMultiplierY = GetScale(distanceY / previousDistanceY);
                            SetState(EGestureActionCallbackState.Executing);
                        }
                        else
                        {
                            ScaleMultiplier = ScaleMultiplierX = ScaleMultiplierY = 1.0f;
                        }
                        previousDistanceDirection = newDistanceDirection;
                        SetPreviousDistance(distance, distanceX, distanceY);
                    }
                    else if (timer.ElapsedMilliseconds > resetDirectionMilliseconds)
                    {
                        previousDistanceDirection = 0.0f;
                    }
                }
            }
            else if (State == EGestureActionCallbackState.Began)
            {
                ScaleMultiplier = ScaleMultiplierX = ScaleMultiplierY = 1.0f;
                previousDistanceDirection = 0.0f;
                SetPreviousDistance(distance, distanceX, distanceY);
                SetState(EGestureActionCallbackState.Executing);
            }
            else
            {
                SetState(EGestureActionCallbackState.Possible);
            }
        }


        public override void TouchesBegin(IEnumerable<GestureTouch> touches)
        {
            previousDistance = 0.0f;
        }

        public override void TouchesMove()
        {
            ProcessTouches();
        }

        public override void TouchesEnd()
        {
            if (State == EGestureActionCallbackState.Executing)
            {
                CalculateFocus(this.m_CurrentTrackGestrueList);
                SetState(EGestureActionCallbackState.Ended);
            }
            else
            {
                // didn't get to the executing state, fail the gesture
                SetState(EGestureActionCallbackState.Failed);
            }
        }
        /// <summary>
        /// The current scale multiplier. Multiply your current scale value by this to scale.
        /// </summary>
        /// <value>The scale multiplier.</value>
        public float ScaleMultiplier { get; private set; }

        /// <summary>
        /// The current scale multiplier for x axis. Multiply your current scale x value by this to scale.
        /// </summary>
        /// <value>The scale multiplier.</value>
        public float ScaleMultiplierX { get; private set; }

        /// <summary>
        /// The current scale multiplier for y axis. Multiply your current scale y value by this to scale.
        /// </summary>
        /// <value>The scale multiplier.</value>
        public float ScaleMultiplierY { get; private set; }

        /// <summary>
        /// Additional multiplier for ScaleMultipliers. This will making scaling happen slower or faster. Default is 3.0.
        /// </summary>
        /// <value>The zoom speed.</value>
        public float ZoomSpeed { get; set; }

        /// <summary>
        /// Get the current scale speed in a range of -1 to 1 with ZoomSpeed applied
        /// </summary>
        public float ScaleMultiplierRange
        {
            get
            {
                if (ScaleMultiplier > 1.0f)
                {
                    return (ScaleMultiplier * ZoomSpeed);
                }
                else if (ScaleMultiplier < 1.0f)
                {
                    return ((-1.0f / ScaleMultiplier) * ZoomSpeed);
                }
                return 0.0f;
            }
        }

        /// <summary>
        /// How many units the distance between the fingers must increase or decrease from the start distance to begin executing. Default is 0.15.
        /// </summary>
        /// <value>The threshold in units</value>
        public float ThresholdUnits { get; set; }
    }
}

