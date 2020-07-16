using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
namespace CaomaoFramework
{
    public class IGestureActionCallbackBase
    {
        private static readonly IGestureActionCallbackBase AllRefrenceGestureCallback = new IGestureActionCallbackBase();
        public EGestureActionCallbackState State { get; set; } = EGestureActionCallbackState.Possible;
        public readonly List<GestureTouch> m_CurrentTrackGestrueList = new List<GestureTouch>();
        private readonly HashSet<IGestureActionCallbackBase> m_FailedGestureCallbackList = new HashSet<IGestureActionCallbackBase>();
        private readonly GestureVelocityTracker velocityTracker = new GestureVelocityTracker();
        private readonly List<GestureTouch> m_TempGestureList = new List<GestureTouch>();
        private readonly List<IGestureActionCallbackBase> m_SimultaneousGesturesCallbackList = new List<IGestureActionCallbackBase>();
        private readonly HashSet<IGestureActionCallbackBase> m_RequireGestureRecognizersToFail = new HashSet<IGestureActionCallbackBase>();

        internal static readonly HashSet<IGestureActionCallbackBase> ActiveGestues = new HashSet<IGestureActionCallbackBase>();
        //private bool bClearTrackGestureList = false;//是否清除Track

        private Vector2 m_Velocity = Vector2.zero;

        public bool bRestarting { get; set; }
        /// <summary>
        /// 是否手势结束的时候reset
        /// </summary>
        public bool ResetWhenEnd { get; set; } = true;
        private bool m_bJustEnd = false;
        private bool m_bJustFailed = false;
        private int lastTrackTouchCount = 0;
        /// <summary>
        /// 是否在执行的过程最后阶段添加touch
        /// </summary>
        public bool ReceiveAdditionalTouches { get; set; }
        private bool m_bInit = false;//是否已经初始化
        private MonoBehaviour m_MonoScript;
        /// <summary>
        /// 是否允许同时执行Gesture，默认为true
        /// </summary>
        public bool AllowSimultaneousExecutionIfPlatformSpecificViewsAreDifferent { get; set; } = true;
        /// <summary>
        /// 平台特殊视图缩放，默认为1
        /// </summary>
        public float PlatformSpecificViewScale { get; set; } = 1f;
        /// <summary>
        /// 是否在结束状态清除Track的touches
        /// </summary>

        public bool WhenEndClearTrackTouches { get; set; } = false;



        private int minimumNumberOfTouchesToTrack = 1;
        private int maximumNumberOfTouchesToTrack = 2;
        //public int MaximumNumberOfTouchesToTrack = 1;
        public Action<IGestureActionCallbackBase> StateUpdated;//状态更新委托



        public static WaitForSecondsRealtime WaitTimeToRemove;


        /// <summary>
        /// 是否追踪的touch数量超过最大最小值
        /// </summary>
        public bool TrackedTouchCountIsWithinRange
        {
            get { return this.m_CurrentTrackGestrueList.Count >= minimumNumberOfTouchesToTrack && this.m_CurrentTrackGestrueList.Count <= maximumNumberOfTouchesToTrack; }
        }

        public int MaximumNumberOfTouchesToTrack
        {
            get
            {
                return this.maximumNumberOfTouchesToTrack;
            }
            set
            {
                this.maximumNumberOfTouchesToTrack = value < 1 ? 1 : value;
            }
        }

        public int MinimumNumberOfTouchesToTrack
        {
            get
            {
                return this.minimumNumberOfTouchesToTrack;
            }
            set
            {
                this.minimumNumberOfTouchesToTrack = value < 0 ? 1 : value;
            }
        }


        //坐标点的信息

        public Vector2 FocuesPos { get; private set; } = Vector2.zero;
        public Vector2 PrevFocusPos { get; private set; } = Vector2.zero;

        public Vector2 StartFocusPos { get; private set; } = Vector2.zero;
        public Vector2 DeltaPos { get; private set; } = Vector2.zero;
        public Vector2 Distance { get; private set; } = Vector2.zero;
        public Vector2 Velocity
        {
            get
            {
                return this.velocityTracker.Veloctiy;
            }
        }
        public float Speed => this.velocityTracker.Speed;
        /// <summary>
        /// 压力
        /// </summary>
        public float Pressure { get; private set; }



