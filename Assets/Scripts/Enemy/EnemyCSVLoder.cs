using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class EnemyCSVLoder : MonoBehaviour
{
    public static EnemyCSVLoder Instance;

    private void Awake()
    {
        Instance = this;

        LoadEnemyCSV();
    }

 
    private void LoadEnemyCSV()
    {
        TextAsset csvText = Resources.Load<TextAsset>("Data/EnemyData"); //지정된 경로에서 파일 로드

        string[] rows = csvText.text.Split('\n'); //하나의 행을 줄바꿈처리함. 

        ParseData(rows);
    }

    private void ParseData(string[] stringArray)
    {
        for (int i = 1; i < stringArray.Length; i++) //첫줄은 헤더여서 건너뛰고 그 다음부터 실행함.
        {
            if (string.IsNullOrWhiteSpace(stringArray[i]))
            {
                continue;
            }

            string[] fields = stringArray[i].Split(',');

            int id = ParseInt(fields[0]);
            string name = fields[1];
            float health = ParseFloat(fields[2]);
            float power = ParseFloat(fields[3]);
            float speed = ParseFloat(fields[4]);
            float traceDistance = ParseFloat(fields[5]);
            Element element = ParseEnum(fields[6]);
            float paralyzation = ParseFloat(fields[7]);
            float defence = ParseFloat(fields[8]) * 0.01f;
            Color color = ParseColor(fields[9]);

            var filePath = Path.Combine(Application.dataPath, $"Resources/Data/EnemyData{name}.asset");

            if (File.Exists(filePath))
            {
                return;
            }
            else
            {
                EnemyScriptableObject enemyScriptableData = ScriptableObject.CreateInstance<EnemyScriptableObject>();
                enemyScriptableData.ID = id;
                enemyScriptableData.Name = name;
                enemyScriptableData.Health = health;
                enemyScriptableData.Power = power;
                enemyScriptableData.Speed = speed;
                enemyScriptableData.TraceDistance = traceDistance;
                enemyScriptableData.Element = element;
                enemyScriptableData.Paralyzation = paralyzation;
                enemyScriptableData.Defence = defence;
                enemyScriptableData.Color = color;

                var scriptableSavepath = $"Assets/Resources/Data/EnemyData{name}.asset";
                AssetDatabase.CreateAsset(enemyScriptableData, scriptableSavepath);
                AssetDatabase.SaveAssets();
            }
        }
    }

    private Color ParseColor(string value)
    {
        Color color;
        Debug.Log(value);
        if (ColorUtility.TryParseHtmlString(value, out color))
        {
            return color;
        }
        else
        {
            return Color.white;
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
