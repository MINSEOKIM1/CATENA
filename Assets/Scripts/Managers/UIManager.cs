using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public CharacterInfoUI[] characterInfoUI;
    public CharacterDataProcessor[] characterDataProcessors;
    public CharacterDataProcessor currentCharacterData;

    public ExpeditionManager expeditionManager;

    public void SetCurrentCharacter(CharacterDataProcessor ctx)
    {
        currentCharacterData = ctx;
        
        int k = 0;
        for (int i = 0; i < characterDataProcessors.Length; i++)
        {
            if (characterDataProcessors[i] == ctx) k = i;
        }
        characterInfoUI[0].SetInfos(ctx, k, expeditionManager);

        k = 1;

        for (int i = 0; i < characterDataProcessors.Length; i++)
        {
            if (characterDataProcessors[i] == currentCharacterData) continue;
            characterInfoUI[k++].SetInfos(characterDataProcessors[i], i, expeditionManager);
        }
    }
}
