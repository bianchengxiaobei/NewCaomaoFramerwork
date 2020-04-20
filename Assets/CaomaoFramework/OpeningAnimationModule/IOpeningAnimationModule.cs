using UnityEngine;
using System.Collections;
using System;
public interface IOpeningAnimationModule
{
    void Awake(Transform root);
    void StartPlay();
    void SetOnFinishedCallback(Action onFinished);
}
