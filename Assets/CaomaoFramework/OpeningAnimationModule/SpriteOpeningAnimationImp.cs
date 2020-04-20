using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
namespace CaomaoFramework 
{  
    public class SpriteOpeningAnimationImp : IOpeningAnimationModule
    {
        private Action m_actionOnFinished;
        private SpriteOpeningAnimConfig SequenceConfig;
        private CanvasGroup m_canvasGroup;//包含logo和白底背景
        private Sequence dotweenSeq;
        private TweenerCore<float, float, FloatOptions> m_fadeInAnim;
        private TweenerCore<float, float, FloatOptions> m_fadeOutAnim;
        private int m_curSeqIndex = 0;
        private Transform m_root;
        public void Awake(Transform root)
        {
            this.m_root = root;
            SequenceConfig = SpriteOpeningAnimConfig.Instance;
            if (SequenceConfig == null || SequenceConfig.SpriteSequences.Count == 0)
            {
                Debug.LogError("SequenceConfig == null");
                return;
            }
            dotweenSeq = DOTween.Sequence();
            this.m_canvasGroup = root.GetComponent<CanvasGroup>();
            this.SeqInit();
        }

        public void SetOnFinishedCallback(Action onFinished)
        {
            this.m_actionOnFinished = onFinished;
        }

        public void StartPlay()
        {
            if (this.m_fadeInAnim == null)
            {
                this.m_fadeInAnim = this.m_canvasGroup.DOFade(1, SequenceConfig.FadeInTime);//从黑到白（淡入）
                this.m_fadeInAnim.SetAutoKill(false);
                this.m_fadeInAnim.SetEase(Ease.Linear);
                //this.m_fadeInAnim.OnComplete(this.OnFadeInFinished);
                this.m_fadeInAnim.Pause();
                this.dotweenSeq.Append(this.m_fadeInAnim);
                this.dotweenSeq.AppendInterval(SequenceConfig.Duration);
            }
            
            if (this.m_fadeOutAnim == null)
            {
                this.m_fadeOutAnim = this.m_canvasGroup.DOFade(0, SequenceConfig.FadeOutTime);//从白到黑（淡出）
                this.m_fadeOutAnim.SetAutoKill(false);
                this.m_fadeOutAnim.SetEase(Ease.Linear);
                this.m_fadeOutAnim.OnComplete(this.OnFadeOutFinished);
                this.m_fadeOutAnim.Pause();
                this.dotweenSeq.Append(this.m_fadeOutAnim);
            }
            this.dotweenSeq.Restart();
        }
        //private void OnFadeInFinished()
        //{

        //}
        private void OnFadeOutFinished()
        {
            //Debug.Log("OpeningAniamtionFinished");
            //结束之后换logo            
            if (this.ChangeIndex())
            {
                this.StartPlay();
            }
            else 
            {
                this.Clear();
            }
        }
        private void Clear() 
        {
            //Debug.Log("Clear");
            this.m_fadeInAnim.Kill();
            this.m_fadeOutAnim.Kill();
            this.dotweenSeq.Kill();
        }
        private bool ChangeIndex() 
        {
            this.m_curSeqIndex++;
            if (this.m_curSeqIndex == this.SequenceConfig.SpriteSequences.Count) 
            {
                this.m_actionOnFinished?.Invoke();
                return false;
            }
            foreach (var data in SequenceConfig.SpriteSequences)
            {
                data.SetVisiable(this.m_curSeqIndex);
            }
            return true;
        }
        private void SeqInit() 
        {
            foreach (var data in SequenceConfig.SpriteSequences) 
            {
                if (string.IsNullOrEmpty(data.ImageGName)) 
                {
                    Debug.LogError("SSName == null");
                    continue;
                }
                var go = this.m_root.Find(data.ImageGName).gameObject;
                if (!go) 
                {
                    Debug.LogError("No Go:" + data.ImageGName);
                    continue;
                }
                data.ImageLogo = go;
                data.SetVisiable(this.m_curSeqIndex);
            }
        }
    }
}

