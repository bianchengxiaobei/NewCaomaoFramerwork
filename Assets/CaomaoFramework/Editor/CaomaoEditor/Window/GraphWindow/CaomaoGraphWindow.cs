using System;
using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using Sirenix.Utilities;
using ColorExtensions = ColorExtenstion;
public class CaomaoGraphWindow : OdinEditorWindow
{
    private static CaomaoGraphWindow Window;
    private const float MinZoom = 0.25f;
    private const float MaxZoom = 1f;
    private Rect canvasRect;
    private GUIStyle canvaBgStyle;
    private GUIContent waitContent;
    private Vector2 m_prePan;
    private Vector2? m_targetPan;
    private Vector2 m_panSpeed;
    private float? m_targetZoom;
    private float m_preZoom;
    private float m_zoomSpeed;
    //private float preTime = 0;
    //private float deltaTime;
    private bool m_bCanEvent = false;
    private bool m_bRepaint = false;
    [MenuItem("CaomaoTools/CaomaoGraphWindow")]
    public static void OpenWindow()
    {
        Window = GetWindow<CaomaoGraphWindow>();
        Window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
    }


    protected override void Initialize()
    {
        base.Initialize();
        canvaBgStyle = CaomaoGUIStyle.GraphBGStyle;
        this.m_preZoom = 1;
        ColorExtensions.PushIntColor(45, 45, 45);
        waitContent = new GUIContent("正在编译中...");
    }

    public virtual void BuildGraph()
    {

    }



    protected override void OnGUI()
    {
        this.canvasRect = new Rect(0, 0, this.position.width, this.position.height);
        this.canvasRect = canvasRect.Padding(5f);
        //this.canvaBgStyle.Draw(this.canvasRect, GUIContent.none, 0);
        //GUI.Box(this.canvasRect, GUIContent.none, this.canvaBgStyle);
        EditorGUI.DrawRect(this.canvasRect, ColorExtenstion.PopIntColor());
        if (EditorApplication.isCompiling)
        {
            ShowNotification(this.waitContent);
            return;
        }
        this.DrawGrid();
        this.HandlerGUIEvent();
        base.OnGUI();
    }

    public void Update()
    {
        this.UpdateMovePan();
        this.UpdateZoom();
        if (this.m_bRepaint)
        {
            this.Repaint();
        }
    }

    private void UpdateMovePan()
    {
        if (this.m_targetPan == null)
        {
            this.m_bRepaint |= false;
            return;
        }
        var tempTarget = (Vector2)this.m_targetPan;
        if ((tempTarget - this.m_prePan).magnitude < 0.1f)
        {
            this.m_targetPan = null;
            this.m_bRepaint |= false;
            return;
        }
        this.m_prePan = Vector2.SmoothDamp(this.m_prePan, tempTarget, ref this.m_panSpeed, 0.08f);
        this.m_bRepaint |= true;
    }

    private void UpdateZoom()
    {
        if (this.m_targetZoom == null)
        {
            this.m_bRepaint |= false;
            return;
        }
        var tempTarget = this.m_targetZoom.Value;
        if (Mathf.Abs(tempTarget - this.m_preZoom) < 0.01f)
        {
            this.m_bRepaint |= false;
            return;
        }
        this.m_preZoom = Mathf.SmoothDamp(this.m_preZoom, tempTarget, ref this.m_zoomSpeed, 0.08f);
        this.m_bRepaint |= true;
    }

    private void DrawGrid()
    {
        if (Event.current.type != EventType.Repaint)
        {
            return;
        }
        Handles.color = Color.black.WithAlpha(0.15f);

        var drawGridSize = this.m_preZoom > 0.5f ? 15 : 15 * 5;
        var step = drawGridSize * m_preZoom;

        var xDiff = this.m_prePan.x % step;
        var xStart = this.canvasRect.xMin + xDiff;
        var xEnd = this.canvasRect.xMax;
        for (var i = xStart; i < xEnd; i += step)
        {
            if (i > this.canvasRect.xMin)
            {
                Handles.DrawLine(new Vector3(i, this.canvasRect.yMin, 0), new Vector3(i, this.canvasRect.yMax, 0));
            }
        }

        var yDiff = this.m_prePan.y % step;
        var yStart = this.canvasRect.yMin + yDiff;
        var yEnd = this.canvasRect.yMax;
        for (var i = yStart; i < yEnd; i += step)
        {
            if (i > this.canvasRect.yMin)
            {
                Handles.DrawLine(new Vector3(this.canvasRect.xMin, i, 0), new Vector3(this.canvasRect.xMax, i, 0));
            }
        }
        Handles.color = Color.white;
    }

    private void DrawGraphNode()
    {

    }


    private void HandlerGUIEvent()
    {
        this.m_bCanEvent = this.canvasRect.Contains(Event.current.mousePosition);
        this.ChangePanEvent();
        this.ChangeZoomEvent();
    }
    private void ChangePanEvent()
    {
        var e = Event.current;
        if (e.button == (int)EGUIEventButtonType.Left && e.type == EventType.MouseDrag && this.m_bCanEvent)
        {
            this.m_prePan += e.delta;
            this.m_targetPan = null;
            this.m_targetZoom = null;
            EditorGUIUtility.AddCursorRect(this.canvasRect, MouseCursor.Pan);
            e.Use();//其他UI元素忽略此事件
        }
    }
    private void ChangeZoomEvent()
    {
        var e = Event.current;
        if (e.type == EventType.ScrollWheel && this.m_bCanEvent)
        {
            EditorGUIUtility.AddCursorRect(this.canvasRect, MouseCursor.Zoom);
            var delteZoom = e.delta.y > 0 ? 0.25f : -0.25f;
            this.Zoom(e.mousePosition, delteZoom);
            e.Use();
        }
    }
    private void Zoom(Vector2 center,float delteZoom)
    {
        if (this.m_preZoom == 1 && delteZoom >= 0)
        {
            return;
        }
        //首先是缩放，需要缩放，移动计算移动到中点的距离
        var tempDis = (center - this.m_prePan) / this.m_preZoom;
        var addZoom = this.m_preZoom + delteZoom;
        this.m_targetZoom = Mathf.Clamp(addZoom, MinZoom, MaxZoom);

        var realCenter = tempDis * this.m_targetZoom + this.m_prePan;
        this.m_targetPan = this.m_prePan + center - realCenter;
    }
}

