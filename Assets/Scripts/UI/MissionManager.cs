using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MissionManager : Singleton<MissionManager>
{
    public Transform missionPanel;
    TextMeshProUGUI titleText;
    TextMeshProUGUI leftObjectText;
    TextMeshProUGUI leftTimeText;
    TextMeshProUGUI topTimeText;
    public Mission mission;

    public GameObject overText;
    public GameObject clearText;

    int maxObj;
    private void Awake()
    {
        titleText = missionPanel.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        leftObjectText =missionPanel.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        leftTimeText = missionPanel.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>();
        topTimeText = missionPanel.transform.GetChild(2).GetChild(0).GetChild(0).GetComponent <TextMeshProUGUI>();

        overText.gameObject.SetActive(false);
        clearText.gameObject.SetActive(false);
    }

    public void StartMission(int time, int maxObj)
    {
        missionPanel.gameObject.SetActive(true);
        this.maxObj = maxObj;
        titleText.text = $"{time}�� ���� �ٶ� ���� {maxObj}�� �����ϱ�";
        leftObjectText.text = $"���� ���� 0/{maxObj}";
    }
    public void UpdateObj(int obj)
    {
        leftObjectText.text = $"���� ���� {obj}/{maxObj}";
    }
    public void UpdateTime(int time)
    {
        TimeSpan time_mmss = TimeSpan.FromSeconds(time);
        leftTimeText.text = $"���� �ð�: {time}��";
        topTimeText.text = time_mmss.ToString(@"mm\:ss");
    }

    public void Succeed()
    {
        mission = null;
        missionPanel.gameObject.SetActive(false);
        clearText.gameObject.SetActive(true);

    }

    public void Failed()
    {
        mission = null;
        missionPanel.gameObject.SetActive(false);
        overText.gameObject.SetActive(true);
    }
    
}
