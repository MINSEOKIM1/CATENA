using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDataProcessor : MonoBehaviour
{
    public MonsterInfo monsterInfo;

    public float hp, maxHp, mp, maxMp;
    public float speed;

    private void Start()
    {
        hp = monsterInfo.hp;
        maxHp = hp;
        
        mp = monsterInfo.mp;
        maxHp = mp;
    }
}
