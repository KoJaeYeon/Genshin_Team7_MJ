using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using UnityEngine;
using UnityEditor;

public enum EnemyID
{
    FireH = 1,
    IceH,
    NormalH,
    LightningH,
    Andrius
}

public class EnemyCSVLoder : MonoBehaviour
{
    public static EnemyCSVLoder Instance;
    private Dictionary<string, EnemyCSVData> _dataDictionary = new Dictionary<string, EnemyCSVData>();

    private void Awake()
    {
        Instance = this;

        LoadEnemyCSV();
    }

    private void LoadEnemyCSV()
    {
        TextAsset csvText = Resources.Load<TextAsset>("Data/Data");

        string[] rows = csvText.text.Split('\n');

        ParseData(rows);
    }

    public EnemyCSVData GetEnemyData(EnemyID id)
    {
        string dictionaryKey = id.ToString();

        if(_dataDictionary.TryGetValue(dictionaryKey, out EnemyCSVData data))
        {
            return data;
        }
        else
        {
            LoadEnemyCSV();

            EnemyCSVData currentData = _dataDictionary[dictionaryKey];
            
            if(currentData != null)
            {
                return currentData;
            }
            else
            {
                EnemyCSVData defaultData = new EnemyCSVData("default", "null", 0,0,0,0,Element.Null, 
                    0,0,Color.white);

                return defaultData;
            }
        }
    }

    private void ParseData(string[] stringArray)
    {
        for (int i = 1; i < stringArray.Length; i++) 
        {
            if (string.IsNullOrWhiteSpace(stringArray[i]))
            {
                continue;
            }

            string[] fields = stringArray[i].Split(',');

            foreach(string field in fields)
            {
                Debug.Log(field);
            }

            string id = fields[0];
            string name = fields[1];
            float health = ParseFloat(fields[2]);
            float power = ParseFloat(fields[3]);
            float speed = ParseFloat(fields[4]);
            float traceDistance = ParseFloat(fields[5]);
            Element element = ParseElenemtEnum(fields[6]);
            float paralyzation = ParseFloat(fields[7]);
            float defence = ParseFloat(fields[8]) * 0.01f;
            Color color = ParseColor(fields[9]);

            EnemyCSVData newData = new EnemyCSVData(id, name, health, power, speed, traceDistance, element, 
                paralyzation, defence, color);

            _dataDictionary.Add(id, newData);

            //var filePath = Path.Combine(Application.dataPath, $"Resources/Data/EnemyData{name}.asset");

            //if (File.Exists(filePath))
            //{
            //    return;
            //}
            //else
            //{
            //    EnemyScriptableObject enemyScriptableData = ScriptableObject.CreateInstance<EnemyScriptableObject>();
            //    enemyScriptableData.ID = id;
            //    enemyScriptableData.Name = name;
            //    enemyScriptableData.Health = health;
            //    enemyScriptableData.Power = power;
            //    enemyScriptableData.Speed = speed;
            //    enemyScriptableData.TraceDistance = traceDistance;
            //    enemyScriptableData.Element = element;
            //    enemyScriptableData.Paralyzation = paralyzation;
            //    enemyScriptableData.Defence = defence;
            //    enemyScriptableData.Color = color;

            //    var scriptableSavepath = $"Assets/Resources/Data/EnemyData{name}.asset";
            //    AssetDatabase.CreateAsset(enemyScriptableData, scriptableSavepath);
            //    AssetDatabase.SaveAssets();
            //}
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

    private Element ParseElenemtEnum(string value)
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
