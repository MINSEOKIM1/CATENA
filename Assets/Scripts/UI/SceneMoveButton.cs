using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMoveButton : MonoBehaviour
{
    public string sceneName;

    public void GoScene()
    {
        GameManager.Instance.DataManager.inDungeon = false;
        SceneManager.LoadScene(sceneName);
    }
}
