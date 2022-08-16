using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CharcterInfos", order = 2)]
public class CharacterInfos : ScriptableObject
{
    public GameObject[] characterPrefabs;
    public CharacterInfo[] characterInfos;
}
