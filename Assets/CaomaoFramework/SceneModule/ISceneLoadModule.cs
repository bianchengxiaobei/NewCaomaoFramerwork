using System;
public interface ISceneLoadModule
{
    float ProgressValue
    {
        get;
    }

    void EnterScene(string sceneName);
    void RegisterLoadSceneFinishCallback(Action actionCallback);
    void RegisterScenePerparedCallback(Action actionCallback);
}
