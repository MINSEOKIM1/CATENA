using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MonsterInfo")]
public class MonsterInfo : ScriptableObject
{
    public string mobName;
    public float hp, mp;
    public float speed;
    public float[] stats; // ad, def;
    public List<MonsterAttackInfo> attacks;
}
