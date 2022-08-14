using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData
{
    public CharacterInfo characterInfo;
    public int level;
    // public List<Equipment> equipments;
    // public List<SkillBook> skillBooks;

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
