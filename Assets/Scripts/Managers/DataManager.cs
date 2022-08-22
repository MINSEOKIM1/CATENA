using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DataManager : MonoBehaviour
{
    public CharacterInfos CharacterInfos;
    // public EquipmentInfos EquipmentInfos;

    public List<GameObject> characterPrefabs;
    public List<CharacterData> expeditionData;

    
    // dungeon to dungeon meta datas
    public bool inDungeon = false;
    public float[] hps, mps;

    public float[,] cooldownTimes;
    public float[] tagCooldown;

    public int currentCharacterNum;
}
