using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CUIHelpTip : MonoBehaviour
{
    public Text lb_tip;
    public Image sp_character;
    public Text lb_characterName;
    public Button bt_tip;
    private Vector3 m_orginPos;
    public void Awake()
    {
        this.lb_tip = this.transform.Find("lb_tip").GetComponent<Text>();
        this.lb_characterName = this.transform.Find("lb_characterName").GetComponent<Text>();
        this.sp_character = this.transform.Find("sp_character").GetComponent<Image>();
        this.m_orginPos = this.transform.position;
    }

    public void SetVisiable(bool bVisiable) 
    {
        if (bVisiable)
        {
            this.transform.position = this.m_orginPos;
        }
        else 
        {
            this.transform.position = Vector3.one * 1000; 
        }
    }
}
