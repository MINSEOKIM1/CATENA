using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MonsterAttackInfo")]
public class MonsterAttackInfo : ScriptableObject
{
    public bool isMelee;
    public Vector2 offset, boxSize;
    public int attackNum;
    public int mp;
}
