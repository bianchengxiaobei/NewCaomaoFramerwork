using System;
using System.Collections.Generic;
using UnityEngine;
using CaomaoFramework;
public class CameraController : MonoBehaviour
{
    public Camera TargetCamera;

    public bool RestrictCamera = true;

    public float WheelScrollRate = 1.5f;
    public float MouseRotationRateX = 4f;
    public float MouseRotationRateY = 2f;
    public float TouchLowRotateRate = 2f;
    public float Damping = 8f;
    public float MinCameraHeight = 1.5f;//摄像机最低高度
    public bool TiltWithZoom = true;//是否斜着摄像机zoom
    public AnimationCurve camCurve = new AnimationCurve();//摄像机zoom的动画曲线
    public float MaxHeight = 500f;//摄像机最大的高度


    public float PivotPointUpScreen = 0.33f;
    private bool panXActive;

    private bool panZActive;

    private float keyPanX;

    private float keyPanZ;

    private Vector3 keyPanAmount = Vector3.zero;

    private int keyZoomActive;

    private bool DRZRotateStarted;

    private bool DRZPinchStarted;

    private Vector3 plannedTargetPos;

    private Quaternion plannedTargetRot;

    private bool plannedMoveActive;

    private Vector3 focusTargetPos;

    private Vector3 focusTargetRot;

    private bool followFocusActive;

    private bool followFocusOver;

    private Vector3 focusWorldPos;

    private Vector3 focusCameraStartPos;

    private Vector3 focusStartRot;

    private float focusZoomAmount;

    private float focusRotY;

    private float focusRotX;

    private Vector2 focusCurrentTouchPos;

    private Vector2 focusDragStart;

    private bool focusDoDrag;

    private bool focusAdjustXRotWithZoom;

    private Vector3 focusLastValidDragPoint;

    private float cam2ZoomAngle;

    private float lastUpdateTime;

    private float minX = float.MinValue;

    private float maxX = float.MaxValue;

    private float minZ = float.MinValue;

    private float maxZ = float.MaxValue;

    private void Awake()
    {
        if (this.TargetCamera == null)
        {
            this.TargetCamera = this.GetComponent<Camera>();
            if (this.TargetCamera == null)
            {
                this.TargetCamera = Camera.main;
            }
        }
        if (this.PivotPointUpScreen > 1f)
        {
            this.PivotPointUpScreen = 0.8f;
        }
        else if (this.PivotPointUpScreen < 0f)
        {
            this.PivotPointUpScreen = 0.33f;
        }
#if UNITY_EDITOR
        this.WheelScrollRate = 1.5f;
#endif
    }


    private void Update()
    {
        float deltaTime = Time.realtimeSinceStartup - this.lastUpdateTime;
        if (this.lastUpdateTime == 0f)
        {
            deltaTime = 0f;
        }
        this.lastUpdateTime = Time.realtimeSinceStartup;
        if (deltaTime > 0.2f)
        {
            deltaTime = 0.2f;
        }
        this.FollowTargets(deltaTime);
        //Debug.Log("ZoomAmout:" + this.focusZoomAmount);
    }


    private void MoveFocusPos(Vector3 moveAmount)
    {
        Vector3 vector = this.focusWorldPos;
        vector += moveAmount;
        vector = this.GetLimitedPosition(vector);
        //float apparentLimitedHeightAtPoint = HeightMapManager.GetApparentLimitedHeightAtPoint(vector.x, vector.z);
        //vector.y = apparentLimitedHeightAtPoint;
        this.focusCameraStartPos += vector - this.focusWorldPos;
        this.focusWorldPos = vector;
    }

