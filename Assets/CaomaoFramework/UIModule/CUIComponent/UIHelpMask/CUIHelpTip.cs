using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

namespace  CaomaoFramework
{
    /// <summary>
    /// 新手引导人物提示的界面
    /// </summary>
    public class CUIHelpTip : MonoBehaviour
    {
        private Text lb_tip;
        private Image sp_character;
        private Text lb_characterName;
        private Button bt_tip;
        private Vector3 m_orginPos;
        public void Awake()
        {
            this.bt_tip = this.transform.Find("bt_tip").GetComponent<Button>();
            this.lb_tip = this.bt_tip.transform.Find("lb_tip").GetComponent<Text>();
            this.lb_characterName = this.bt_tip.transform.Find("lb_characterName").GetComponent<Text>();
            this.sp_character = this.transform.Find("sp_character").GetComponent<Image>();
            this.m_orginPos = this.transform.position;
        }
        /// <summary>
        /// 添加事件
        /// </summary>
        /// <param name="buttonEvent"></param>
        public void AddButtonListener(UnityAction buttonEvent)
        {
            if (buttonEvent == null)
            {
                Debug.LogError("ButtonTipEvent == null");
                this.bt_tip.onClick.RemoveAllListeners();//移除所有的button事件
                return;
            }
            if (this.bt_tip.onClick.GetPersistentEventCount() == 0)
            {
                this.bt_tip.onClick.AddListener(buttonEvent);
            }
        }

        public void SetNewbieHelpStepData(NewbieHelpTipStepData data,int index)
        {
            if (index >= data.Dialogs.Count)
            {
                Debug.LogError("Index >= Dialog.Count");
                return;
            }

            var tipData = data.Dialogs[index];
            if (tipData != null)
            {
                this.lb_tip.text = tipData.DialogContent;
                this.lb_characterName.text = tipData.CharacterName;
                CaomaoDriver.ResourceModule.LoadAssetAsync(tipData.CharacterPath, (asset) =>
                {
                    this.sp_character.sprite = asset  as Sprite;//替换人物的图片
                });
            }
        }

        public void SetVisiable(bool bVisiable)
        {
            if (bVisiable)
            {
                this.transform.position = this.m_orginPos;
                CaomaoDriver.NewbieHelpModule.SetVaildArea(this.bt_tip.image.rectTransform);//添加可以点击区域
            }
            else
            {
                this.transform.position = Vector3.one * 1000;
            }
        }
    }
}

