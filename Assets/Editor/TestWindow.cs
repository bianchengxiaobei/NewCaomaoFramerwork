using UnityEditor;
using UnityEngine;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using Sirenix.Utilities;
using Sirenix.OdinInspector.Editor;
public class TestWindow : OdinEditorWindow
{
    public static TestWindow Window;
    private float menuWidth = 180f;
    private CaomaoToolbarDropdown d;
    [MenuItem("CaomaoTools/TestWindow")]
    public static void ShowWindow()
    {
        Window = GetWindow<TestWindow>();
        Window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
    }
    protected override void Initialize()
    {
        base.Initialize();
        d = new CaomaoToolbarDropdown();
        var t = new CaomaoToolbarMenuItem("few");
        t.AddMenuItem("e22", () =>
        {
            Debug.Log("1111");
        });
        t.AddMenuItem("3434", () =>
        {
            Debug.Log("3434");
        });
        d.AddToolbarMenuItems("1",10,t);
        d.AddToolbarMenuItems("1", 4, t);
        var t1 = new CaomaoToolbarMenuItem("f32432");
        t1.AddMenuItem("2342", () =>
        {
            Debug.Log("1111");
        });
        t1.AddMenuItem("3434", () =>
        {
            Debug.Log("3434");
        });
        d.AddToolbarMenuItems("2", 4, t);
        d.AddToolbarMenuItems("2", 4, t1);
    }
    protected override void OnGUI()
    {
        d.BeginDrawGroup();
        d.DrawSingleGroup("1");
        GUILayout.FlexibleSpace();
        d.DrawSingleGroup("2");
        d.EndDrawGruop();
    }
}
