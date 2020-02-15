using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace CaomaoFramework
{
    public class PCOldInputGestureImp : ICGestureModule
    {
        private const byte MouseGestureId0 = 0;
        private const byte MouseGestureId1 = 1;
        private const byte MouseGestureId2 = 2;

        private Dictionary<int, Vector2> m_dicPreTouchPosition = new Dictionary<int, Vector2>();
        private bool m_bInitCamera = false;
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
            this.CheckMouseButton(0);
            this.CheckMouseButton(1);
            this.CheckMouseButton(2);
        }
        private void CheckMouseButton(int mouseType)
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
            if (this.m_dicPreTouchPosition.TryGetValue(mouseType,out prePos) == false)
            {
                prePos = Input.mousePosition;
            }
            GestureTouch touch = new GestureTouch(mouseType, Input.mousePosition, prePos, touchPhase);
            switch (touchPhase)
            {
                case TouchPhase.Began:
                    this.m_dicPreTouchPosition[touch.id] = touch.GetCurPos();
                    break;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    this.m_dicPreTouchPosition[touch.id] = touch.GetCurPos();
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    this.m_dicPreTouchPosition.Remove(touch.id);
                    break;
            }
        }
    }
}
