using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;
using System.Globalization;
using UnityEngine.InputSystem;
public enum EnemyID
{
    FireH = 1,
    IceH,
    NormalH,
    LightningH,
    Andrius
}

public enum AndriusData
{
    BasePattern,
    SkillPattern
}

public class EnemyCSVLoder : MonoBehaviour
{
    public static EnemyCSVLoder Instance;

    private Dictionary<string, EnemyCSVData> _dataDictionary;
    private Dictionary<AndriusData, List<AndriusPatternData>> _andriusDictionary;

    private void Awake()
    {
        Instance = this;

        _dataDictionary = new Dictionary<string, EnemyCSVData>();

        LoadEnemyBaseCSV();
        LoadEnemyElementCSV();
        LoadEnemyOverlapCSV();
        LoadEnemyTraceCSV();

        _andriusDictionary = new Dictionary<AndriusData, List<AndriusPatternData>>();

    }

    private void LoadEnemyBaseCSV()
    {
        TextAsset baseCSV = Resources.Load<TextAsset>("Data/BaseData/BaseData");

        string[] splitArray = baseCSV.text.Split('\n');

        for(int i = 1; i <  splitArray.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(splitArray[i]))
            {
                continue;
            }

            string[] fields = splitArray[i].Split(',');

            string id = fields[0];
            string name = fields[1];
            float health = ParseFloat(fields[2]);
            float power = ParseFloat(fields[3]);
            float speed = ParseFloat(fields[4]);
            float defence = ParseFloat(fields[5]);

            EnemyBaseData baseData = new EnemyBaseData(id, name, health, power, speed, defence);

            _dataDictionary.Add(id, baseData);
        }
    }

    private void LoadEnemyElementCSV()
    {
        TextAsset elementCSV = Resources.Load<TextAsset>("Data/BaseData/ElementData");

        string[] splitArray = elementCSV.text.Split('\n');

        for (int i = 1; i <  splitArray.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(splitArray[i]))
            {
                continue;
            }

            string[] fields = splitArray[i].Split(',');

            string id = fields[0];
            Element element = ParseElenemtEnum(fields[1]);
            Color color = ParseColor(fields[2]);
            int dropCount = ParseInt(fields[3]);

            EnemyElementData elementData = new EnemyElementData(id, element, color, dropCount);
            _dataDictionary.Add(id, elementData);
        }
    }

    private void LoadEnemyOverlapCSV()
    {
        TextAsset overlapCSV = Resources.Load<TextAsset>("Data/BaseData/OverlapData");

        string[] splitArray = overlapCSV.text.Split('\n');

        for(int i = 1; i < splitArray.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(splitArray[i]))
            {
                continue;
            }

            string[] fields = splitArray[i].Split(',');

            string id = fields[0];

            List<Vector3> vector3List = new List<Vector3>();

            for(int k = 1; k < fields.Length; k++)
            {
                Vector3 newVector3 = ParseVector3(fields[k]);

                vector3List.Add(newVector3);
            }
            
            EnemyOverlapData overlapData = new EnemyOverlapData(id, vector3List);

            _dataDictionary.Add(id, overlapData);
        }
    }

    private void LoadEnemyTraceCSV()
    {
        TextAsset traceCSV = Resources.Load<TextAsset>("Data/BaseData/TraceData");

        string[] splitArray = traceCSV.text.Split('\n');

        for(int i = 1; i < splitArray.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(splitArray[i]))
            {
                continue;
            }

            string[] fields = splitArray[i].Split(',');

            string id = fields[0];
            float traceDistance = ParseFloat(fields[1]);
            float stopDistance = ParseFloat(fields[2]);
            float agentStopDistacne = ParseFloat(fields[3]);

            EnemyTraceData traceData = new EnemyTraceData(id, traceDistance, stopDistance, agentStopDistacne);

            _dataDictionary.Add(id, traceData);
        }
    }

    private IEnumerator LoadAndriusBasePatternCSV()
    {
        List<TextAsset> textList = new List<TextAsset>();

        TextAsset paralyzationCSV = Resources.Load<TextAsset>("Data/AndriusData/AndriusParalyzation");
        TextAsset walkCSV = Resources.Load<TextAsset>("Data/AndriusData/AndriusWalkData");
        TextAsset attackCSV = Resources.Load<TextAsset>("Data/AndriusData/AndriusAttackData");

        textList.Add(paralyzationCSV);
        textList.Add(walkCSV);
        textList.Add(attackCSV);

        foreach(var textAsset in  textList)
        {
            string[] splitArray = textAsset.text.Split('\n');

            yield return new WaitUntil(() => ParseAndrius(splitArray));
        }
    }

    private bool ParseAndrius(string[] splitArray)
    {
        for(int i = 1; i < splitArray.Length; i++)
        {
            string[] nextSplitArray = splitArray[i].Split(',');

            string id = nextSplitArray[0];

            switch (id)
            {
                case nameof(AndriusParalyzationData):
                    ParseAndriusParalyzation(nextSplitArray, AndriusData.BasePattern);
                    break;
                case nameof(AndriusWalkData):
                    ParseAndriusWalk(nextSplitArray, AndriusData.BasePattern);
                    break;
                case nameof(AndriusAttackData):
                    ParseAndriusAttack(nextSplitArray, AndriusData.BasePattern);
                    break;
            }
        }

        return true;
    }

    private void ParseAndriusParalyzation(string[] nextSplitArray, AndriusData key)
    {
        string id = nextSplitArray[0];
        float changeTime = ParseFloat(nextSplitArray[1]);
        float paralyzationValue = ParseFloat(nextSplitArray[2]);

        AndriusParalyzationData data = new AndriusParalyzationData(id, changeTime, paralyzationValue);

        AddAndriusData(data, key);
    }

    private void ParseAndriusWalk(string[] nextSplitArray, AndriusData key)
    {
        string id = nextSplitArray[0];
        float speed = ParseFloat(nextSplitArray[1]);
        float walkTime = ParseFloat(nextSplitArray[2]);

        AndriusWalkData data = new AndriusWalkData(id, speed, walkTime);

        AddAndriusData(data, key);
    }

    private void ParseAndriusAttack(string[] nextSplitArray, AndriusData key)
    {
        AndriusAttackData data = new AndriusAttackData();

        string id = nextSplitArray[0];

        data.Id = id;

        for(int i = 1; i < nextSplitArray.Length; i++)
        {
            float value = ParseFloat(nextSplitArray[i]);

            data.Data.Add((AndriusAttackData.DataList)i, value);
        }

        AddAndriusData(data, key);
    }


    private void AddAndriusData(AndriusPatternData data, AndriusData key)
    {
        if (!_andriusDictionary.ContainsKey(key))
        {
            _andriusDictionary[key] = new List<AndriusPatternData>();
        }

        _andriusDictionary[key].Add(data);
    }

    //private IEnumerator LoadAndriusSkillPatternCSV()
    //{
    //    TextAsset andriusSkillCSV = Resources.Load<TextAsset>("Data/AndriusData/AndriusSkillData");

    //    string[] splitArray = andriusSkillCSV.text.Split('\n');

    //    string[] header = splitArray[0].Split(',');

    //    for(int i = 1; i <  splitArray.Length; i++)
    //    {
    //        string[] fields = splitArray[i].Split(',');

    //        if(header.Length == fields.Length)
    //        {

    //        }

    //    }
    //}


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

    private Vector3 ParseVector3(string value)
    {
        string replace = Regex.Replace(value, "[{}\"]", "");

        string[] replaceArray = replace.Split('/');

        float x = ParseFloat(replaceArray[0].Trim());
        float y = ParseFloat(replaceArray[1].Trim());
        float z = ParseFloat(replaceArray[2].Trim());

        Vector3 newVector3 = new Vector3(x, y, z);

        return newVector3;
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

    //public EnemyCSVData GetEnemyData(EnemyID id)
    //{
    //    string dictionaryKey = id.ToString();

    //    if (_dataDictionary.TryGetValue(dictionaryKey, out EnemyCSVData data))
    //    {
    //        return data;
    //    }
    //    else
    //    {
    //        LoadEnemyCSV();

    //        EnemyCSVData currentData = _dataDictionary[dictionaryKey];

    //        if (currentData != null)
    //        {
    //            return currentData;
    //        }
    //        else
    //        {
    //            EnemyCSVData defaultData = new EnemyCSVData("default", "null", 0, 0, 0, 0, Element.Null,
    //                0, 0, Color.white, new List<Vector3> { new Vector3(0, 0, 0), new Vector3(0, 0, 0) });

    //            return defaultData;
    //        }
    //    }
    //}

    #region SaveScriptableObject
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
    #endregion
}
