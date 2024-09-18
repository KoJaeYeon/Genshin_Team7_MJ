using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//기본적으로 파일 이름을 EnemyData 우클릭 생성 메뉴 이름을ScriptableObjects의 EnemyData로 하겠다.
[CreateAssetMenu( menuName = "Scriptable/EnemyDatas")]
public class EnemyScriptableObject : ScriptableObject
{
    [Header("Id")]
    [SerializeField] private int _id;
    [Header("Name")]
    [SerializeField] private string _name;
    [Header("Health")]
    [SerializeField] private float _health;
    [Header("Power")]
    [SerializeField] private float _power;
    [Header("Speed")]
    [SerializeField] private float _speed;
    [Header("TraceDistance")]
    [SerializeField] private float _traceDistance;
    [Header("Paralyzation")]
    [SerializeField] private float _paralyzation;
    [Header("Element")]
    [SerializeField] private Element _element;
    [Header("Defence")]
    [SerializeField] private float _defence;
    [Header("Color")]
    [SerializeField] private Color _color;

    #region Property
    public int ID
    {
        get => _id;
        set => _id = value;
    }

    public string Name
    {
        get => _name;
        set => _name = value;
    }

    public float Health
    {
        get => _health;
        set => _health = value;
    }

    public float Power
    {
        get => _power;
        set => _power = value;
    }

    public float Speed
    {
        get => _speed;
        set => _speed = value;
    }

    public float TraceDistance
    {
        get => _traceDistance;
        set => _traceDistance = value;
    }

    public float Paralyzation
    {
        get => _paralyzation;
        set => _paralyzation = value;   
    }

    public Element Element
    {
        get => _element;
        set => _element = value;
    }

    public float Defence
    {
        get => _defence;
        set => _defence = value;
    }

    public Color Color
    {
        get => _color;
        set => _color = value;
    }
    #endregion

}
