using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivePanel_CharacterPanel : MonoBehaviour, IActivePanel
{
    IActivePanel previousPanel;
    Animator animator;
    public Light _light;
    public RenderManager renderManager;
    characterItemSettingButton characterItemSettingButton;
    GameObject property;

    bool _hasAnimator = false;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator != null) _hasAnimator = true;
        characterItemSettingButton = transform.GetChild(5).GetComponent<characterItemSettingButton>();
        property = transform.GetChild(4).gameObject;
    }
    public void PanelActive(IActivePanel previousPanel)
    {
        this.previousPanel = previousPanel;
        UIManager.Instance.activePanel = this;
        previousPanel.DisablePanel();
        gameObject.SetActive(true);
        _light.gameObject.SetActive(false); // �ʵ� ���� ����
        renderManager.gameObject.SetActive(true); // ����ŷ ĳ���� �ִ� ���� Ȱ��ȭ
        renderManager.ChangeCharacter(0);
        characterItemSettingButton.SelectActive(0);
    }

    public void PanelInactive()
    {
        UIManager.Instance.activePanel = previousPanel;
        renderManager.gameObject.SetActive(false);
        _light.gameObject.SetActive(true);
        gameObject.SetActive(false);
        if(previousPanel != null) { previousPanel.EnablePanel(); }
        
    }
    public virtual void DisablePanel()
    {
        property.SetActive(false);
        if (_hasAnimator) animator.Play("Disable_Panel");
    }
    public virtual void EnablePanel()
    {
        property.SetActive(true);
        if(_hasAnimator) animator.Play("Enable_Panel");
    }

    public void TimeRenew()
    {
        Time.timeScale = 1;
    }

}