    private void FollowTargets(float deltaTime)
    {
        if (this.plannedMoveActive)
        {
            Debug.LogError("PlaneMove");
            this.FollowPlannedMove(deltaTime);
        }
        else if (this.followFocusActive)
        {
            if (this.keyPanX != 0f || this.keyPanZ != 0f)
            {
                this.UpdateKeyPanAmount(deltaTime);
                this.MoveFocusPos(this.keyPanAmount);
            }
            this.UpdateKeyZoom(deltaTime);
            this.UpdateFocusTarget();
            this.FollowFocus(deltaTime);
        }
        else
        {
            this.DoKeyPan(deltaTime);
            this.UpdateKeyZoom(deltaTime);
        }
    }
    private void FollowPlannedMove(float deltaTime)
    {
        Vector3 position = this.transform.position;
        float t = Mathf.Min(deltaTime * this.Damping, 0.75f);
        Vector3 vector = Vector3.Lerp(position, this.plannedTargetPos, t);
        this.transform.position = vector;
        float sqrMagnitude = (vector - this.plannedTargetPos).sqrMagnitude;
        bool flag = sqrMagnitude <= 1f;
        Quaternion rotation = this.transform.rotation;
        Quaternion rotation2 = Quaternion.Slerp(rotation, this.plannedTargetRot, t);
        this.transform.rotation = rotation2;
        bool flag2 = this.TestEulerAnglesInRange(rotation2.eulerAngles, this.plannedTargetRot.eulerAngles, 0.1f);
        if (flag2 && flag)
        {
            this.plannedMoveActive = false;
        }
    }
    private Vector3 GetPannedPosition(Vector3 startPos, float xMove, float zMove)
    {
        Vector3 vector = new Vector3(xMove, 0f, zMove);
        Vector3 eulerAngles = this.TargetCamera.transform.eulerAngles;
        eulerAngles.x = 0f;
        Quaternion rotation = Quaternion.Euler(eulerAngles);
        vector = rotation * vector;
        startPos += vector;
        return startPos;
    }
    private void UpdateKeyPanAmount(float timeDelta)
    {
        float num = Mathf.Sign(this.keyPanX) * Mathf.Sqrt(Mathf.Abs(this.keyPanX));
        float num2 = Mathf.Sign(this.keyPanZ) * Mathf.Sqrt(Mathf.Abs(this.keyPanZ));
        if (num != 0f || num2 != 0f)
        {
            float num3 = this.TargetCamera.transform.position.y;
            if (num3 < 3f)
            {
                num3 = 3f;
            }
            float num4 = 1f + 5f * 0.5f;
            num *= num4 * timeDelta * num3;
            num2 *= num4 * timeDelta * num3;
            this.keyPanAmount = this.GetPannedPosition(Vector3.zero, num, num2);
            if (!this.panXActive)
            {
                float num5 = timeDelta * 10f;
                if (this.keyPanX > 0f)
                {
                    this.keyPanX -= num5;
                    UnityToolBase.ClampMin(ref this.keyPanX, 0f);
                }
                else if (this.keyPanX < 0f)
                {
                    this.keyPanX += num5;
                    UnityToolBase.ClampMax(ref this.keyPanX, 0f);
                }
            }
            else
            {
                float num6 = timeDelta * 5f;
                if (this.keyPanX > 0f)
                {
                    this.keyPanX += num6;
                    UnityToolBase.ClampMax(ref this.keyPanX, 1f);
                }
                else if (this.keyPanX < 0f)
                {
                    this.keyPanX -= num6;
                    UnityToolBase.ClampMin(ref this.keyPanX, -1f);
                }
            }
            if (!this.panZActive)
            {
                float num7 = timeDelta * 10f;
                if (this.keyPanZ > 0f)
                {
                    this.keyPanZ -= num7;
                    UnityToolBase.ClampMin(ref this.keyPanZ, 0f);
                }
                else if (this.keyPanZ < 0f)
                {
                    this.keyPanZ += num7;
                    UnityToolBase.ClampMax(ref this.keyPanZ, 0f);
                }
            }
            else
            {
                float num8 = timeDelta * 5f;
                if (this.keyPanZ > 0f)
                {
                    this.keyPanZ += num8;
                    UnityToolBase.ClampMax(ref this.keyPanZ, 1f);
                }
                else if (this.keyPanZ < 0f)
                {
                    this.keyPanZ -= num8;
                    UnityToolBase.ClampMin(ref this.keyPanZ, -1f);
                }
            }
        }
        else
        {
            this.keyPanAmount = Vector3.zero;
        }
    }


