using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyCSVLoder : MonoBehaviour
{
    public static EnemyCSVLoder Instance;

    private Dictionary<MonsterType, EnemyCSVData> _enemyDataDictionary;

    private void Awake()
    {
        Instance = this;

        LoadEnemyCSV();
    }

    private void LoadEnemyCSV()
    {
        _enemyDataDictionary = new Dictionary<MonsterType, EnemyCSVData>();

        TextAsset csvText = Resources.Load<TextAsset>("Data/EnemyData"); //지정된 경로에서 파일 로드

        string[] rows = csvText.text.Split('\n'); //하나의 행을 줄바꿈처리함. 

        for(int i = 1; i < rows.Length; i++) //첫줄은 헤더여서 건너뛰고 그 다음부터 실행함.
        {
            if (string.IsNullOrWhiteSpace(rows[i]))
            {
                continue;
            }

            string[] fields = rows[i].Split(',');

            int id = ParseInt(fields[0]);
            string name = fields[1];
            float health = ParseFloat(fields[2]);
            float power = ParseFloat(fields[3]);
            float speed = ParseFloat(fields[4]);
            float defence = ParseFloat(fields[5]);
            Element element = ParseEnum(fields[6]);

            EnemyCSVData data = new EnemyCSVData(id,name,health,power,speed,defence,element);

            _enemyDataDictionary.Add((MonsterType)i,data);  
        }
    }

    public EnemyCSVData GetData(MonsterType type)
    {
        if(_enemyDataDictionary.TryGetValue(type, out EnemyCSVData data))
        {
            return data;
        }
        else
        {
            Debug.Log("데이터 가져오기 실패");
            return null;
        } 
    }

    private Element ParseEnum(string value)
    {
        Element element;

        if(!Enum.TryParse(value, true ,out element))
        {
            element = Element.Normal;
        }

        return element;
    }

    private float ParseFloat(string value)
    {
        float.TryParse(value, out float result);
        return result;
    }


    private int ParseInt(string value)
    {
        int.TryParse(value, out int result);
        return result;  
    }

}
