using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{
    public TMP_Text[] texts;
    public Image panel;
    
    public void Appear()
    {
        gameObject.SetActive(true);

        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        for (int i = 0; i < 100; i++)
        {
            for (int j = 0; j < texts.Length; j++)
            {
                texts[j].alpha = i / 100.0f;
            }

            panel.color = new Color(0, 0, 0, i / 100.0f);

            yield return new WaitForSeconds(0.03f);
        }
    }
}
