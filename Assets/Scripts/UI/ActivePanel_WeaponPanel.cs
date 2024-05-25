using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivePanel_WeaponPanel : ActivePanel
{
    public override void PanelActive(IActivePanel previousPanel)
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

    public override void PanelInactive()
    {
        UIManager.Instance.activePanel = previousPanel;
        gameObject.SetActive(false);
        if(previousPanel != null) { previousPanel.EnablePanel(); }
        InventoryManager.Instance.UnLoad_Weapon();
        UIManager.Instance.DataPanel = null;
        
    }
    public override void DisablePanel()
    {
        if (_hasAnimator) animator.Play("Disable_Panel");
    }
    public override void EnablePanel()
    {
        if(_hasAnimator) animator.Play("Enable_Panel");
    }

}
