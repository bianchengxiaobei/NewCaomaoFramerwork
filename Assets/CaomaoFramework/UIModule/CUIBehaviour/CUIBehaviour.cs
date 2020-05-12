using UnityEngine;
using System.Collections;

public class CUIBehaviour : MonoBehaviour
{
    protected bool m_bVisiable = true;

    public virtual void Show(bool value)
    {
        if (this.m_bVisiable != value)
        {
            this.m_bVisiable = value;
            this.gameObject.SetActive(value);
        }
    }
}
