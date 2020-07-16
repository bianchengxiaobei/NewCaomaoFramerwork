using System;
using System.Collections.Generic;
using UnityEngine;
namespace CaomaoFramework
{
    public class MobileInputGestureImp : ICGestureModule
    {
        private readonly Dictionary<int, Vector2> m_PreviousTouchPositions = new Dictionary<int, Vector2>();
        private readonly HashSet<int> isTouchDown = new HashSet<int>();//是否touch已经按下去了
        private readonly List<GestureTouch> touchesBegan = new List<GestureTouch>();
        private readonly List<GestureTouch> touchesMoved = new List<GestureTouch>();
        private readonly List<GestureTouch> touchesEnded = new List<GestureTouch>();

        private readonly List<IGestureActionCallbackBase> gestures = new List<IGestureActionCallbackBase>();//手势

        private readonly List<GestureTouch> touches = new List<GestureTouch>();



        public void Init()
        {

        }

        public void Update()
        {
            this.touchesBegan.Clear();
            this.touchesMoved.Clear();
            this.touchesEnded.Clear();
            this.ProcessTouch();
            foreach (var gesture in this.gestures)
            {
                gesture.ProcessTouchBegin(this.touchesBegan);
                gesture.ProcessTouchMove(this.touchesMoved);
                gesture.ProcessTouchEnd(this.touchesEnded);
            }

            this.touches.Clear();
            this.touches.AddRange(this.touchesBegan);
            this.touches.AddRange(this.touchesMoved);
            this.touches.AddRange(this.touchesEnded);
        }
        private void ProcessTouch()
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                var touch = Input.GetTouch(i);
                //MobileOperator.lb_debugInfo.text = "Touch:" + touch.fingerId;
                this.ProcessTouch(ref touch);
            }
        }

        private ICollection<GestureTouch> FilterTouchBegin(ICollection<GestureTouch> touches, IGestureActionCallbackBase gestureCallback)
        {
            //这里没有过滤，应该判断是否该gameobject是能够touch?
            return touches;
        }


        private void ProcessTouch(ref Touch touch)
        {
            GestureTouch g;
            this.GestureTouchFromTouch(ref touch, out g);
            if (this.isTouchDown.Contains(touch.fingerId))
            {
                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    this.touchesMoved.Add(g);
                    this.m_PreviousTouchPositions[touch.fingerId] = touch.position;
                }
                else if (touch.phase != TouchPhase.Began)
                {
                    this.touchesEnded.Add(g);
                    this.isTouchDown.Remove(touch.fingerId);
                    this.m_PreviousTouchPositions.Remove(touch.fingerId);
                }
            }
            else
            {
                if (touch.phase != TouchPhase.Ended &&
                    touch.phase != TouchPhase.Canceled)
                {
                    this.touchesBegan.Add(g);
                    this.isTouchDown.Add(touch.fingerId);
                    this.m_PreviousTouchPositions[touch.fingerId] = touch.position;
                }
            }
        }
        private void GestureTouchFromTouch(ref Touch t, out GestureTouch g)
        {
            if (!this.m_PreviousTouchPositions.TryGetValue(t.fingerId, out Vector2 prev))
            {
                prev.x = t.position.x;
                prev.y = t.position.y;
            }
            g = new GestureTouch(t.fingerId, t.position, prev);
        }

        public void AddGesture(IGestureActionCallbackBase callback)
        {
            if (this.gestures != null)
            {
                if (!this.gestures.Contains(callback))
                {
                    this.gestures.Add(callback);
                }
            }
        }
        public void RemoveGesture(IGestureActionCallbackBase callback)
        {
            if (this.gestures != null && this.gestures.Count > 0)
            {
                this.gestures.Remove(callback);
            }
        }
    }
}


