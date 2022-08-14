using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterBehavior : MonoBehaviour
{
    private Vector2 _moving;
    
    private float _attackTime;

    private Rigidbody2D _rigidbody2D;
    /*
    private ExpeditionManaging _expeditionManaging;
    */
    private CharacterDataProcessor _characterDataProcessor;
    private CharacterSkills _characterSkills;
    
    private Animator _animator;
    
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        transform.Translate(new Vector3(_moving.x, 0, 0) * (_characterDataProcessor.speed * Time.deltaTime));
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        _moving = value.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        if (value.started)
        {
            _rigidbody2D.AddForce(Vector2.up * _characterDataProcessor.jumpPower, ForceMode2D.Impulse);
        }
    }
}
