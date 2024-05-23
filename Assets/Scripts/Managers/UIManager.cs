using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public Transform itemGetContent_J;
    public Transform FTrans;
    float initFtransY;
    float UIscaleY;
    public GameObject mainPanel;
    IActivePanel mainPanel_IActivePanel;

    public GameObject characterPanel;

    public Transform UI;
    public GameObject DataPanel;

    public GameObject damageTextPrefap;
    public Transform MonsterPoint;
    public TextMeshPro damageText;



    private void Awake()
    {
        if(androidB != null && editorB != null)
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


        settingBar_IActivePanel = settingBar.GetComponent<IActivePanel>();
        Debug.Log(settingBar_IActivePanel);
        initFtransY = FTrans.position.y;
        UIscaleY = UI.transform.localScale.y;

        mainPanel_IActivePanel = mainPanel.GetComponent<IActivePanel>();
        activePanel = mainPanel_IActivePanel;
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

    public void AddGetSlot_J(ItemGetPanelSlot itemGetPanelSlot)
    {

        itemGetPanelSlot.transform.SetParent(itemGetContent_J);
        itemGetContent_J.transform.parent.parent.parent.gameObject.SetActive(true);
        itemGetPanelSlot.transform.localScale = Vector3.one;
    }

    public void Check_GetSlot_J()
    {
        if (itemGetContent_J.childCount == 0)
        {
            itemGetContent_J.transform.parent.parent.parent.gameObject.SetActive(false);
        }
        else
        {
            itemGetContent_J.transform.parent.parent.parent.gameObject.SetActive(true);
        }
    }

    public void showDataUpdate(int id, CharacterItemSprite character)
    {
        IItemPanel itemPanel = DataPanel.GetComponent<IItemPanel>();
        itemPanel.UpdateItemPanel(id, character);
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

    public void DamageText(float damage, Vector3 monsterPos, Transform playerTrans)
    {
        GameObject next = Instantiate(damageTextPrefap, monsterPos + Vector3.up, Quaternion.identity);
        next.GetComponent <TextMeshPro>().text = damage.ToString();
        next.transform.SetParent(MonsterPoint);
        next.transform.LookAt(transform);
    }

    public void BurstPoint(float point)
    {

    }

}
