using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace CaomaoFramework
{
    public class CUIHighSpriteTabToggle : CUITabToggle
{
        public Sprite sp_normalSprite;
        public Sprite sp_highSprite;
        public Text lb_content;
        public Color textNormalColor = Color.white;
        public Color textHighColor = Color.white;

        protected override void Awake()
        {
            base.Awake();
            if (Application.isPlaying)
            {
                this.ChangeSpriteState();
            }           
        }


        public override void ChangeSpriteState()
        {
            if (this.isOn)
            {
                this.SetNoSprite(this.sp_highSprite);
                if (this.lb_content != null)
                {
                    this.lb_content.color = this.textHighColor;
                }
            }
            else
            {
                this.SetNoSprite(this.sp_normalSprite);
                if (this.lb_content != null)
                {
                    this.lb_content.color = this.textNormalColor;
                }
            }
        }
        private void SetNoSprite(Sprite sprite)
        {
            if (sprite == null)
            {
                //说明无图片，那么颜色的话alpha就设置为0
                var color = this.image.color;
                color.a = 0;
                this.image.color = color;
            }
            else
            {
                this.image.sprite = sprite;
                if (this.image.color.a == 0)
                {
                    var color = this.image.color;
                    color.a = 1;
                    this.image.color = color;
                }
            }
        }
    }
}


