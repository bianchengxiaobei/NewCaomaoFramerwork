using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Yoyo.UI;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
namespace CaomaoFramework 
{
    [CInstanceNumber(10)]
    public class ScrollData : IClassInstance
    {
        public string Info;
        public int Loop = 1;

        public void OnAlloc()
        {
            //this.Loop = 1;
        }

        public void OnRelease()
        {
            
        }
    }
    public class CUIScrollBroadcast : CUIBehaviour
    {
        public Text lb_content;
        public DOTweenAnimation m_anim;
        private Queue<ScrollData> m_queueScrollDatas = new Queue<ScrollData>();
        private bool m_bIsPlaying = false;
        private float m_labelWidth = 0;
        private float m_curScrollTime = 0;
        private ScrollData m_curPlayingScroll;
        private float m_curScrollBgWidth = 0;
        private float preSecondWidth = 0;
        private float defalutTime = 5f;//默认为5秒。从右到左
        private TweenerCore<Vector3, Vector3, VectorOptions> tweener;
        public int MaxCount { get; set; } = 50;
        public float InfiniteScrollTime { get; set; } = 20f;
        private void Awake()
        {
            if (this.m_anim == null) 
            {
                this.m_anim = this.GetComponentInChildren<DOTweenAnimation>();
                this.m_anim.onComplete.AddListener(this.OnRollEndCallback);           
                this.Show(false);
            }
            if (this.lb_content == null) 
            {
                this.lb_content = this.GetComponentInChildren<Text>();
            }
            var rect = this.GetComponent<RectTransform>();
            this.m_curScrollBgWidth = rect.sizeDelta.x;
            this.preSecondWidth = this.m_curScrollBgWidth / this.defalutTime;
            ClassPoolModule<ScrollData>.Init();
        }
        private void OnEnable()
        {
            this.tweener = this.m_anim.tween as TweenerCore<Vector3, Vector3, VectorOptions>;
        }

        private void OnRollEndCallback()
        {
            this.m_bIsPlaying = false;
            if (this.m_curPlayingScroll != null) 
            {
                this.m_curPlayingScroll.Loop--;
                if (this.m_curPlayingScroll.Loop == 0)
                {
                    ClassPoolModule<ScrollData>.Release(this.m_curPlayingScroll);
                    if (this.m_queueScrollDatas.Count > 0)
                    {
                        var scrollData = this.m_queueScrollDatas.Dequeue();
                        this.AddScrollBroadcast(scrollData);
                        this.StartRoll();
                    }
                    else
                    {
                        this.Show(false);
                        this.m_curPlayingScroll = null;
                    }
                }
                else
                {
                    //说明无限滚动或者次数没玩
                    this.StartRoll();
                }
            }
           
        }
        private void StartRoll()
        {
            this.Show(true);
            if (this.m_anim != null)
            {
                if (!this.m_bIsPlaying)
                {
                    this.m_bIsPlaying = true;
                    this.tweener.ChangeEndValue(new Vector3(-290f - this.m_labelWidth, 0, 0),this.m_curScrollTime, false);
                    this.m_anim.DORestart();
                }
            }
        }
        public void AddScrollBroadcast(string info, int loop = 1) 
        {
            var data = ClassPoolModule<ScrollData>.Alloc();
            data.Info = info;
            data.Loop = loop;
            this.AddScrollBroadcast(data);
            this.StartRoll();
        }

        private void AddScrollBroadcast(ScrollData data)
        {
            //如果正在滚动,先缓存起来，等滚动完再接着滚动
            if (this.m_queueScrollDatas.Count >= MaxCount)
            {
                //太多就不再添加
                return;
            }
            if (this.m_bIsPlaying)
            {
                this.m_queueScrollDatas.Enqueue(data);
                return;
            }
            this.m_curPlayingScroll = data;
            this.lb_content.text = data.Info;
            //计算时间
            //设置滚动时间
            //判断是否为无限滚动，如果是就设置滚动时间间隔长点
            this.m_labelWidth = this.lb_content.preferredWidth;
            if (this.m_curPlayingScroll.Loop < 0)
            {
                this.m_curScrollTime = this.InfiniteScrollTime;
            }
            else
            {              
                var width = this.m_labelWidth + this.m_curScrollBgWidth;//580 5s = 116 1s
                this.m_curScrollTime = width / this.preSecondWidth;
            }          
            //Debug.Log(this.m_curScrollTime);
        }
    }
}
