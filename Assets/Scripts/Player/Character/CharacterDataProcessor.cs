using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDataProcessor : MonoBehaviour
{
    public CharacterData CharacterData;

    public float hp, maxHp, mp, maxMp, speed, jumpPower;
    public float[] stats;
    public List<float> coolDownTimes;
    

    public void SetCharacterData(CharacterData cd)
    {
        CharacterData = cd;
        
        hp = cd.characterInfo.hp;
        maxHp = hp;
        mp = cd.characterInfo.mp;
        maxMp = mp;

        speed = cd.characterInfo.speed;
        jumpPower = cd.characterInfo.jumpPower;

        stats = new float[cd.characterInfo.stats.Length];
        
        for (int i = 0; i < stats.Length; i++)
        {
            stats[i] = cd.characterInfo.stats[i];
        }
    }

    public void AddExp(float exp)
    {
        CharacterData.exp += exp;
        while (CharacterData.exp >= CharacterData.maxExp)
        {
            CharacterData.exp -= CharacterData.maxExp;
            CharacterData.LevelUp();
            CharacterData.maxExp *= 1.5f;
            CharacterData.maxExp = (int)CharacterData.maxExp;
        }
    }
}