    private void DoKeyPan(float deltaTime)
    {
        if (this.keyPanX == 0f && this.keyPanZ == 0f)
        {
            return;
        }
        this.UpdateKeyPanAmount(deltaTime);
        Vector2 dumbActionPoint = this.GetDumbActionPoint();
        //Vector3 vector = this.ScreenToGroundPoint(dumbActionPoint);
        Vector3 targetWorldPos = this.TargetCamera.ScreenToWorldPoint(dumbActionPoint);
        Vector3 vector2 = targetWorldPos + this.keyPanAmount;
        vector2 = this.GetLimitedPosition(vector2);
        Vector3 b = vector2 - targetWorldPos;
        b.y = 0f;
        Vector3 vector3 = this.transform.position + b;
        //float num = HeightMapManager.SeaLevel;
        //if (SMap.Current.OnMap(vector3))
        //{
        //    num = HeightMapManager.GetApparentLimitedHeightAtPoint(vector3.x, vector3.z);
        //}
        //if (vector3.y - num < this.MinCameraHeight)
        //{
        //    vector3.y = num + this.MinCameraHeight;
        //}
        if (vector3.y < this.MinCameraHeight)
        {
            vector3.y = this.MinCameraHeight;
        }
        this.transform.position = vector3;
    }

    private void FollowFocus(float deltaTime)
    {
        Vector3 position = this.transform.position;
        float t = Mathf.Min(deltaTime * this.Damping, 0.75f);
        //Debug.Log("TargetFocusPos:" + this.focusTargetPos);
        Vector3 vector = Vector3.Lerp(position, this.focusTargetPos, t);
        if (UnityToolBase.IsVaildPos(vector))
        {
            return;
        }
        //Debug.LogError("Follow");
        this.transform.position = vector;
        float sqrMagnitude = (vector - this.focusTargetPos).sqrMagnitude;
        bool bArray = sqrMagnitude <= 1f;
        Quaternion rotation = this.transform.rotation;
        Quaternion quaternion = Quaternion.Slerp(rotation, Quaternion.Euler(this.focusTargetRot), t);
        if (UnityToolBase.IsVaildPos(quaternion))
        {
            return;
        }
        this.transform.rotation = quaternion;
        bool bArrayQuanternion = this.TestEulerAnglesInRange(quaternion.eulerAngles, this.focusTargetRot, 0.1f);
        //Debug.Log("Array:" + bArray);
        //D//ebug.Log("ArrayQuanternion:" + bArrayQuanternion);
        //Debug.Log("FollowOver:" + this.followFocusOver);
        if (bArrayQuanternion && bArray && this.followFocusOver)
        {
            //Debug.LogError("FocursOver");
            this.followFocusActive = false;
            this.followFocusOver = true;
        }
    }
    private bool TestEulerAnglesInRange(Vector3 angles1, Vector3 angles2, float delta)
    {
        float f = Mathf.DeltaAngle(angles1.x, angles2.x);
        if (Mathf.Abs(f) > delta)
        {
            return false;
        }
        float f2 = Mathf.DeltaAngle(angles1.y, angles2.y);
        if (Mathf.Abs(f2) > delta)
        {
            return false;
        }
        float f3 = Mathf.DeltaAngle(angles1.z, angles2.z);
        return Mathf.Abs(f3) <= delta;
    }
    private void UpdateKeyZoom(float deltaTime)
    {
        if (this.keyZoomActive != 0)
        {
            this.KeyZoom((float)this.keyZoomActive * deltaTime);
        }
    }

    private void KeyZoom(float zoom)
    {
        Debug.LogError("Zoom:" + zoom);
        //zoom *= 2f * (0.5f + PlayerSettings.Instance.KeyScrollRate);
        zoom *= 2f;
        bool flag = this.followFocusOver;
        if (this.followFocusActive)
        {
            this.focusZoomAmount *= 1f - zoom;
        }
        else
        {
            this.BeginFollowFocus(this.GetDumbActionPoint(), true, this.TiltWithZoom);
            this.focusZoomAmount = 1f - zoom;
        }
    }

