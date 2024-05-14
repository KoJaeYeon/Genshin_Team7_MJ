using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public GameObject androidB;
    public GameObject editorB;

    public IActivePanel activePanel;
    public GameObject settingBar;
    IActivePanel settingBar_IActivePanel;

    public Transform itemGetContent;

    public GameObject mainPanel;

    private void Awake()
    {
        settingBar_IActivePanel = settingBar.GetComponent<IActivePanel>();
    }

    void Start()
    {
        //#if UNITY_ANDROID
        ////안드로이드
        //androidB.gameObject.SetActive(true);
        //editorB.gameObject.SetActive(false);
        //#elif UNITY_EDITOR
        ////에디터
        //androidB.gameObject.SetActive(false);
        //editorB.gameObject.SetActive(true);
        //#endif
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            QuitPanel();
        }
    }

    public void QuitPanel()
    {
        if (activePanel != null) { activePanel.PanelInactive(); }
        else { settingBar_IActivePanel.PanelActive(activePanel); }
        
    }

    public void PanelActive(GameObject gameObject)
    {
        IActivePanel iactivePanel = gameObject.GetComponent<IActivePanel>();
        if (iactivePanel != null) { iactivePanel.PanelActive(activePanel); }
        else { Debug.LogWarning("NotPanel"); }
    }

    public void QuitAndPanelAcitve(GameObject gameObject)
    {
        QuitPanel();
        IActivePanel iactivePanel = gameObject.GetComponent<IActivePanel>();
        if (iactivePanel != null) { iactivePanel.PanelActive(activePanel); }
        else { Debug.LogWarning("NotPanel"); }
    }

    public void AddGetSlot(GetSlot getSlot)
    {
        if(!itemGetContent.transform.parent.parent.gameObject.activeSelf)
        {
            itemGetContent.transform.parent.parent.gameObject.SetActive(true);
        }
        getSlot.transform.SetParent(itemGetContent);
    }

    public void RemoveGetSlot()
    {
        if(transform.childCount == 0)
        {
            itemGetContent.transform.parent.parent.gameObject.SetActive(false);
        }
    }
}
