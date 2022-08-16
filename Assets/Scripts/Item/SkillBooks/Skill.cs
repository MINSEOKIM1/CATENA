using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Skill", order = 3)]
public class Skill : ScriptableObject
{
    public string skillName;
    public string description;

    public Sprite skillImage;
    public float coolDownTime;
    
    public int skillNum;
    public float defaultDamage;
    public float[] statCoefficient;

    public Vector2 airborne;
    public float stunTime;
}
