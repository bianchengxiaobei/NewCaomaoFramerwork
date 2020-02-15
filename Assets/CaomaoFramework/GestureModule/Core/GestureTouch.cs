using System;
using System.Collections.Generic;
using UnityEngine;
namespace CaomaoFramework
{
    public struct GestureTouch
    {
        public int id;
        public float x;
        public float y;
        public float preX;
        public float preY;
        public TouchPhase touchPhase;
        public GestureTouch(int gestureId, float x, float y, float preX, float preY,TouchPhase touchPhase = TouchPhase.Began)
        {
            this.id = gestureId;
            this.x = x;
            this.y = y;
            this.preX = preX;
            this.preY = preY;
            this.touchPhase = touchPhase;
        }
        public GestureTouch(int gestureId, Vector2 curPos, Vector2 prePos, TouchPhase touchPhase = TouchPhase.Began)
        {
            this.id = gestureId;
            this.x = curPos.x;
            this.y = curPos.y;
            this.preX = prePos.x;
            this.preY = prePos.y;
            this.touchPhase = touchPhase;
        }
        public Vector2 GetPrePos()
        {
            return new Vector2(this.preX, this.preY);
        }
        public Vector2 GetCurPos()
        {
            return new Vector2(this.x, this.y);
        }
    }
}
