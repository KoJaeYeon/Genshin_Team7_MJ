using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivePanel_InventoryPanel : ActivePanel
{
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
}
