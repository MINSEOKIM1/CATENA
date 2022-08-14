using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDataProcessor : MonoBehaviour
{
    private CharacterData _characterData;

    public float hp, maxHp, mp, maxMp, speed, jumpPower;
    public float[] stats;
    public List<float> coolDownTimes;

    public void SetCharacterData(CharacterData cd)
    {
        _characterData = cd;
    }
}
