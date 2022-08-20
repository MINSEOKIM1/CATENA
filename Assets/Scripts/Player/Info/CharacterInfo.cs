using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CharcterInfo", order = 1)]
// 기본적인 스탯들... 아이템의 효과는 적용되지 않음.
public class CharacterInfo : ScriptableObject
{
    public int characterNum;
    /*
     * 0 : Knight
     * 1 : Berserker
     * 2 : Lancer
     * 3 : Ranger
     * 4 : Gunslinger
     * 5 : Wizard
     * 6 : Priest
     * 7 : Thief
     * 8 : Assassin
     */
    
    public SpriteLibraryAsset characterSLA;
    public String characterName;
    public Sprite characterImage;

    public float hp, mp, speed, jumpPower;
    public float[] stats;

    public float growthHp, growthMp, growthSpeed, growthJumpPower;
    public float[] growthStats;
    
    /*
     * Stats index
     * 0 : AD
     * 1 : AP
     * 2 : DEF
     * 3 : CRT
     * 4 : CRTD
     * 5 : CDRD 
     */
    public Skill[] skills;

    public List<Sprite> skillImages;
    public List<int> skillNums; 
    /*
     * skillNums[0] == 기본 공격
     * skillNums[1] == 스킬 1
     * skillNums[2] == 스킬 2
     * skillNums[3] == 스킬 3
     */
    public List<float> skillCooldowns;
}
