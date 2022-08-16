using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item
{
    public EquipmentInfo EquipmentInfo;
    
    public List<SpecialAbility> SpecialAbility;
    /*
     * 0 : 경험치 상승
     * 1 : 생명력 흡수
     * 2 : 기절 시키기 
     * 3 : 원수 갚기
     */

    public float[] additionalStats;
    public float[] p_additionalStats;

    public Equipment(EquipmentInfo ei)
    {
        EquipmentInfo = ei;
    }
}
