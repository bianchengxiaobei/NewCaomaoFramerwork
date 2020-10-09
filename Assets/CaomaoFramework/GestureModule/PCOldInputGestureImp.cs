using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace CaomaoFramework
{

    public enum EMouseOperatorType
    {
        None,
        Move,
        Begin,
        End
    }


    public class PCOldInputGestureImp : ICGestureModule
    {
        private const byte MouseGestureId0 = 0;
        private const byte MouseGestureId1 = 1;
        private const byte MouseGestureId2 = 2;
        private const byte MouseGestureRotationId1 = 3;
        private const byte MouseGestureRotationId2 = 4;

        private readonly List<IGestureActionCallbackBase> gestures = new List<IGestureActionCallbackBase>();
        private readonly List<IGestureActionCallbackBase> tempGestures = new List<IGestureActionCallbackBase>();


        /// <summary>
        /// 模拟手指开始旋转和缩放的距离
        /// </summary>
        [Range(1f, 10f)]
        public float MouseDistanceInUnitsRotationAndScale = 2.0f;
        [Range(0.0001f, 1f)]
        public float MouseWheelDeltaMutliplier = 0.025f;
        /// <summary>
        /// 是否需要用键盘控制鼠标缩放
        /// </summary>
        public bool RequireControlKeyForMouseZoom = true;



        private readonly List<GestureTouch> touchesMoved = new List<GestureTouch>();
        private readonly List<GestureTouch> touchesEnd = new List<GestureTouch>();
        private readonly List<GestureTouch> touchesBegin = new List<GestureTouch>();
        private readonly List<GestureTouch> touches = new List<GestureTouch>();



        private readonly List<GestureTouch> m_CurrentTouches = new List<GestureTouch>();
        private readonly List<GestureTouch> m_PrevTouches = new List<GestureTouch>();
        private readonly HashSet<GestureTouch> m_TempTouches = new HashSet<GestureTouch>();



        private Dictionary<int, Vector2> m_dicPreTouchPosition = new Dictionary<int, Vector2>();




        private bool m_bInitCamera = false;
        private DateTime lastMouseWheelTime;
        private float pinchScale = 1f;
        private float rotationAngle = 0;
        private EMouseOperatorType operatorType = EMouseOperatorType.None;
        private GestureTouch rotatePinch1;
        private GestureTouch rotatePinch2;



        public void Init()
        {

        }

        public void InitCamera()
        {
            if (this.m_bInitCamera)
            {
                return;
            }
            this.m_bInitCamera = true;
            foreach (Camera camera in Camera.allCameras)
            {
                if (camera.cameraType == CameraType.Game)
                {
                    switch (CGestureModule.GestureType)
                    {
                        case EGestureType.e_2D:
                            if (camera.GetComponent<Physics2DRaycaster>() == null)
                            {
                                camera.gameObject.AddComponent<Physics2DRaycaster>().hideFlags = HideFlags.HideAndDontSave;
                            }
                            break;
                        case EGestureType.e_3D:
                            if (camera.GetComponent<PhysicsRaycaster>() == null)
                            {
                                camera.gameObject.AddComponent<PhysicsRaycaster>().hideFlags = HideFlags.HideAndDontSave;
                            }
                            break;
                        case EGestureType.Mixed:
                            if (camera.GetComponent<PhysicsRaycaster>() == null)
                            {
                                camera.gameObject.AddComponent<PhysicsRaycaster>().hideFlags = HideFlags.HideAndDontSave;
                            }
                            if (camera.GetComponent<Physics2DRaycaster>() == null)
                            {
                                camera.gameObject.AddComponent<Physics2DRaycaster>().hideFlags = HideFlags.HideAndDontSave;
                            }
                            break;
                    }
                }
            }
        }

        public void Update()
        {
            this.ProcessTouch();
        }
        /// <summary>
        /// 鼠标键盘模拟
        /// </summary>
        private void ProcessTouch()
        {
            if (Input.mousePresent == false)
            {
                Debug.LogError("检测到不存在鼠标");
                return;
            }
            this.InitCamera();
            this.touchesBegin.Clear();
            this.touchesMoved.Clear();
            this.touchesEnd.Clear();
            this.m_CurrentTouches.Clear();

            this.ProcessMouseButton();
            this.ProcessMouseWheel();
            //this.ProcessLostTouches();
            this.tempGestures.AddRange(this.gestures);

            foreach (var gesture in this.tempGestures)
            {
                gesture.ProcessTouchBegin(this.touchesBegin);
                //Debug.LogError("Current:" + this.m_CurrentTouches.Count);
                gesture.ProcessTouchMove(this.touchesMoved);
                gesture.ProcessTouchEnd(this.touchesEnd);
            }
            this.tempGestures.Clear();
            //this.touches.Clear();
            //this.touches.AddRange(this.touchesBegin);
            //this.touches.AddRange(this.touchesMoved);
            //this.touches.AddRange(this.touchesEnd);
        }

        private void ProcessMouseButton()
        {
            if (UnityEngine.Input.mousePresent == false)
            {
                return;
            }
            Vector2 curPos = Input.mousePosition;
            this.AddMouseTouch(0, MouseGestureId0, curPos);
            this.AddMouseTouch(1, MouseGestureId1, curPos);
            this.AddMouseTouch(2, MouseGestureId2, curPos);
        }

        private void ProcessLostTouches()
        {
            // handle lost touches due to Unity bugs, Unity can not send touch end states properly
            //  and it appears that even the id's of touches can change in WebGL
            foreach (GestureTouch t in this.m_PrevTouches)
            {
                if (!this.m_CurrentTouches.Contains(t))
                {
                    this.m_TempTouches.Add(t);
                }
            }
            foreach (IGestureActionCallbackBase g in gestures)
            {
                bool reset = false;
                foreach (GestureTouch t in g.m_CurrentTrackGestrueList)
                {
                    if (!this.m_CurrentTouches.Contains(t))
                    {
                        this.m_TempTouches.Add(t);
                        reset = true;
                    }
                }
                if (reset)
                {
                    g.Reset();
                }
            }
            foreach (GestureTouch t in this.m_TempTouches)
            {
                // only end touch here, as end touch removes from previousTouches list
                GestureTouch tmp = t;
                FingersEndTouch(ref tmp, true);
                this.m_PrevTouches.Remove(tmp);
            }

            this.m_TempTouches.Clear();
        }

        private void FingersEndTouch(ref GestureTouch g, bool lost = false)
        {
            if (!lost)
            {
                //Debug.LogError("AddEndTouch");
                this.touchesEnd.Add(g);
            }
            this.m_dicPreTouchPosition.Remove(g.id);
            this.m_PrevTouches.Remove(g);
        }


        private void AddMouseTouch(int mouseType, int fingerId, Vector2 pos)
        {
            TouchPhase touchPhase;
            if (Input.GetMouseButtonDown(mouseType))
            {
                touchPhase = TouchPhase.Began;
            }
            else if (Input.GetMouseButton(mouseType))
            {
                touchPhase = TouchPhase.Moved;
            }
            else if (Input.GetMouseButtonUp(mouseType))
            {
                touchPhase = TouchPhase.Ended;
            }
            else
            {
                return;
            }
            Vector2 prePos = Vector2.zero;
            if (this.m_dicPreTouchPosition.TryGetValue(mouseType, out prePos) == false)
            {
                prePos = Input.mousePosition;
            }
            GestureTouch touch = new GestureTouch(mouseType, Input.mousePosition, prePos, touchPhase);
            this.ProcessFingerTouch(ref touch);
            prePos = Input.mousePosition;
            this.m_dicPreTouchPosition[touch.id] = prePos;
        }
        private void ProcessFingerTouch(ref GestureTouch touch)
        {
            this.m_CurrentTouches.Add(touch);
            //Debug.LogError("CurrentTouchAdd:" + touch.touchPhase);
            if (touch.touchPhase == TouchPhase.Moved || touch.touchPhase == TouchPhase.Stationary)
            {
                //Debug.LogError("PCAddMoveTouch");
                this.touchesMoved.Add(touch);
                this.m_dicPreTouchPosition[touch.id] = touch.GetCurPos();
            }
            else if (touch.touchPhase == TouchPhase.Began)
            {
                if (!this.m_PrevTouches.Contains(touch))
                {
                    this.m_PrevTouches.Add(touch);
                }
                this.touchesBegin.Add(touch);
                this.m_dicPreTouchPosition[touch.id] = touch.GetCurPos();
            }
            else
            {
                //Debug.LogError("AddEndTouch1");
                this.FingersEndTouch(ref touch);
            }
        }



        private void ProcessMouseWheel()
        {
            if (Input.mousePresent == false)
            {
                return;
            }
            var delta = Input.mouseScrollDelta;
            float scrollDelta = delta.y == 0.0f ? delta.x : delta.y * this.MouseWheelDeltaMutliplier;
            var threshold = DeviceInfo.UnitToPixels(this.MouseDistanceInUnitsRotationAndScale * 0.5f);
            this.operatorType = EMouseOperatorType.None;
            if (this.RequireControlKeyForMouseZoom == false)
            {
                if (delta == Vector2.zero)
                {
                    if (this.lastMouseWheelTime != DateTime.MinValue)
                    {
                        var whellTime = (DateTime.UtcNow - this.lastMouseWheelTime).TotalSeconds;
                        if (whellTime < 1.0f)
                        {
                            //继续滚动
                            pinchScale = Mathf.Max(0.35f, pinchScale + scrollDelta);
                            this.operatorType = EMouseOperatorType.Move;
                        }
                        else
                        {
                            //停止滚动
                            this.operatorType = EMouseOperatorType.End;
                            this.lastMouseWheelTime = DateTime.MinValue;
                        }
                    }
                }
                else if (this.lastMouseWheelTime == DateTime.MinValue)
                {
                    this.operatorType = EMouseOperatorType.Begin;
                    this.lastMouseWheelTime = DateTime.UtcNow;
                }
                else
                {
                    this.pinchScale = Mathf.Max(0.35f, this.pinchScale + scrollDelta);
                    this.operatorType = EMouseOperatorType.Move;
                    this.lastMouseWheelTime = DateTime.UtcNow;
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
            {
                this.operatorType = EMouseOperatorType.Begin;

            }
            else if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                this.operatorType = EMouseOperatorType.Move;
                this.pinchScale = Mathf.Max(0.35f, this.pinchScale + scrollDelta);
            }
            else if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
            {
                this.operatorType = EMouseOperatorType.End;
            }
            if (this.operatorType == EMouseOperatorType.None)
            {
                this.pinchScale = 1;
                this.rotationAngle = 0;
                return;
            }
            //计算旋转
            float x = Input.mousePosition.x;
            float y = Input.mousePosition.y;
            float xRot1 = x - threshold;
            float yRot1 = y;
            float xRot2 = threshold + x;
            float yRot2 = y;
            float distance = threshold * pinchScale;
            xRot1 = x - distance;
            yRot2 = y;
            xRot2 = x + distance;
            yRot2 = y;
            this.RotateAroundPoint(ref xRot1, ref yRot1, x, y, this.rotationAngle);
            this.RotateAroundPoint(ref xRot2, ref yRot2, x, y, this.rotationAngle);

            if (this.operatorType == EMouseOperatorType.Move)
            {
                this.rotatePinch1 = new GestureTouch(MouseGestureRotationId1, new Vector2(xRot1, yRot1), this.rotatePinch1.GetCurPos(), TouchPhase.Moved);
                this.rotatePinch2 = new GestureTouch(MouseGestureRotationId2, new Vector2(xRot2, yRot2), this.rotatePinch2.GetCurPos(), TouchPhase.Moved);
                this.ProcessFingerTouch(ref this.rotatePinch1);
                this.ProcessFingerTouch(ref this.rotatePinch2);
            }
            else if (this.operatorType == EMouseOperatorType.Begin)
            {
                this.rotatePinch1 = new GestureTouch(MouseGestureRotationId1, new Vector2(xRot1, yRot1), this.rotatePinch1.GetCurPos(), TouchPhase.Began);
                this.rotatePinch2 = new GestureTouch(MouseGestureRotationId2, new Vector2(xRot2, yRot2), this.rotatePinch2.GetCurPos(), TouchPhase.Began);
                this.ProcessFingerTouch(ref this.rotatePinch1);
                this.ProcessFingerTouch(ref this.rotatePinch2);
            }
            else if (this.operatorType == EMouseOperatorType.End)
            {
                this.rotatePinch1 = new GestureTouch(MouseGestureRotationId1, new Vector2(xRot1, yRot1), this.rotatePinch1.GetCurPos(), TouchPhase.Ended);
                this.rotatePinch2 = new GestureTouch(MouseGestureRotationId2, new Vector2(xRot2, yRot2), this.rotatePinch2.GetCurPos(), TouchPhase.Ended);
                this.ProcessFingerTouch(ref this.rotatePinch1);
                this.ProcessFingerTouch(ref this.rotatePinch2);
            }
        }

        private void RotateAroundPoint(ref float xRot, ref float yRot, float centerX, float centerY, float rotationAngle)
        {
            float cosTheta = Mathf.Cos(rotationAngle);
            float sinTheta = Mathf.Sin(rotationAngle);
            float x = xRot - centerX;
            float y = yRot - centerY;
            xRot = (cosTheta * x - sinTheta * y) + centerX;
            yRot = (sinTheta * x + cosTheta * y) + centerY;
        }




        public void AddGesture(IGestureActionCallbackBase callback)
        {
            this.gestures.Add(callback);
        }

        public void RemoveGesture(IGestureActionCallbackBase callback)
        {
            this.gestures.Remove(callback);
        }
    }
}
