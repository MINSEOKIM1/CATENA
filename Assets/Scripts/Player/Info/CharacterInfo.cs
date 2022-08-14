using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CharcterInfo", order = 1)]
// 기본적인 스탯들... 아이템의 효과는 적용되지 않음.
public class CharacterInfo : ScriptableObject
{
    public String characterName;
    public Sprite characterImage;

    public float hp, mp, speed, jumpPower;
    public float[] stats; 
    
    /*
     * Stats index
     * 0 : AD
     * 1 : AP
     * 2 : DEF
     * 3 : CRT
     * 4 : CRTD
     * 5 : CDRD 
     */

    public List<Sprite> skillImages;
    public List<int> skillNums;
    public List<float> skillCooldowns;
}
