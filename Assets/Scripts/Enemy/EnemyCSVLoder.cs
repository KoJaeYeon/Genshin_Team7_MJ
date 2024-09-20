using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Globalization;

public class EnemyCSVLoder : MonoBehaviour
{
    public static EnemyCSVLoder Instance;

    private Dictionary<string, EnemyCSVData> _dataDictionary = new Dictionary<string, EnemyCSVData> ();
    private Dictionary<string, AndriusCSVData> _andriusDictionary = new Dictionary<string, AndriusCSVData>();

    private void Awake()
    {
        Instance = this;

        LoadEnemyBaseCSV();
        LoadEnemyElementCSV();
        LoadEnemyOverlapCSV();
        LoadEnemyTraceCSV();

        StartCoroutine(LoadAndriusBasePatternCSV());
        StartCoroutine(LoadAndriusSkillPatternCSV());
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
            float defence = ParseFloat(fields[5]) * 0.01f;

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
            float nextMoveTime = ParseFloat(fields[4]);

            EnemyTraceData traceData = new EnemyTraceData(id, traceDistance, stopDistance,
                agentStopDistacne, nextMoveTime);

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
                    ParseAndriusParalyzation(nextSplitArray);
                    break;
                case nameof(AndriusWalkData):
                    ParseAndriusWalk(nextSplitArray);
                    break;
                case nameof(AndriusAttackData):
                    ParseAndriusAttack(nextSplitArray, splitArray);
                    break;
            }
        }

        return true;
    }

    private void ParseAndriusParalyzation(string[] nextSplitArray)
    {
        string id = nextSplitArray[0];
        float changeTime = ParseFloat(nextSplitArray[1]);
        float paralyzationValue = ParseFloat(nextSplitArray[2]);

        AndriusParalyzationData data = new AndriusParalyzationData(id, changeTime, paralyzationValue);

        if (!_andriusDictionary.ContainsKey(id))
        {
            _andriusDictionary.Add(id, data);
        }
    }

    private void ParseAndriusWalk(string[] nextSplitArray)
    {
        string id = nextSplitArray[0];
        float speed = ParseFloat(nextSplitArray[1]);
        float walkTime = ParseFloat(nextSplitArray[2]);

        AndriusWalkData data = new AndriusWalkData(id, speed, walkTime);

        if (!_andriusDictionary.ContainsKey(id))
        {
            _andriusDictionary.Add(id, data);
        }
    }

    private void ParseAndriusAttack(string[] nextSplitArray, string[] splitArray)
    {
        AndriusAttackData data = new AndriusAttackData();

        data.Data = new Dictionary<AttackDataList, float>();

        string id = nextSplitArray[0];

        string[] headerArray = splitArray[0].Split(',');

        data.Id = id;

        for(int i = 1; i < nextSplitArray.Length; i++)
        {
            float value = ParseFloat(nextSplitArray[i]);

            string header = headerArray[i];

            if (header == "Turn_leftAngle" || 
                header == "Back_leftAngle")
            {
                value *= -1f;
            }

            data.Data.Add((AttackDataList)i, value);
        }

        if (!_andriusDictionary.ContainsKey(id))
        {
            _andriusDictionary.Add(id, data);
        }
    }


    private IEnumerator LoadAndriusSkillPatternCSV()
    {
        TextAsset andriusSkillCSV = Resources.Load<TextAsset>("Data/AndriusData/AndriusSkillData");

        string[] splitArray = andriusSkillCSV.text.Split('\n');

        for(int i = 1; i < splitArray.Length;i++)
        {
            string[] fieldes = splitArray[i].Split(',');

            if (string.IsNullOrWhiteSpace(splitArray[i]))
            {
                continue;
            }

            yield return new WaitUntil(() => ParseAndriusSkillPattern(fieldes));
        }
    }
    
    private bool ParseAndriusSkillPattern(string[] fieldArray)
    {
        string id = fieldArray[0];
        float moveSpeed = ParseFloat(fieldArray[1]);
        float rotationSpeed = ParseFloat(fieldArray[2]);
        float chargeTime = ParseFloat(fieldArray[3]);
        float maxAngle = ParseFloat(fieldArray[4]);
        float maxNormalizedTime = ParseFloat(fieldArray[5]);

        if(id == nameof(AndriusHowlData))
        {
            string replace = Regex.Replace(fieldArray[6], "[{}\"]", "");
            string[] split = replace.Split('/');

            float howlDamage = ParseFloat(split[0].Trim());
            float iceDamage = ParseFloat(split[1].Trim());

            AndriusHowlData howlData = new AndriusHowlData(id, moveSpeed, rotationSpeed, chargeTime,
                maxAngle, maxNormalizedTime);

            howlData.Data = new Dictionary<DataList, float>
            {
                { DataList.SKillDamage_howl, howlDamage },
                { DataList.SKillDamage_ice, iceDamage}
            };

            AddData(id, howlData);
            
            return true;
        }

        float skillDamage = ParseFloat(fieldArray[6]);

        switch (id)
        {
            case nameof(AndriusJumpData):
                AndriusJumpData jumpData = new AndriusJumpData(id, moveSpeed, rotationSpeed,
                    chargeTime, maxAngle, maxNormalizedTime, skillDamage);
                AddData(id, jumpData);
                break;
            case nameof(AndriusClawData):
                AndriusClawData clawData = new AndriusClawData(id, moveSpeed, rotationSpeed,
                    chargeTime, maxAngle, maxNormalizedTime, skillDamage);
                AddData(id, clawData);
                break;
            case nameof(AndriusChargeData):
                AndriusChargeData chargeData = new AndriusChargeData(id, moveSpeed, rotationSpeed,
                    chargeTime, maxAngle, maxNormalizedTime, skillDamage);
                AddData(id, chargeData);
                break;
            case nameof(AndriusStampData):
                AndriusStampData stampData = new AndriusStampData(id, moveSpeed, rotationSpeed,
                    chargeTime, maxAngle, maxNormalizedTime, skillDamage);
                AddData(id, stampData);
                break;
            case nameof(AndriusDriftData):
                AndriusDriftData driftData = new AndriusDriftData(id, moveSpeed, rotationSpeed,
                    chargeTime, maxAngle, maxNormalizedTime, skillDamage);
                AddData(id, driftData);
                break;
        }

        return true;        
    }

    private void AddData(string id, AndriusCSVData data)
    {
        if (!_andriusDictionary.ContainsKey(id))
        {
            _andriusDictionary.Add(id, data);
        }
        else
        {
            Debug.Log($"{id}데이터가 딕셔너리에 들어가지 않았습니다(Add).");
        }
    }

    public T GetEnemyCSVData<T>(string id) where T : class
    {
        var data = GetEnemyCSV(id);

        if(data is T tData)
        {
            return tData;
        }
        else
        {
            Debug.Log("T 변환 실패(GetEnemyCSVData)");
            return null;
        }
    }

    public T GetAndriusCSVData<T>(string id) where T : class
    {
        var data = GetAndriusCSV(id);

        if (data is T tData)
        {
            return tData;
        }
        else
        {
            Debug.Log("T 변환 실패(GetAndriusCSVData)");
            return null;
        }
    }

    private AndriusCSVData GetAndriusCSV(string id)
    {
        if(_andriusDictionary.TryGetValue(id, out AndriusCSVData data))
        {
            return data;
        }
        else
        {
            Debug.Log($"{id}의 AndriusData를 가져오지 못했습니다(Get).");
            return null;
        }
    }

    private EnemyCSVData GetEnemyCSV(string id)
    {
        if (_dataDictionary.TryGetValue(id, out EnemyCSVData data))
        {
            return data;
        }
        else
        {
            Debug.Log($"{id}의 EnemyData를 가져오지 못했습니다(Get).");
            return null;
        }
    }

    #region ParseValue
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
    #endregion

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