        public void Init(MonoBehaviour drive)
        {
            if (this.m_bInit)
            {
                return;
            }
            if (drive == null)
            {
                return;
            }
            this.m_bInit = true;
            if (WaitTimeToRemove == null)
            {
                WaitTimeToRemove = new WaitForSecondsRealtime(0.001f);
            }
            if (this.m_MonoScript == null)
            {
                this.SetMono(drive);
            }
        }

        public void SetMono(MonoBehaviour mono)
        {
            this.m_MonoScript = mono;
        }


        public void ProcessTouchBegin(ICollection<GestureTouch> touches)
        {
            if (touches == null || touches.Count == 0)
            {
                //Debug.Log("No Touch");
                return;
            }
            else
            {
                var isInState = this.State == EGestureActionCallbackState.Possible ||
                this.State == EGestureActionCallbackState.Began || this.State == EGestureActionCallbackState.Executing;
                //Debug.LogError("IsInStataet:" + isInState);
                var addTrackTouchCount = this.TrackTouches(touches);
                //Debug.LogError("AddTrackCount:" + addTrackTouchCount);
                if (isInState && addTrackTouchCount > 0)
                {
                    if (this.m_CurrentTrackGestrueList.Count > this.maximumNumberOfTouchesToTrack)
                    {
                        //Debug.Log("SetFailed:" + this.m_CurrentTrackGestrueList.Count);
                        this.SetState(EGestureActionCallbackState.Failed);
                    }
                    else
                    {
                        //Debug.Log("TouchBegin");
                        this.TouchesBegin(touches);
                    }
                }
            }

        }

        protected int TrackTouches(IEnumerable<GestureTouch> touches)
        {
            var count = this.TrackTouchesInternal(touches);
            //Debug.LogError("TarckTouchCount:" + count);
            return count;
        }
        private int TrackTouchesInternal(IEnumerable<GestureTouch> touches)
        {
            int count = 0;
            foreach (GestureTouch touch in touches)
            {
                // always track all touches in possible state, allows failing gesture if too many touches
                // do not track higher than the max touch count if in another state
                if ((State == EGestureActionCallbackState.Possible || this.m_CurrentTrackGestrueList.Count < maximumNumberOfTouchesToTrack) &&
                    !this.m_CurrentTrackGestrueList.Contains(touch))
                {
                    //Debug.LogError("AddTrackTouch");
                    this.m_CurrentTrackGestrueList.Add(touch);
                    count++;
                }
            }
            if (this.m_CurrentTrackGestrueList.Count > 1)
            {
                this.m_CurrentTrackGestrueList.Sort();
            }
            return count;
        }

        public virtual void TouchesBegin(IEnumerable<GestureTouch> touches)
        {

        }

        public virtual void TouchesMove()
        {

        }


        public void ProcessTouchMove(ICollection<GestureTouch> touches)
        {
            if (touches == null || touches.Count == 0)
            {
                return;
            }
            var inte = this.TouchesIntersect(touches, this.m_CurrentTrackGestrueList);
            //Debug.LogError("Intersect:" + inte);
            //Debug.LogError("Current:" + this.m_CurrentTrackGestrueList.Count);
            if (inte == false)
            {
                return;
            }
            //else if (this.State == EGestureActionCallbackState.Ended ||
            //    this.State == EGestureActionCallbackState.EndPending)
            else if (this.m_CurrentTrackGestrueList.Count > this.MaximumNumberOfTouchesToTrack
                || (this.State != EGestureActionCallbackState.Executing && this.State != EGestureActionCallbackState.Began
                && this.State != EGestureActionCallbackState.Possible))
            {
                this.SetState(EGestureActionCallbackState.Failed);
            }
            else if (!this.EndGestureRestart(touches))
            {
                //Debug.Log("Move");
                this.UpdateTrackGesture(touches);
                this.TouchesMove();
            }
        }