    /// <summary>
    /// 开始拖动（不包括旋转）
    /// </summary>
    /// <param name="touch"></param>
    public void BeginDrag(Vector2 curPos)
    {
        var screenTargetPos = this.GetDumbActionPoint();
        this.BeginFollowFocus(screenTargetPos, curPos, true, false);
    }
    /// <summary>
    /// 拖动
    /// </summary>
    /// <param name="touch"></param>
    public void DragMove(Vector2 curPos)
    {
        this.focusCurrentTouchPos = curPos;
    }
    /// <summary>
    /// 结束拖动
    /// </summary>
    public void EndDrag()
    {
        this.followFocusOver = false;
    }

    public bool WheelScrolled(Vector2 mousePos, float zoom)
    {
        bool flag = this.followFocusOver;
        if (zoom > 0.2f)
        {
            zoom = 0.2f;
        }
        else if (zoom < -0.2f)
        {
            zoom = -0.2f;
        }
        zoom *= this.WheelScrollRate;
        float magnitude = (mousePos - this.focusDragStart).magnitude;
        //Debug.LogError("Leng:" + magnitude);
        bool flag2 = magnitude > 10f;
        if (this.followFocusActive && !flag2)
        {
            this.focusZoomAmount *= 1f - zoom;
            //Debug.Log("Zoom1:" + this.focusZoomAmount);
            this.UpdateFocusTarget();
        }
        else
        {
            Vector2 targetPoint = this.GetDumbActionPoint();
            this.BeginFollowFocus(targetPoint, mousePos, false, this.TiltWithZoom);
            this.focusZoomAmount = 1f - zoom;
            //Debug.Log("Zoom:" + this.focusZoomAmount);
        }
        this.followFocusOver = flag;
        return true;
    }
    private float GetTargetTheta(float startTheta, float length)
    {
        float startThetaRate = startTheta * 0.0174532924f;//初始角度60度
        //Debug.Log("StartTheTa:" + num);
        float height = Mathf.Sin(startThetaRate) * length;
        //Debug.Log(height);
        float targetAngleRate = this.TargetAngleForHeightRadians(height);
        //Debug.LogError("TargetAngle:" + targetAngle);
        float num3 = startThetaRate - targetAngleRate;
        float num4 = Mathf.Sign(num3);
        float num5 = 0.05f;
        int num6 = 0;
        while ((double)Mathf.Abs(num3) > 0.01 && num6 < 100)
        {
            if (Mathf.Sign(num3) != num4)
            {
                num5 *= 0.5f;
            }
            if (num3 > 0f)
            {
                startThetaRate -= num5;
            }
            else
            {
                startThetaRate += num5;
            }
            num4 = Mathf.Sign(num3);
            height = Mathf.Sin(startThetaRate) * length;
            num3 = startThetaRate - this.TargetAngleForHeightRadians(height);
            num6++;
        }
        return 57.29578f * startThetaRate;
    }
    private float TargetAngleForHeightRadians(float height)
    {
        float angle;
        var driHeight = this.MaxHeight - this.MaxHeight * 0.333f;
        //Debug.Log(driHeight);
        if (height < 8f)
        {
            angle = 5f;
            this.followFocusOver = true;
        }
        else if (height > driHeight)
        {
            //angle = 70f;
            angle = 70f;
            //说明到点了
            this.followFocusOver = true;
        }
        //else if (height > 2000f)
        //{
        //    num = 70f;
        //}
        else
        {
            this.followFocusOver = false;
            //float realHeight = height - 8f;
            float realHeight = height;
            //float time = num2 / 2000f;
            float time = realHeight / driHeight;
            float curveRate = this.camCurve.Evaluate(time);
            //Debug.LogError("Time:" + time);
            //Debug.LogError("CurveRate:" + curveRate);
            angle = 5f + curveRate * 65f;
        }
        angle += this.cam2ZoomAngle;
        return angle * 0.0174532924f;
    }
    private void UpdateFocusTarget()
    {
        Vector3 cameraZoomDir = this.focusCameraStartPos - this.focusWorldPos;
        float magnitude = cameraZoomDir.magnitude;
        cameraZoomDir.Normalize();
        Quaternion rotation = Quaternion.AngleAxis(this.focusRotY, Vector3.up);
        cameraZoomDir = rotation * cameraZoomDir;
        if (this.focusAdjustXRotWithZoom)
        {
            float height = magnitude * (1f / this.focusZoomAmount);
            //Debug.Log(height);
            int num2 = 60;
            float targetTheta = this.GetTargetTheta((float)num2, height);
            float targetRotationX = targetTheta - this.cam2ZoomAngle;
            bool flag = false;
            if (targetRotationX < this.focusStartRot.x && this.focusZoomAmount > 1f)
            {
                flag = true;
            }
            if (targetRotationX > this.focusStartRot.x && this.focusZoomAmount < 1f)
            {
                flag = true;
            }
            if (flag)
            {
                //Debug.LogError("Over");
                float leapTime = (this.focusZoomAmount >= 1f) ? (this.focusZoomAmount - 1f) : ((1f - this.focusZoomAmount) * 2f);
                UnityToolBase.ClampMax(ref leapTime, 1f);
                if (this.focusRotX == 0f)
                {
                    leapTime = 1f;
                }
                else
                {
                    float num5 = 1f - leapTime;
                    leapTime = 1f - num5 * num5;
                }
                float target = Mathf.Lerp(this.focusStartRot.x, targetRotationX, leapTime);
                this.focusRotX = Mathf.DeltaAngle(this.focusStartRot.x, target);
                float num6 = targetRotationX + this.cam2ZoomAngle;
                Vector3 vector2 = new Vector3(cameraZoomDir.x, 0f, cameraZoomDir.z);
                vector2.Normalize();
                vector2 *= height * Mathf.Cos(num6 * 0.0174532924f);
                vector2.y = height * Mathf.Sin(num6 * 0.0174532924f);
                Vector3 vector3 = vector2;
                vector3.Normalize();
                cameraZoomDir = vector3;
            }
        }
        else
        {
            if (this.RestrictCamera)
            {
                float height = cameraZoomDir.y * magnitude * (1f / this.focusZoomAmount);
                //if (num7 > 3000f)
                //{
                //    num7 = 3000f;
                //}
                if (height > this.MaxHeight)
                {
                    height = this.MaxHeight;
                }
                if (height < this.MinCameraHeight)
                {
                    height = this.MinCameraHeight;
                }
                this.focusRotX = this.GetLimitedXRotAdjustment(height, this.focusRotX, this.focusStartRot.x);
            }
            Vector3 axis = Vector3.Cross(cameraZoomDir, Vector3.up);
            Quaternion rotation2 = Quaternion.AngleAxis(this.focusRotX, axis);
            cameraZoomDir = rotation2 * cameraZoomDir;
        }
        Vector3 vector4 = cameraZoomDir * magnitude;
        vector4 *= 1f / this.focusZoomAmount;
        Vector3 vector5 = this.focusWorldPos;
        Vector3 vector6 = vector5 + vector4;
        float maxHeight = this.MaxHeight;
        if (vector6.y > maxHeight)
        {           
            float distance = maxHeight - this.focusWorldPos.y;
            float d = distance / cameraZoomDir.y;
            vector4 = cameraZoomDir * d;
            vector6 = vector5 + vector4;
        }
        if (this.focusDoDrag)
        {
            Ray limitedScreenPointToRay = this.GetLimitedScreenPointToRay(this.focusCurrentTouchPos);
            Vector3 b;
            if (limitedScreenPointToRay.direction.y < 0f)
            {
                float num11 = limitedScreenPointToRay.origin.y - this.focusWorldPos.y;
                b = limitedScreenPointToRay.origin + limitedScreenPointToRay.direction * -num11 / limitedScreenPointToRay.direction.y;
                this.focusLastValidDragPoint = b;
            }
            else
            {
                b = this.focusLastValidDragPoint;
            }
            Ray limitedScreenPointToRay2 = this.GetLimitedScreenPointToRay(this.focusDragStart);
            if (limitedScreenPointToRay2.direction.y < 0f)
            {
                float num12 = limitedScreenPointToRay2.origin.y - this.focusWorldPos.y;
                Vector3 a = limitedScreenPointToRay2.origin + limitedScreenPointToRay2.direction * -num12 / limitedScreenPointToRay2.direction.y;
                Vector3 b2 = a - b;
                vector5 = this.focusWorldPos + b2;
                vector5 = this.GetLimitedPosition(vector5);
                vector6 = vector5 + vector4;
            }
        }
        if (UnityToolBase.IsVaildPos(vector6))
        {
            return;
        }
        if (this.RestrictCamera)
        {
            //float apparentLimitedHeightAtPoint = HeightMapManager.GetApparentLimitedHeightAtPoint(vector6.x, vector6.z);
            //float minCameraHeight = this.MinCameraHeight;
            //if (vector6.y < apparentLimitedHeightAtPoint + minCameraHeight)
            //{
            //    vector6.y = apparentLimitedHeightAtPoint + minCameraHeight;
            //}
            if (vector6.y < vector5.y + this.MinCameraHeight)
            {
                vector6.y = vector5.y + this.MinCameraHeight;
            }
            this.focusRotX = this.GetLimitedXRotAdjustment(this.focusTargetPos.y, this.focusRotX, this.focusStartRot.x);
        }
        this.focusTargetPos = vector6;
        Vector3 vector7 = this.focusStartRot;
        vector7.y += this.focusRotY;
        vector7.x += this.focusRotX;
        vector7.z = 0f;
        this.focusTargetRot = vector7;
    }


