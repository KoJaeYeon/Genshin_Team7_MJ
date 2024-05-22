using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivePanel_WeaponPanel : MonoBehaviour, IActivePanel
{
    IActivePanel previousPanel;
    Animator animator;

    bool _hasAnimator = false;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator != null) _hasAnimator = true;
    }
    public void PanelActive(IActivePanel previousPanel)
    {
        this.previousPanel = previousPanel;
        previousPanel.DisablePanel();
        gameObject.SetActive(true);
        UIManager.Instance.activePanel = this;
        InventoryManager.Instance.Load_Weapon(transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0));
        UIManager.Instance.DataPanel = transform.GetChild(1).gameObject;
        if(transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).childCount != 0 )
        {
            transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetComponent<ItemSlot>().ShowData();
        }

    }

    public void PanelInactive()
    {
        UIManager.Instance.activePanel = previousPanel;
        gameObject.SetActive(false);
        if(previousPanel != null) { previousPanel.EnablePanel(); }
        InventoryManager.Instance.UnLoad_Weapon();
        UIManager.Instance.DataPanel = null;
        
    }
    public virtual void DisablePanel()
    {
        if (_hasAnimator) animator.Play("Disable_Panel");
    }
    public virtual void EnablePanel()
    {
        if(_hasAnimator) animator.Play("Enable_Panel");
    }

}
