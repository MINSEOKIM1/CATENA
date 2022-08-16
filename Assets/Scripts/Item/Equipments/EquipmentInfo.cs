using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentInfo : ScriptableObject
{
    public Sprite equipmentImage;

    public string equipmentName;
    public string equipmentDescription;

    public float[] extraStats;
    public float[] p_extraStats;
}
