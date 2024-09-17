using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//기본적으로 파일 이름을 EnemyData 우클릭 생성 메뉴 이름을ScriptableObjects의 EnemyData로 하겠다.
[CreateAssetMenu( menuName = "Scriptable/EnemyDatas")]
public class EnemyScriptableObject : ScriptableObject
{
    public int _id;
    public string _name;
    public float _health;
    public float _power;
    public float _speed;
    public float _defence;
    public Element _element;
}
