using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActivePanel
{
    void PanelActive(IActivePanel previousPanel);
    void PanelInactive();
}
