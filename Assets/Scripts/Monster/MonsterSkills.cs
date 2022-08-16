using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkills : MonoBehaviour
{
    public void Skill(int n)
    {
        switch (n)
        {
            case 1:
                break;
            default :
                throw new ArgumentOutOfRangeException();
        }
    }
}