    private Vector3 GetLimitedPosition(Vector3 startPos)
    {
        if (!this.RestrictCamera)
        {
            return startPos;
        }
        int num = 0;
        Vector3 result = startPos;
        if (result.x < this.minX + (float)num)
        {
            result.x = this.minX + (float)num;
        }
        if (result.x > this.maxX - (float)num)
        {
            result.x = this.maxX - (float)num;
        }
        if (result.z < this.minZ + (float)num)
        {
            result.z = this.minZ + (float)num;
        }
        if (result.z > this.maxZ - (float)num)
        {
            result.z = this.maxZ - (float)num;
        }
        return result;
    }



    private float GetLimitedXRotAdjustment(float h, float adjust, float start)
    {
        if (start > 180f)
        {
            start = 360f - start;
        }
        float num = this.CalcMinXRotForHeight(h);
        //Debug.LogError("AngleX:" + num);
        if (start + adjust < num)
        {
            adjust = num - start;
        }
        if (start + adjust > 85f)
        {
            adjust = 85f - start;
        }
        return adjust;
    }

    private float CalcMinXRotForHeight(float height)
    {
        //height -= HeightMapManager.SeaLevel;
        height -= 0;
        float result;
        if (height < 150f + this.MinCameraHeight)
        {
            float num = (height - this.MinCameraHeight) / (150f + this.MinCameraHeight);
            result = -10f + num * 45f;
        }
        else
        {
            //float num2 = (height - 150f) / 3150f;
            float num2 = (height - 150f) / (this.MaxHeight + 150f);
            result = 35f + num2 * 45f;
        }
        return result;
    }
    private void BeginFollowFocus(Vector2 targetPoint, bool doDrag, bool adjustXRotWithZoom)
    {
        this.BeginFollowFocus(targetPoint, targetPoint, doDrag, adjustXRotWithZoom);
    }
    private void BeginFollowFocus(Vector2 screenTargetPoint, Vector2 mouseOrTouchPos, bool doDrag, bool adjustXRotWithZoom)
    {
        //Vector3 targetWorldPos = this.ScreenToGroundPoint(screenTargetPoint);
        //这里直接屏幕坐标转世界坐标
        Vector3 targetWorldPos = this.TargetCamera.ScreenToWorldPoint(screenTargetPoint);
        targetWorldPos.y = 20f;
        this.BeginFollowFocus(targetWorldPos, mouseOrTouchPos, doDrag, adjustXRotWithZoom);
    }
    private void BeginFollowFocus(Vector3 targetWorldPos, Vector2 mouseOrTouchPos, bool doDrag, bool adjustXRotWithZoom)
    {
        if (targetWorldPos.y < 0f)
        {
            return;
        }
        this.focusWorldPos = targetWorldPos;
        this.focusLastValidDragPoint = this.focusWorldPos;
        this.focusZoomAmount = 1f;
        this.focusRotY = (this.focusRotX = 0f);
        this.focusCameraStartPos = this.transform.position;
        this.focusStartRot = this.transform.rotation.eulerAngles;
        if (this.focusStartRot.x > 180f)
        {
            this.focusStartRot.x = 360f - this.focusStartRot.x;
        }
        this.focusTargetPos = this.focusCameraStartPos;
        this.focusTargetRot = this.focusStartRot;
        this.focusDragStart = mouseOrTouchPos;
        this.focusCurrentTouchPos = mouseOrTouchPos;
        this.followFocusActive = true;
        this.followFocusOver = false;
        this.focusDoDrag = doDrag;
        this.focusAdjustXRotWithZoom = adjustXRotWithZoom;
        this.SetCameraToZoomAngle();
    }

