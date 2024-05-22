using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivePanel_RelicPanel : MonoBehaviour, IActivePanel
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
        UIManager.Instance.DataPanel = transform.GetChild(1).gameObject;
        transform.GetChild(1).GetChild(4).GetComponent<RelicSelectButton>().relicPanelActive(0);
    }

    public void PanelInactive()
    {
        UIManager.Instance.activePanel = previousPanel;
        gameObject.SetActive(false);
        if(previousPanel != null) { previousPanel.EnablePanel(); }
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
