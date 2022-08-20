using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class CharacterState : MonoBehaviour
{
    private SpriteResolver _spriteResolver;

    [SerializeField] private CharacterDataProcessor _characterDataProcessor;
    [SerializeField] private CharacterBehavior _characterBehavior;

    private void Start()
    {
        _spriteResolver = GetComponent<SpriteResolver>();
    }

    private void FixedUpdate()
    {
        if (_characterDataProcessor.CharacterData.characterInfo.characterNum == 1) // berserker
        {
            _spriteResolver.SetCategoryAndLabel(_spriteResolver.GetCategory(), _characterBehavior.characterState.ToString());
        }
        
        if (_characterDataProcessor.CharacterData.characterInfo.characterNum == 4) // gunslinger
        {
            _spriteResolver.SetCategoryAndLabel(_spriteResolver.GetCategory(), _characterBehavior.characterState.ToString());
        }
    }
}
