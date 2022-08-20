using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D.Animation;

public class ExpeditionManager : MonoBehaviour
{
    public int currentPlayerNum;

    public GameObject currentPlayer;
    public List<GameObject> characters;

    public CinemachineVirtualCamera cine;

    public List<CharacterDataProcessor> characterDataProcessors;

    public float extraAd, extraAp, extraDef, extraAvd;

    public float[,] CooldownTimes;
    private void Start()
    {
        currentPlayerNum = 0;
        CooldownTimes = new float[5, 5];
        for (int i = 0; i < GameManager.Instance.DataManager.expeditionData.Count; i++)
        {
            var character = Instantiate(GameManager.Instance.DataManager.characterPrefabs[i], 
                transform.position, Quaternion.identity);
            
            characters.Add(character);
            characters[i].GetComponentInChildren<SpriteLibrary>().spriteLibraryAsset =
                GameManager.Instance.DataManager.expeditionData[i].characterInfo.characterSLA;
            characters[i].transform.SetParent(gameObject.transform);
            characters[i].SetActive(false);
            characterDataProcessors.Add(characters[i].GetComponent<CharacterDataProcessor>());
            characters[i].GetComponent<CharacterBehavior>().characterNum = i;
            characterDataProcessors[i].SetCharacterData(GameManager.Instance.DataManager.expeditionData[i]);
            // characterDataProcessors[i].UpdateData();

            for (int j = 0; j < 3; j++)
            {
                CooldownTimes[i, j] = 0;
            }
        }

        currentPlayer = characters[0];
        currentPlayer.SetActive(true);

        cine.Follow = currentPlayer.transform;
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (CooldownTimes[i, j] > 0)
                {
                    CooldownTimes[i, j] -= Time.deltaTime;
                    if (CooldownTimes[i, j] <= 0)
                    {
                        CooldownTimes[i, j] = 0;
                    }
                }
            }
        }

        extraAd = 0; extraAp = 0; extraDef = 0; extraAvd = 0;
    }

    public void ChangeCharacter(InputAction.CallbackContext value)
    {
        if (value.started && (int)value.ReadValue<float>() != currentPlayerNum + 1 &&
            (int)value.ReadValue<float>() <= characters.Count)
        {
            Vector3 curPo = currentPlayer.transform.position;
            Vector2 velocity = characters[currentPlayerNum].GetComponent<Rigidbody2D>().velocity;

            currentPlayer = characters[(int)value.ReadValue<float>() - 1];
            currentPlayer.SetActive(true);
            currentPlayer.transform.position = curPo;

            currentPlayerNum = (int)value.ReadValue<float>() - 1;

            characters[currentPlayerNum].GetComponent<Rigidbody2D>().velocity = velocity;

            cine.Follow = currentPlayer.transform;
        }
    }
}
