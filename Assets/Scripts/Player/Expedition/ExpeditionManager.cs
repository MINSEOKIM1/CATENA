using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.U2D.Animation;

public class ExpeditionManager : MonoBehaviour
{
    public static ExpeditionManager Instance;
    
    public int currentPlayerNum;

    public GameObject currentPlayer;
    public List<GameObject> characters;

    public CinemachineVirtualCamera cine;

    public List<CharacterDataProcessor> characterDataProcessors;

    public float extraAd, extraAp, extraDef, extraAvd;

    public float[,] CooldownTimes;
    public float[] tagCooldown;

    public GameOverScreen gameOverScreen;
    private bool _gameOver = false;

    public void Test()
    {
        SceneManager.LoadScene("Town2");
    }
    private void Start()
    {
        GameManager.Instance.UIManager.expeditionManager = this;
        GameManager.Instance.UIManager.characterDataProcessors = new CharacterDataProcessor[3];

        if (!GameManager.Instance.DataManager.inDungeon)
        {
            GameManager.Instance.DataManager.inDungeon = true;
            
            currentPlayerNum = 0;
            CooldownTimes = new float[5, 5];
            tagCooldown = new float[3];
            
            GameManager.Instance.DataManager.hps = new float[3];
            GameManager.Instance.DataManager.mps = new float[3];
            GameManager.Instance.DataManager.cooldownTimes = new float[3, 3];
            GameManager.Instance.DataManager.tagCooldown = new float[3];

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
                

                GameManager.Instance.UIManager.characterDataProcessors[i] = characterDataProcessors[i];
                
                for (int j = 0; j < 3; j++)
                {
                    CooldownTimes[i, j] = 0;
                }
            }
            
        }
        else
        {
            currentPlayerNum = GameManager.Instance.DataManager.currentCharacterNum;
            CooldownTimes = GameManager.Instance.DataManager.cooldownTimes;
            tagCooldown = GameManager.Instance.DataManager.tagCooldown;
            
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
                characterDataProcessors[i].SetCharacterData(GameManager.Instance.DataManager.expeditionData[i], 
                    GameManager.Instance.DataManager.hps[i],
                    GameManager.Instance.DataManager.mps[i]);
                // characterDataProcessors[i].UpdateData();

                GameManager.Instance.UIManager.characterDataProcessors[i] = characterDataProcessors[i];
            }
        }
        

        currentPlayer = characters[currentPlayerNum];
        currentPlayer.SetActive(true);
        GameManager.Instance.UIManager.SetCurrentCharacter(characterDataProcessors[currentPlayerNum]);

        cine.Follow = currentPlayer.transform;
    }

    private void FixedUpdate()
    {
        int deathCheck = 0;

        GameManager.Instance.DataManager.currentCharacterNum = currentPlayerNum;
        GameManager.Instance.DataManager.cooldownTimes = CooldownTimes;
        GameManager.Instance.DataManager.tagCooldown = tagCooldown;

        for (int i = 0; i < characterDataProcessors.Count; i++)
        {
            if (characterDataProcessors[i].isDead) deathCheck++;
        }

        if (deathCheck >= 3 && !_gameOver)
        {
            _gameOver = true;
            gameOverScreen.Appear();
        } 
        for (int i = 0; i < characters.Count; i++)
        {
            if (characterDataProcessors[i].hp > 0)
            {
                if (characterDataProcessors[i].hp < characterDataProcessors[i].maxHp)
                    characterDataProcessors[i].hp += Time.deltaTime;
                if (characterDataProcessors[i].mp < characterDataProcessors[i].maxMp)
                    characterDataProcessors[i].mp += Time.deltaTime;

                GameManager.Instance.DataManager.hps[i] = characterDataProcessors[i].hp;
                GameManager.Instance.DataManager.mps[i] = characterDataProcessors[i].mp;
            }

            for (int j = 0; j < 3; j++)
            {
                tagCooldown[j] = tagCooldown[j] > 0 ? tagCooldown[j] -= Time.deltaTime : 0;
                
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
            (int)value.ReadValue<float>() <= characters.Count && tagCooldown[(int)value.ReadValue<float>()-1] == 0
            && !characterDataProcessors[(int)value.ReadValue<float>()-1].isDead)
        {
            GameManager.Instance.UIManager.SetCurrentCharacter(characterDataProcessors[(int)value.ReadValue<float>() - 1]);
            Vector3 curPo = currentPlayer.transform.position;
            Vector2 velocity = characters[currentPlayerNum].GetComponent<Rigidbody2D>().velocity;

            currentPlayer = characters[(int)value.ReadValue<float>() - 1];
            currentPlayer.SetActive(true);
            currentPlayer.transform.position = curPo;
            
            tagCooldown[currentPlayerNum] = 3f;

            currentPlayerNum = (int)value.ReadValue<float>() - 1;

            characters[currentPlayerNum].GetComponent<Rigidbody2D>().velocity = velocity;

            cine.Follow = currentPlayer.transform;

            
        }
    }
}