        private void UpdateTrackGesture(ICollection<GestureTouch> touches)
        {
            int count = 0;
            foreach (GestureTouch touch in touches)
            {
                for (int i = 0; i < this.m_CurrentTrackGestrueList.Count; i++)
                {
                    if (this.m_CurrentTrackGestrueList[i].id == touch.id)
                    {
                        this.m_CurrentTrackGestrueList[i] = touch;
                        count++;
                        break;
                    }
                }
            }
            if (count != 0)
            {
                this.m_CurrentTrackGestrueList.Sort();
            }
        }

        private bool EndGestureRestart(ICollection<GestureTouch> touches)
        {
            if (this.bRestarting)
            {
                foreach (var touch in touches)
                {
                    if (this.m_CurrentTrackGestrueList.Contains(touch))
                    {
                        this.m_TempGestureList.Add(touch);
                    }
                }
                Debug.LogError("ClearTrack2");
                this.m_CurrentTrackGestrueList.Clear();
                this.ProcessTouchBegin(this.m_TempGestureList);
                this.m_TempGestureList.Clear();
                this.bRestarting = false;
                return true;
            }
            return false;
        }

        private bool CanExecuteGestureWithOtherGesturesOrFail(EGestureActionCallbackState value)
        {
            // if we are trying to execute from a non-executing state and there are gestures already executing,
            // we need to make sure we are allowed to execute simultaneously
            if (ActiveGestues.Count != 0 &&
            (
                value == EGestureActionCallbackState.Began ||
                value == EGestureActionCallbackState.Executing ||
                value == EGestureActionCallbackState.Ended
            ) && this.State != EGestureActionCallbackState.Began && this.State != EGestureActionCallbackState.Executing)
            {
                // check all the active gestures and if any are not allowed to simultaneously
                // execute with this gesture, fail this gesture immediately
                foreach (IGestureActionCallbackBase gesture in ActiveGestues)
                {
                    if (gesture != this &&
                        (!AllowSimultaneousExecutionIfPlatformSpecificViewsAreDifferent &&
                        !this.m_SimultaneousGesturesCallbackList.Contains(gesture) &&
                        !gesture.m_SimultaneousGesturesCallbackList.Contains(this) &&
                        !m_SimultaneousGesturesCallbackList.Contains(AllRefrenceGestureCallback) &&
                        !gesture.m_SimultaneousGesturesCallbackList.Contains(AllRefrenceGestureCallback)))
                    {
                        this.FailedGestureNow();
                        //FailGestureNow();
                        return false;
                    }
                }
            }
            return true;
        }
        private void UpdateTouchState(bool executing)
        {
            if (executing && this.lastTrackTouchCount != this.m_CurrentTrackGestrueList.Count)
            {
                this.ReceiveAdditionalTouches = true;
                this.lastTrackTouchCount = this.m_CurrentTrackGestrueList.Count;
            }
            else
            {
                this.ReceiveAdditionalTouches = false;
            }
        }
        public bool SetState(EGestureActionCallbackState state)
        {
            Debug.Log("SetState:" + state);
            if (state == EGestureActionCallbackState.Failed)
            {
                //Debug.LogError("SetFailed");
                this.FailedGestureNow();
                return true;
            }
            else if (this.CanExecuteGestureWithOtherGesturesOrFail(state) == false)
            {
                return false;
            }
            else if (state == EGestureActionCallbackState.Ended && this.CheckRequiredGesturesToFail())
            {
                this.State = EGestureActionCallbackState.EndPending;
                return false;
            }
            else
            {
                if (state == EGestureActionCallbackState.Began || state == EGestureActionCallbackState.Executing)
                {
                    this.State = state;
                    ActiveGestues.Add(this);
                    this.UpdateTouchState(state == EGestureActionCallbackState.Executing);
                    this.StateChange();
                }
                else if (state == EGestureActionCallbackState.Ended)
                {
                    EndGesture();

                    // end after a one frame delay, this allows multiple gestures to properly
                    // fail if no simulatenous execution allowed and there were multiple ending at the same frame
                    ActiveGestues.Add(this);
                    if (this.m_MonoScript != null)
                    {
                        this.m_MonoScript.StartCoroutine(this.RemoveActionDelay(this.RemoveActiveGestureCallback));
                    }
                    else
                    {
                        Debug.LogError("No Mono");
                    }
                    //RunActionAfterDelay(0.001f, RemoveFromActiveGestures);
                }
                else
                {
                    this.State = state;
                    this.StateChange();
                }
            }
            return true;
        }

