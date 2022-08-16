using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharacterData
{
    public CharacterInfo characterInfo;
    public int level;

    public float exp;
    public float maxExp;
    
    // public List<Equipment> equipments;
    // public List<SkillBook> skillBooks; 2개가 최대

    public CharacterData(CharacterInfo ci)
    {
        characterInfo = ci;
        // equipments = new List<Equipment>;
        // skillBooks = new List<SkillBook>;
    }

    public void LevelUp()
    {
        level++;
    }
}
