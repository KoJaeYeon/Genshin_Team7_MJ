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
    // Transform 받는 변수 선언
    public Transform FTrans;
    float initFtransY;
    float UIscaleY;
    public GameObject mainPanel;
    IActivePanel mainPanel_IActivePanel;

    public Transform UI;

    private void Awake()
    {
        settingBar_IActivePanel = settingBar.GetComponent<IActivePanel>();
        Debug.Log(settingBar_IActivePanel);
        initFtransY = FTrans.position.y;
        UIscaleY = UI.transform.localScale.y;

        mainPanel_IActivePanel = mainPanel.GetComponent<IActivePanel>();
        activePanel = mainPanel_IActivePanel;
    }

    void Start()
    {
        #if UNITY_ANDROID
        //안드로이드
        androidB.gameObject.SetActive(true);
        editorB.gameObject.SetActive(false);
        #elif UNITY_EDITOR
        //에디터
        androidB.gameObject.SetActive(false);
        editorB.gameObject.SetActive(true);
        #endif
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
        if (activePanel != mainPanel_IActivePanel) { activePanel.PanelInactive(); }
        else {  settingBar_IActivePanel.PanelActive(activePanel);  }
        
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
        getSlot.transform.SetAsFirstSibling();
        getSlot.transform.localScale = Vector3.one;

    }

    public void RemoveGetSlot()
    {
        if(itemGetContent.childCount == 0)
        {
            itemGetContent.transform.parent.parent.gameObject.SetActive(false);
        }
    }

    public void SetFPoint(int searchPoint)
    {
        FTrans.position = new Vector3(FTrans.position.x,initFtransY - ((itemGetContent.transform.childCount -1) * 37 ) + (searchPoint * 75 ) * UIscaleY,FTrans.position.z);
        if (itemGetContent.childCount == 0)
        {
            itemGetContent.transform.parent.parent.gameObject.SetActive(false);
        }
    }
}
