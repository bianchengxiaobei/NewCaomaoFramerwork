using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Canvas))]
public class CanvasHelper : MonoBehaviour
{
    public UnityEvent onOrientationChange = new UnityEvent();
    public UnityEvent onResolutionChange = new UnityEvent();
    public bool isLandscape { get; private set; }
    private bool screenChangeVarsInitialized = false;
    private ScreenOrientation lastOrientation = ScreenOrientation.Portrait;
    private Vector2 lastResolution = Vector2.zero;
    private Rect lastSafeArea = Rect.zero;

    private Canvas canvas;
    private RectTransform safeAreaTransform;

    void Awake()
    {
        canvas = GetComponent<Canvas>();
        safeAreaTransform = transform.Find("SafeArea") as RectTransform;

        if (!screenChangeVarsInitialized)
        {
            lastOrientation = Screen.orientation;
            lastResolution.x = Screen.width;
            lastResolution.y = Screen.height;
            lastSafeArea = Screen.safeArea;
            screenChangeVarsInitialized = true;
        }
    }

    void Start()
    {
        ApplySafeArea();
    }

    void Update()
    {
        if (Application.isMobilePlatform)
        {
            if (Screen.orientation != lastOrientation)
                OrientationChanged();

            if (Screen.safeArea != lastSafeArea)
                SafeAreaChanged();
        }
        else
        {
            if (Screen.width != lastResolution.x || Screen.height != lastResolution.y)
                ResolutionChanged();
        }
    }

    public void ApplySafeArea()
    {
        if (safeAreaTransform == null)
            return;

        var safeArea = Screen.safeArea;

        var anchorMin = safeArea.position;
        var anchorMax = safeArea.position + safeArea.size;
        anchorMin.x /= canvas.pixelRect.width;
        anchorMin.y /= canvas.pixelRect.height;
        anchorMax.x /= canvas.pixelRect.width;
        anchorMax.y /= canvas.pixelRect.height;

        safeAreaTransform.anchorMin = anchorMin;
        safeAreaTransform.anchorMax = anchorMax;

        Debug.Log(
        "ApplySafeArea:" +
        "Screen.orientation: " + Screen.orientation +
#if UNITY_IOS
         "\n Device.generation: " + UnityEngine.iOS.Device.generation.ToString() +
#endif
         "Screen.safeArea.position: " + Screen.safeArea.position.ToString() +
        "Screen.safeArea.size: " + Screen.safeArea.size.ToString() +
        "Screen.width / height: (" + Screen.width.ToString() + ", " + Screen.height.ToString() + ")" +
        "canvas.pixelRect.size: " + canvas.pixelRect.size.ToString() +
        "anchorMin: " + anchorMin.ToString() +
        "anchorMax: " + anchorMax.ToString());
    }

    private void OrientationChanged()
    {
        //Debug.Log("Orientation changed from " + lastOrientation + " to " + Screen.orientation + " at " + Time.time);

        lastOrientation = Screen.orientation;
        lastResolution.x = Screen.width;
        lastResolution.y = Screen.height;

        isLandscape = lastOrientation == ScreenOrientation.LandscapeLeft || lastOrientation == ScreenOrientation.LandscapeRight || lastOrientation == ScreenOrientation.Landscape;
        onOrientationChange.Invoke();

    }

    private void ResolutionChanged()
    {
        if (lastResolution.x == Screen.width && lastResolution.y == Screen.height)
            return;

        //Debug.Log("Resolution changed from " + lastResolution + " to (" + Screen.width + ", " + Screen.height + ") at " + Time.time);

        lastResolution.x = Screen.width;
        lastResolution.y = Screen.height;

        isLandscape = Screen.width > Screen.height;
        onResolutionChange.Invoke();
    }

    private void SafeAreaChanged()
    {
        if (lastSafeArea == Screen.safeArea)
            return;

        //Debug.Log("Safe Area changed from " + lastSafeArea + " to " + Screen.safeArea.size + " at " + Time.time);

        lastSafeArea = Screen.safeArea;

        this.ApplySafeArea();
    }

    public Vector2 GetSafeAreaSize()
    {
        if (this.safeAreaTransform != null) 
        {
            return this.safeAreaTransform.sizeDelta;
        }
        return Vector2.zero;
    }
}