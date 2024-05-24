using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivePanel_RelicPanel : ActivePanel, IActivePanel
{
    public override void PanelActive(IActivePanel previousPanel)
    {
        this.previousPanel = previousPanel;
        previousPanel.DisablePanel();
        gameObject.SetActive(true);
        UIManager.Instance.activePanel = this;
        UIManager.Instance.DataPanel = transform.GetChild(1).gameObject;
        transform.GetChild(1).GetChild(4).GetComponent<RelicSelectButton>().relicPanelActive(0);
    }

    public override void PanelInactive()
    {
        UIManager.Instance.activePanel = previousPanel;
        gameObject.SetActive(false);
        if(previousPanel != null) { previousPanel.EnablePanel(); }
        UIManager.Instance.DataPanel = null;

    }
}