        /// <summary>
        /// Gets the distance between two points, in units
        /// </summary>
        /// <returns>The distance between the two points in units.</returns>
        /// <param name="x1">The first x value in pixels.</param>
        /// <param name="y1">The first y value in pixels.</param>
        /// <param name="x2">The second x value in pixels.</param>
        /// <param name="y2">The second y value in pixels.</param>
        public float DistanceBetweenPoints(float x1, float y1, float x2, float y2)
        {
            float a = (float)(x2 - x1);
            float b = (float)(y2 - y1);
            float d = (float)Math.Sqrt(a * a + b * b) * PlatformSpecificViewScale;
            return DeviceInfo.PixelsToUnit(d);
        }



        public bool CalculateFocus(ICollection<GestureTouch> touches)
        {
            return this.CalculateFocus(touches, false);
        }

        public bool CalculateFocus(ICollection<GestureTouch> touches, bool resetFocus)
        {
            if (touches.Count <= 0)
            {
                Debug.LogError("Touch Count < 0");
                return false;
            }
            bool first = resetFocus || this.StartFocusPos.x == float.MinValue || this.StartFocusPos.y == float.MinValue;
            this.PrevFocusPos = FocuesPos;
            this.FocuesPos = Vector2.zero;
            this.Pressure = 0f;

            foreach (var touch in touches)
            {
                this.FocuesPos += touch.GetCurPos();
                this.Pressure += 1;//默认压力为1f
            }
            float invTouchCount = 1 / touches.Count;
            this.FocuesPos = this.FocuesPos * invTouchCount;
            this.Pressure = this.Pressure * invTouchCount;
            if (first)
            {
                this.StartFocusPos = this.FocuesPos;
                this.DeltaPos = Vector2.zero;
                this.velocityTracker.Restart();
            }
            else
            {
                this.DeltaPos = this.FocuesPos - this.PrevFocusPos;
            }
            this.velocityTracker.Update(this.FocuesPos);
            this.Distance = this.FocuesPos - this.StartFocusPos;
            return first;
        }

        public float DistanceVector(Vector2 distance)
        {
            var sqr = Mathf.Sqrt(distance.x * distance.x + distance.y * distance.y);
            var sqrPlatform = sqr * this.PlatformSpecificViewScale;
            return sqrPlatform / (int)Screen.dpi;
        }

        public float DistanceVector(float length)
        {
            float d = Mathf.Abs(length) * PlatformSpecificViewScale;
            return DeviceInfo.UnitToPixels(d);
        }


        private IEnumerator RemoveActionDelay(Action callback)
        {
            yield return WaitTimeToRemove;
            callback?.Invoke();
        }

        private void EndGesture()
        {
            this.State = EGestureActionCallbackState.Ended;
            this.StateChange();
            if (this.ResetWhenEnd)
            {
                this.PrivateReset(this.WhenEndClearTrackTouches);
            }
            else
            {
                this.State = EGestureActionCallbackState.Possible;
                this.RemoveActiveGestureCallback();
            }
        }

        private bool CheckRequiredGesturesToFail()
        {
            if (this.m_RequireGestureRecognizersToFail.Count > 0)
            {
                bool returnedValue = true;
                using (HashSet<IGestureActionCallbackBase>.Enumerator gestureToFailEnumerator = this.m_RequireGestureRecognizersToFail.GetEnumerator())
                {
                    while (gestureToFailEnumerator.MoveNext() && returnedValue)
                    {
                        returnedValue &= gestureToFailEnumerator.Current.State == EGestureActionCallbackState.Possible &&
                            (gestureToFailEnumerator.Current.m_CurrentTrackGestrueList.Count != 0 || gestureToFailEnumerator.Current.m_bJustEnd) &&
                            !gestureToFailEnumerator.Current.m_bJustFailed;
                    }
                }
                return returnedValue;
            }
            return false;
        }

