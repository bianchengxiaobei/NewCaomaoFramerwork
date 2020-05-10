using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
/// <summary>
/// 新手教程UI发光提示
/// </summary>
public class UINewbieGlow : MonoBehaviour
{
    private Tweener m_scaleAnim;
    private Tweener m_innerOutFadeAnim;
    private Tweener m_innerInFadeAnim;
    private Tweener m_scaleFadeAnim;
    private Sequence m_seqAnim;
    public Image scaleImage;
    private Vector2 targetSize;
    private Vector2 orginSize;
    private Image m_innerImage;
    private void Awake()
    {
        this.m_innerImage = this.GetComponent<Image>();
        if (this.scaleImage == null) 
        {
            this.scaleImage = this.transform.Find("ScaleImage").GetComponent<Image>();
        }
    }


    public void SetGlow(RectTransform center,Vector2? targetSize = null,Vector2? orginSize = null) 
    {
        this.transform.position = center.position;
        if (targetSize == null) 
        {
            this.targetSize = center.sizeDelta * 1.125f;
            this.orginSize = center.sizeDelta * 4;
        }
        else
        {
            this.targetSize = targetSize.Value;
            this.orginSize = orginSize.Value;
        }
        this.m_innerImage.rectTransform.sizeDelta = this.targetSize;
        this.scaleImage.rectTransform.sizeDelta = this.orginSize;
        this.PlayAnimation();
    }

    public void SetNoVisiable() 
    {
        this.transform.position = Vector3.one * 1000;
        if (this.m_seqAnim != null) 
        {
            this.m_seqAnim.Pause();
        }       
    }


    private void PlayAnimation() 
    {
        if (this.m_scaleAnim == null) 
        {
            this.m_scaleAnim = this.scaleImage.rectTransform.DOSizeDelta(this.targetSize, 1f);
            this.m_scaleAnim.SetAutoKill(false);
            this.m_scaleAnim.SetEase(Ease.InQuint);//先慢后快
        }
        if (this.m_scaleFadeAnim == null)
        {
            this.m_scaleFadeAnim = this.scaleImage.DOFade(0,2f);
            this.m_scaleFadeAnim.SetAutoKill(false);
            //this.m_scaleFadeAnim.SetDelay(1f);
        }
        if (this.m_innerInFadeAnim == null) 
        {
            this.m_innerInFadeAnim = this.m_innerImage.DOFade(1, 1f);
            this.m_innerInFadeAnim.SetAutoKill(false);
        }
        if (this.m_innerOutFadeAnim == null) 
        {
            this.m_innerOutFadeAnim = this.m_innerImage.DOFade(0, 4f);
            this.m_innerOutFadeAnim.SetAutoKill(false);
        }
        if (this.m_seqAnim == null) 
        {
            this.m_seqAnim = DOTween.Sequence();
            this.m_seqAnim.Append(this.m_scaleAnim);
            //this.m_seqAnim.Insert(1f, this.m_innerInFadeAnim);
            this.m_seqAnim.Insert(1f,this.m_scaleFadeAnim);
            //this.m_seqAnim.AppendInterval(3f);
            this.m_seqAnim.Append(this.m_innerOutFadeAnim);
            this.m_seqAnim.SetLoops(-1);
        }
        this.m_seqAnim.Restart();
    }
}
