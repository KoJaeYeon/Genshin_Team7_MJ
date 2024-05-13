using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivePanel : MonoBehaviour, IActivePanel
{
    IActivePanel previousPanel;
    public void PanelActive(IActivePanel previousPanel)
    {
        this.previousPanel = previousPanel;
        gameObject.SetActive(true);
        UIManager.Instance.activePanel = this;
    }

    public void PanelInactive()
    {
        UIManager.Instance.activePanel = previousPanel;
        gameObject.SetActive(false);
    }
}
