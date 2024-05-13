using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public GameObject androidB;
    public GameObject editorB;

    public IActivePanel activePanel;

    void Start()
    {
         #if UNITY_ANDROID
        //�ȵ���̵�
        androidB.gameObject.SetActive(true);
        editorB.gameObject.SetActive(false);
        #elif UNITY_EDITOR
        //������
        androidB.gameObject.SetActive(false);
        editorB.gameObject.SetActive(true);
        #endif

    }

    public void QuitPanel()
    {
        activePanel.PanelInactive();
    }

    public void PanelActive(GameObject gameObject)
    {
        IActivePanel iactivePanel = gameObject.GetComponent<IActivePanel>();
        if (iactivePanel != null) { iactivePanel.PanelActive(activePanel); }
        else { Debug.LogWarning("NotPanel"); }
    }
}
