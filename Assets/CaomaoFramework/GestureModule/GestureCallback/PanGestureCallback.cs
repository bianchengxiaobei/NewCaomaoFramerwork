using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
namespace CaomaoFramework
{
    /// <summary>
    /// 移动手势
    /// </summary>
    public class PanGestureCallback : IGestureActionCallbackBase
    {

        public float ThresholdUnits { get; set; } = 0.2f;
        private Vector2 StartPos = Vector2.zero;
        private readonly Stopwatch timeBelowSpeedUnitsToRestartThresholdUnits = new Stopwatch();
        private bool needsDistanceThreshold;
        public float SpeedUnitsToRestartThresholdUnits { get; set; } = 0.1f;
        public float TimeToRestartThresholdUnits { get; set; } = 0.2f;
        private void ProcessTouches(bool resetFocus)
        {
            bool firstFocus = CalculateFocus(this.m_CurrentTrackGestrueList, resetFocus);
            UnityEngine.Debug.Log(firstFocus);
            if (firstFocus)
            {
                this.timeBelowSpeedUnitsToRestartThresholdUnits.Reset();
                this.timeBelowSpeedUnitsToRestartThresholdUnits.Restart();
                if (ThresholdUnits <= 0)
                {
                    this.needsDistanceThreshold = false;
                    this.SetState(EGestureActionCallbackState.Began);
                }
                else
                {
                    this.needsDistanceThreshold = true;
                    this.SetState(EGestureActionCallbackState.Possible);
                }
                this.StartPos = this.FocuesPos;
            }
            else if (!needsDistanceThreshold && (State == EGestureActionCallbackState.Began || State == EGestureActionCallbackState.Executing))
            {
                if (SpeedUnitsToRestartThresholdUnits > 0.0f && DistanceVector(this.Velocity) < SpeedUnitsToRestartThresholdUnits &&
                    (float)timeBelowSpeedUnitsToRestartThresholdUnits.Elapsed.TotalSeconds >= TimeToRestartThresholdUnits)
                {
                    if (!needsDistanceThreshold)
                    {
                        needsDistanceThreshold = true;
                        this.StartPos = this.FocuesPos;
                    }
                }
                else
                {
                    timeBelowSpeedUnitsToRestartThresholdUnits.Reset();
                    timeBelowSpeedUnitsToRestartThresholdUnits.Start();
                    UnityEngine.Debug.Log("Executing");
                    SetState(EGestureActionCallbackState.Executing);
                }
            }
            else if (TrackedTouchCountIsWithinRange)
            {
                if (needsDistanceThreshold)
                {
                    var vetor = this.FocuesPos - this.StartPos;
                    float distance = this.DistanceVector(vetor);
                    if (distance >= ThresholdUnits)
                    {
                        needsDistanceThreshold = false;
                        SetState(EGestureActionCallbackState.Began);
                    }
                    else if (State != EGestureActionCallbackState.Executing)
                    {
                        SetState(EGestureActionCallbackState.Possible);
                    }
                }
                else
                {
                    SetState(EGestureActionCallbackState.Possible);
                }
            }
        }
        public override void TouchesBegin(IEnumerable<GestureTouch> touches)
        {
            //base.TouchesBegin(touches);
            this.ProcessTouches(true);
        }
        public override void TouchesMove()
        {
            //base.TouchesMove();
            UnityEngine.Debug.Log("TouchMove");
            this.ProcessTouches(false);
        }
        public override void TouchesEnd()
        {
            this.ProcessTouches(false);
            if (this.State == EGestureActionCallbackState.Possible)
            {
                this.SetState(EGestureActionCallbackState.Failed);
            }
            else
            {
                this.SetState(EGestureActionCallbackState.Ended);
            }
        }
    }
}
