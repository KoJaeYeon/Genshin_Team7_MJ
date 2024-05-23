using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivePanel_InventoryPanel : ActivePanel
{
    IActivePanel previousPanel;
    Animator animator;

    bool _hasAnimator = false;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator != null) _hasAnimator = true;
    }
    public override void PanelActive(IActivePanel previousPanel)
    {
        this.previousPanel = previousPanel;
        previousPanel.DisablePanel();
        gameObject.SetActive(true);
        UIManager.Instance.activePanel = this;
        UIManager.Instance.DataPanel = transform.GetChild(4).gameObject;
        transform.GetChild(0).transform.GetChild(3).GetComponent<InventorySelectButton>().InventoryActive(0);
    }

    public override void PanelInactive()
    {
        UIManager.Instance.activePanel = previousPanel;
        gameObject.SetActive(false);
        if(previousPanel != null) { previousPanel.EnablePanel(); }
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
