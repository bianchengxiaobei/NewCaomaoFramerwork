using UnityEngine;
namespace CaomaoFramework
{
    public enum EUIHideType
    {
        Active,
        MoveOutCamera
    }
    public abstract class UIBase
    {
        protected Transform m_oRoot;//UI根目录
        protected string m_sResName;         //资源名
        protected bool m_bNotDestroy;          //是否常驻
        protected bool m_bVisible = false;   //是否可见
        protected RectTransform m_rectTransform;
        protected EUIHideType m_eHideType = EUIHideType.Active;//隐藏UI的类型
        /// <summary>
        /// UI面板根节点
        /// </summary>
        public Transform PanelRoot
        {
            get
            {
                return this.m_oRoot;
            }
        }
        /// <summary>
        /// 是否已经打开
        /// </summary>
        public bool Visiable
        {
            get
            {
                return this.m_bVisible;
            }
        }
        /// <summary>
        /// 是否永久驻留在内存中
        /// </summary>
        public bool NotDestroy
        {
            get
            {
                return this.m_bNotDestroy;
            }
        }
        //类对象初始化
        public abstract void Init();

        //窗口控制初始化
        protected abstract void InitGraphicComponet();
        /// <summary>
        /// 将UI放到前面层级
        /// </summary>
        protected virtual void GoToForwardLayer()
        {
            this.m_oRoot.SetAsFirstSibling();
        }

        //窗口控件释放
        protected abstract void RealseGraphicComponet();

        //游戏事件注册
        protected abstract void OnAddListener();

        //游戏事件注消
        protected abstract void OnRemoveListener();

        //显示初始化
        public abstract void OnEnable();

        //隐藏处理
        public abstract void OnDisable();

        //每帧更新
        public virtual void Update()
        {

        }

        //显示
        public void Show()
        {
            if (m_oRoot == null)
            {
                CreateUI();
            }
            else if (m_oRoot && m_oRoot.gameObject.activeSelf == false)
            {
                this.GoToForwardLayer();
                m_oRoot.gameObject.SetActive(true);
                m_bVisible = true;
                OnEnable();
                OnAddListener();
            }
        }

        //隐藏
        public void Hide()
        {
            if (m_oRoot && m_oRoot.gameObject.activeSelf)
            {
                OnRemoveListener();
                OnDisable();
                ///如果是永久在内存中，就直接隐藏
                if (m_bNotDestroy)
                {
                    this.m_oRoot.gameObject.SetActive(false);
                }
                else
                {
                    RealseGraphicComponet();
                    Destroy();
                }
            }
            this.m_bVisible = false;
        }

        //预加载
        public void PreLoad()
        {
            if (m_oRoot == null)
            {
                PreLoadUI();
            }
        }

        //延时删除
        public void DelayDestory()
        {
            if (m_oRoot)
            {
                RealseGraphicComponet();
                Destroy();
            }
        }

        //创建窗体
        protected virtual void CreateUI()
        {
            if (m_oRoot)
            {
                Debug.LogError("Window Create Error Exist!");
            }

            if (m_sResName == null || m_sResName == "")
            {
                Debug.LogError("Window Create Error ResName is empty!");
            }
            CaomaoDriver.ResourceModule.LoadGameObjectAsync(this.m_sResName,(UIObj)=>
            {
                if (UIObj != null)
                {
                    this.m_oRoot = UIObj.transform;
                    this.m_rectTransform = this.m_oRoot.GetComponent<RectTransform>();
                    this.m_oRoot.SetParent(CaomaoDriver.UIRoot);
                    this.m_rectTransform.sizeDelta = Vector2.zero;
                    this.m_oRoot.localPosition = Vector3.zero;
                    this.m_oRoot.localRotation = Quaternion.identity;
                    this.m_oRoot.localScale = Vector3.one;
                    this.m_oRoot.gameObject.SetActive(false);//设置为隐藏
                    InitGraphicComponet();
                    if (NotDestroy == false)
                    {
                        this.GoToForwardLayer();
                        this.m_oRoot.gameObject.SetActive(true);
                        this.m_bVisible = true;
                        this.OnEnable();
                        this.OnAddListener();
                    }
                }
                else
                {
                    Debug.LogError($"加载UI Prefab失败:{m_sResName}");
                }
            });
        }
        protected virtual void PreLoadUI() 
        {
            if (m_oRoot)
            {
                Debug.LogError("Window Create Error Exist!");
            }

            if (m_sResName == null || m_sResName == "")
            {
                Debug.LogError("Window Create Error ResName is empty!");
            }
            CaomaoDriver.ResourceModule.AddGameObjectTask(this.m_sResName, (UIObj) =>
            {
                if (UIObj != null)
                {
                    this.m_oRoot = UIObj.transform;
                    this.m_rectTransform = this.m_oRoot.GetComponent<RectTransform>();
                    this.m_oRoot.SetParent(CaomaoDriver.UIRoot);
                    this.m_rectTransform.sizeDelta = Vector2.zero;
                    this.m_oRoot.localPosition = Vector3.zero;
                    this.m_oRoot.localRotation = Quaternion.identity;
                    this.m_oRoot.localScale = Vector3.one;
                    this.m_oRoot.gameObject.SetActive(false);//设置为隐藏
                    InitGraphicComponet();
                }
                else
                {
                    Debug.LogError($"加载UI Prefab失败:{m_sResName}");
                }
            });
        }
        //销毁窗体
        protected void Destroy()
        {
            if (this.m_oRoot)
            {
                CaomaoDriver.ResourceModule.UnloadResource(this.m_oRoot.gameObject);
                this.m_oRoot = null;
            }
        }
    }
}
