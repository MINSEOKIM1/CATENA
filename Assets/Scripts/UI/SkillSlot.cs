using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    public int characterNum, skillNum;
    public CharacterDataProcessor characterDataProcessor;
    public ExpeditionManager expeditionManager;
    public Image cooldown, skillImage;

    public void SetInfos(int cn, int sn, ExpeditionManager em, CharacterDataProcessor cdp)
    {
        characterNum = cn;
        skillNum = sn;
        characterDataProcessor = cdp;
        expeditionManager = em;

        // skillImage.sprite = characterDataProcessor.CharacterData.characterInfo.skills[sn + 1].skillImage;
    }
    public void FixedUpdate()
    {
        cooldown.fillAmount = expeditionManager.CooldownTimes[characterNum, skillNum] /
            characterDataProcessor.CharacterData.characterInfo.skills[skillNum + 1].coolDownTime;
    }
}
