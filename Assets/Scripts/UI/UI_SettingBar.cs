using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SettingBar : ActivePanel
{
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void EnablePanel()
    {
        animator.Play("Enable_Panel");
    }

    public override void DisablePanel()
    {
        animator.Play("Disable_Panel");
    }

}