        private void RemoveActiveGestureCallback()
        {
            ActiveGestues.Remove(this);
        }
        /// <summary>
        /// 让手势直接失效
        /// </summary>
        private void FailedGestureNow()
        {
            this.State = EGestureActionCallbackState.Failed;
            this.RemoveActiveGestureCallback();
            this.StateChange();
            foreach (var failed in this.m_FailedGestureCallbackList)
            {
                if (failed.State == EGestureActionCallbackState.EndPending)
                {
                    failed.SetState(EGestureActionCallbackState.Ended);
                }
            }
            this.PrivateReset(this.WhenEndClearTrackTouches);
            this.m_bJustFailed = true;
            this.lastTrackTouchCount = 0;
            this.ReceiveAdditionalTouches = false;
        }

        public virtual void StateChange()
        {
            //Debug.Log("StateChange");
            this.StateUpdated?.Invoke(this);
            //如果还有失败的手势，直接设置结束
            if (this.m_FailedGestureCallbackList.Count > 0 && (this.State == EGestureActionCallbackState.Began
                || this.State == EGestureActionCallbackState.Executing || this.State == EGestureActionCallbackState.Possible))
            {
                foreach (var failed in this.m_FailedGestureCallbackList)
                {
                    failed.FailedGestureNow();
                }
            }
        }

        public void Reset()
        {
            this.PrivateReset(true);
        }

        private void PrivateReset(bool clearTrack)
        {
            if (clearTrack)
            {
                Debug.LogError("ClearTrack");
                this.m_CurrentTrackGestrueList.Clear();
            }
            this.StartFocusPos = this.PrevFocusPos = Vector2.one * float.MinValue;
            this.Distance = this.DeltaPos = this.FocuesPos = Vector2.zero;
            this.Pressure = 0;
            this.velocityTracker.Reset();
            this.RemoveActiveGestureCallback();
            this.SetState(EGestureActionCallbackState.Possible);
        }

        private bool TouchesIntersect(ICollection<GestureTouch> touches, List<GestureTouch> trackList)
        {
            foreach (var t in touches)
            {
                for (int i = 0; i < trackList.Count; i++)
                {
                    if (trackList[i].id == t.id)
                    {
                        return true;
                    }
                }
            }
            return false;
        }



        public void ProcessTouchEnd(ICollection<GestureTouch> touches)
        {
            if (touches == null || touches.Count == 0)
            {
                Debug.LogError("No Touch");
                return;
            }
            Debug.LogError("End:" + touches.Count);
            //if (this.State == EGestureActionCallbackState.Ended || this.State == EGestureActionCallbackState.EndPending)
            //{
            //    this.SetState(EGestureActionCallbackState.Failed);
            //}
            if (this.TrackedTouchCountIsWithinRange == false ||
                (State != EGestureActionCallbackState.Possible &&
                State != EGestureActionCallbackState.Began &&
                State != EGestureActionCallbackState.Executing))
            {
                Debug.LogError("Failed:" + this.State);
                Debug.LogError("trackFailed:" + this.m_CurrentTrackGestrueList.Count);
                this.FailedGestureNow();
            }
            else if (this.TouchesIntersect(touches, this.m_CurrentTrackGestrueList))
            {
                this.UpdateTrackGesture(touches);
                this.TouchesEnd();
            }
            this.StopTrackingTouches(touches);
            this.m_bJustEnd = true;
        }
        private int StopTrackingTouches(ICollection<GestureTouch> touches)
        {
            if (touches == null || touches.Count == 0)
            {
                return 0;
            }
            int count = 0;
            foreach (GestureTouch t in touches)
            {
                for (int i = 0; i < this.m_CurrentTrackGestrueList.Count; i++)
                {
                    if (this.m_CurrentTrackGestrueList[i].id == t.id)
                    {
                        Debug.LogError("RemoveTrack:" + t.id);
                        this.m_CurrentTrackGestrueList.RemoveAt(i);
                        count++;
                        break;
                    }
                }
            }
            return count;
        }
        public virtual void TouchesEnd()
        {

        }
    }
}


