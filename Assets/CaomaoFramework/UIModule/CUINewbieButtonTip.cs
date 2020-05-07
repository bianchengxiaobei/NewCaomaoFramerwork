using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class CUINewbieButtonTip : MonoBehaviour
{
    public Text lb_tip;
    private Tweener m_anim;
    private Vector2 m_targetPos;
    private Vector2 m_offsetPos; 
    private bool bVisiable = true;
    public void SetTip(string content,Vector2 targetPos,bool down) 
    {
        this.lb_tip.text = content;
        this.m_targetPos = targetPos;
        this.m_offsetPos = down ? this.m_targetPos + Vector2.down * 50: this.m_targetPos + Vector2.up * 50;
        this.SetVisiable(true);
    }

    public void SetVisiable(bool bVisiable) 
    {
        if (this.bVisiable == bVisiable) 
        {
            return;
        }
        this.bVisiable = bVisiable;
        if (this.bVisiable)
        {
            this.transform.localPosition = this.m_offsetPos;
            if (this.m_anim == null)
            {
                this.m_anim = this.transform.DOLocalMove(this.m_targetPos, 0.5f, false);
                this.m_anim.SetAutoKill(false);
            }
            this.m_anim.Restart();
        }
        else 
        {
            this.transform.position = Vector3.one * 1000f;
        }
    }

}