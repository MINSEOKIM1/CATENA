using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private float _speed;
    private float _damage;
    private TMP_Text _text;
    private Vector3 _originalPos;

    private void LateUpdate()
    {
        transform.position = Camera.main.WorldToScreenPoint(_originalPos);
        _speed = Mathf.Lerp(_speed, 0, Time.deltaTime);
        _originalPos += Vector3.up * (_speed * Time.deltaTime);

        if (_text.alpha > 0)
        {
            _text.alpha -= 2 * Time.deltaTime;
        }
    }

    private void Delete()
    {
        Destroy(gameObject);
    }

    public void SetDamage(float damage, Vector3 pos)
    {
        _originalPos = pos;
        _damage = damage;
        _speed = 5f;

        _text = GetComponent<TMP_Text>();
        _text.text = "" + (int)damage;


        // 데미지 높을 수록 커지고 빨개짐
        // _text.color = new Color(1, 1 - Mathf.Clamp(damage / 100, 0, 1), 1 - Mathf.Clamp(damage / 100, 0, 1));
        // _text.fontSize *= Mathf.Clamp(damage / 70, 0.7f, 1.2f);

        Invoke("Delete", 10f);
    }
}

