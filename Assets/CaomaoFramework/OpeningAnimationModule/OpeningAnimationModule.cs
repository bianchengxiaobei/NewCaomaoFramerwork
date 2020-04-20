using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
namespace CaomaoFramework 
{
    public enum EOpeningAnimationType 
    {
        Video,//直接视频播放开场
        SpriteAnimation//通过sprite来切换
    }
    /// <summary>
    /// 开场动画
    /// </summary>
    public class OpeningAnimationModule : MonoBehaviour, IOpeningAnimationModule
    {
        private IOpeningAnimationModule m_openingAnimationImp;
        public EOpeningAnimationType m_animationType = EOpeningAnimationType.SpriteAnimation;
        private bool m_bJump = false;//是否跳过
        private void Awake()
        {
            this.m_bJump = PlayerPrefModule.GetBool("OpenAnimationJump");
            if (this.m_bJump == false)
            {
                this.Awake(this.transform);
            }
            else 
            {
                this.OnFinished();
            }
        }
        private void Start()
        {
            if (this.m_bJump) 
            {
                return;
            }
            this.StartPlay();//这里只是演示
            this.SetOnFinishedCallback(this.OnFinished);
        }
        private void OnFinished() 
        {
            SceneManager.LoadSceneAsync("Driver", LoadSceneMode.Single);
        }
        public void Awake(Transform root)
        {
            if (this.m_openingAnimationImp == null)
            {
                if (this.m_animationType == EOpeningAnimationType.SpriteAnimation)
                {
                    this.m_openingAnimationImp = new SpriteOpeningAnimationImp();
                }
                else
                {
                    this.m_openingAnimationImp = new VideoOpeningAnimationImp();
                }

            }
            this.m_openingAnimationImp.Awake(root);
        }
   

        public void SetOnFinishedCallback(Action onFinished)
        {
            this.m_openingAnimationImp.SetOnFinishedCallback(onFinished);
        }

        public void StartPlay()
        {
            this.m_openingAnimationImp.StartPlay();
        }
    }
}

