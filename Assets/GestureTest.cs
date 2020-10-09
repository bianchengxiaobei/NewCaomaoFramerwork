using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CaomaoFramework;
/// <summary>
/// 手势测试和摄像机测试
/// </summary>
public class GestureTest : MonoBehaviour
{
    private PanGestureCallback panGesture;
    private ScaleGestureCallback scaleGesture;
    public CameraController cameraController;
    private ICGestureModule module;
    private float Dir = 1;
    private int DelayScaleFrameCount;
    public int ScaleFrameCountInpector = 2;
    public Text lb_debug;
    private void Awake()
    {
        this.module = new CGestureModule();
        this.module.Init();
        this.panGesture = new PanGestureCallback();
        this.panGesture.Init(this);
        this.panGesture.StateUpdated += this.PanUpdate;
        this.scaleGesture = new ScaleGestureCallback();
        this.scaleGesture.Init(this);
        this.scaleGesture.StateUpdated += this.ScaleUpdate;
        this.module.AddGesture(this.panGesture);
        this.module.AddGesture(this.scaleGesture);
        if (this.cameraController == null)
        {
            this.cameraController = Camera.main.GetComponent<CameraController>();
            if (this.cameraController == null)
            {
                Debug.LogError("CameraController == null");
            }
        }
        this.DelayScaleFrameCount = this.ScaleFrameCountInpector;
    }


    private void Update()
    {
        if (this.module != null)
        {
            this.module.Update();
        }
#if UNITY_EDITOR
        var asValue = Input.GetAxis("Mouse ScrollWheel");
        asValue *= -1f;
        if (asValue != 0f)
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector2 vector = new Vector2(mousePosition.x, mousePosition.y);
            this.cameraController.WheelScrolled(vector, asValue);
        }
#endif
    }

    private void ScaleUpdate(IGestureActionCallbackBase gesture)
    {
        //Debug.Log(gesture.State);
#if UNITY_EDITOR == false
        //this.lb_debug.text = gesture.State.ToString();
        if (gesture.State == EGestureActionCallbackState.Executing)
        {
            //默认2帧执行一次
            this.DelayScaleFrameCount++;
            if (this.DelayScaleFrameCount >= this.ScaleFrameCountInpector)
            {
                this.DelayScaleFrameCount = 0;
            }
            else
            {
                return;
            }
            var scrollValue = this.scaleGesture.ScaleMultiplier;
            this.lb_debug.text = scrollValue.ToString();
            var leftValue = scrollValue - 1;
            if (leftValue > 0)
            {
                this.Dir = -1f;
            }
            else if (leftValue < 0)
            {
                this.Dir = 1f;
            }
            this.cameraController.WheelScrolled(gesture.FocuesPos, this.Dir);
        }
        else if (gesture.State == EGestureActionCallbackState.Ended)
        {
            this.DelayScaleFrameCount = this.ScaleFrameCountInpector;
        }
#endif
    }

    private void PanUpdate(IGestureActionCallbackBase gesture)
    {
        //Debug.Log(gesture.State);
        lb_debug.text = gesture.State.ToString();
        if (gesture.State == EGestureActionCallbackState.Began)
        {
            //刚开始拖动
            this.cameraController.BeginDrag(gesture.FocuesPos);
        }
        else if (gesture.State == EGestureActionCallbackState.Executing)
        {
            this.cameraController.DragMove(gesture.FocuesPos);
        }
        else if (gesture.State == EGestureActionCallbackState.Ended)
        {
            this.cameraController.EndDrag();
        }
    }
}

