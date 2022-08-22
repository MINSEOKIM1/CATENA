using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoUI : MonoBehaviour
{
    public CharacterDataProcessor characterDataProcessor;
    public ExpeditionManager expeditionManager;

    public int characterNum;

    public Slider hpBar, mpBar;
    public Image characterImage;
    public Image tagCoolImage;

    public TMP_Text characterNumText;

    public SkillSlot[] skillSlots;

    public int k;

    public void SetInfos(CharacterDataProcessor cdp, int n, ExpeditionManager em)
    {
        characterDataProcessor = cdp;
        expeditionManager = em;

        characterNum = n;
        characterNumText.text = (characterNum+1).ToString();

        characterImage.sprite = cdp.CharacterData.characterInfo.characterImage;

        for (int i = 0; i < skillSlots.Length; i++)
        {
            Debug.Log("!!!!:" + i);
            skillSlots[i].SetInfos(n, i, expeditionManager, characterDataProcessor);
        }
    }

    private void Start()
    {
        GameObject.FindWithTag("GameManager").GetComponentInChildren<UIManager>().characterInfoUI[k] = this;
    }

    private void FixedUpdate()
    {
        if (characterDataProcessor == null) return;

        hpBar.value = (characterDataProcessor.hp / characterDataProcessor.maxHp);
        mpBar.value = (characterDataProcessor.mp / characterDataProcessor.maxMp);
        if (!characterDataProcessor.isDead)
            tagCoolImage.fillAmount = expeditionManager.tagCooldown[characterNum] / 3;
        else tagCoolImage.fillAmount = 1;
    }
}
