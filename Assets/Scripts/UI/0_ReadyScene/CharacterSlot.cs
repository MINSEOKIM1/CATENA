using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlot : MonoBehaviour
{
    public int currentSlot;
    public Image[] images;
    public Sprite nullSprite;

    public List<KeyValuePair<CharacterData, bool>> characterDatasWithActive;

    public Image[] buttonActive;

    public void SetCurrentSlot(int n)
    {
        currentSlot = n;
    }

    private void Start()
    {
        characterDatasWithActive = new List<KeyValuePair<CharacterData, bool>>();
        for (int i = 0; i < 9; i++)
        {
            CharacterData cd = new CharacterData(GameManager.Instance.DataManager.CharacterInfos.characterInfos[i]);
            if (i < 3) GameManager.Instance.DataManager.expeditionData[i] = cd;
            characterDatasWithActive.Add(new KeyValuePair<CharacterData, bool>(
               cd, i < 3 ? true : false));
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < 3; i++)
        {
            if (GameManager.Instance.DataManager.expeditionData[i] != null)
            {
                images[i].sprite = GameManager.Instance.DataManager.expeditionData[i].characterInfo.characterImage;
            }
            else
            {
                images[i].sprite = null;
            }
        }

        for (int i = 0; i < 9; i++)
        {
            if (i == 5 || i == 6) continue;
            buttonActive[i].gameObject.SetActive(characterDatasWithActive[i].Value);
        }
    }

    public void ChangeCharacter(int n)
    {
        if (characterDatasWithActive[n].Value == true) return;
        
        if (GameManager.Instance.DataManager.expeditionData[currentSlot] != null)
        {
            for (int i = 0; i < characterDatasWithActive.Count; i++)
            {
                if (characterDatasWithActive[i].Key == GameManager.Instance.DataManager.expeditionData[currentSlot])
                {
                    characterDatasWithActive[i] = new KeyValuePair<CharacterData, bool>(
                            GameManager.Instance.DataManager.expeditionData[currentSlot], false);
                }
            }
        }
        
        GameManager.Instance.DataManager.expeditionData[currentSlot] = characterDatasWithActive[n].Key;
        characterDatasWithActive[n] = new KeyValuePair<CharacterData, bool>(characterDatasWithActive[n].Key, true);
    }
}
