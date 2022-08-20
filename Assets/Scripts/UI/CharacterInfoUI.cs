using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoUI : MonoBehaviour
{
    public CharacterDataProcessor characterDataProcessor;

    public Slider hpBar, mpBar;
    public Image characterImage;

    public GameObject[] skillSlots;

    private void FixedUpdate()
    {
        if (characterDataProcessor != null)
        {
            hpBar.value = (characterDataProcessor.hp / characterDataProcessor.maxHp);
            mpBar.value = (characterDataProcessor.mp / characterDataProcessor.maxMp);
        }
    }
}