    private void SetCameraToZoomAngle()
    {
        float num = this.TargetCamera.nearClipPlane * Mathf.Tan(this.TargetCamera.fieldOfView * 0.5f * 0.0174532924f);
        if (this.PivotPointUpScreen <= 0.5f)
        {
            float num2 = (1f - 2f * this.PivotPointUpScreen) * num;//0.34 * 
            this.cam2ZoomAngle = Mathf.Atan(num2 / this.TargetCamera.nearClipPlane) * 57.29578f;
        }
        else
        {
            //float num2 = (1f - 2f * this.PivotPointUpScreen) * num;//0.34 * 
            float num2 = (2f * this.PivotPointUpScreen - 1f) * num;
            this.cam2ZoomAngle = Mathf.Atan(num2 / this.TargetCamera.nearClipPlane) * 57.29578f;
        }
        //Debug.LogError("Cam2ZoomAngle:" + this.cam2ZoomAngle);
    }

    private Ray GetLimitedScreenPointToRay(Vector2 p)
    {
        Ray result = this.TargetCamera.ScreenPointToRay(p);//中点偏移y值的屏幕坐标，或者是移动的鼠标屏幕位置
        //Ray ray = this.TargetCamera.ScreenPointToRay(new Vector2(p.x, (float)(Screen.height / 2)));//屏幕中点坐标
        //// 貌似拉到做上面会都出现1

        //if (result.direction.y > ray.direction.y)
        //{
        //    result.direction = (result.direction + ray.direction) * 0.5f;
        //}
        Vector3 direction = result.direction;
        direction.Normalize();
        if (direction.y > -0.13f)
        {
            //Debug.LogError("2");
            Vector3 direction2 = result.direction;
            direction2.y = -0.13f;
            direction2.Normalize();
            result = new Ray(result.origin, direction2);
        }
        return result;
    }
    //private Vector3 ScreenToGroundPoint(Vector2 p)
    //{
    //    Ray limitedScreenPointToRay = this.GetLimitedScreenPointToRay(p);
    //    Vector3 rayIntersection = this.GetRayIntersection(limitedScreenPointToRay);
    //    if (rayIntersection.y > 0f)
    //    {
    //        //float apparentLimitedHeightAtPoint = HeightMapManager.GetApparentLimitedHeightAtPoint(rayIntersection.x, rayIntersection.z);
    //        float apparentLimitedHeightAtPoint = 20f;//接近地面的高度为20f
    //        rayIntersection.y = apparentLimitedHeightAtPoint;
    //    }
    //    return rayIntersection;
    //}


    private Vector2 GetDumbActionPoint()
    {
        int num = Screen.width / 2;
        float y = (float)Screen.height * this.PivotPointUpScreen;
        Vector2 result = new Vector2((float)num, y);
        return result;
    }
}

