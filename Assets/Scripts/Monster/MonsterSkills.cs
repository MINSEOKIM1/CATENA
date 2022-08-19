using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkills : MonoBehaviour
{
    private Animator _animator;
    private MonsterHitBoxCheck _monsterHitBoxCheck;
    private MonsterBehavior _monsterBehavior;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _monsterHitBoxCheck = GetComponentInChildren<MonsterHitBoxCheck>();
        _monsterBehavior = GetComponent<MonsterBehavior>();
    }
    
    private void AttackBoundaryCheck(Vector2 offset, Vector2 boxSize, float damage, Vector2 airborne)
    {
        _monsterHitBoxCheck.boxOffset = offset;
        _monsterHitBoxCheck.boxSize = boxSize;
        _monsterHitBoxCheck.damage = damage;
        _monsterHitBoxCheck.airborne = airborne;
    }

    public void Skill(int n)
    {
        switch (n)
        {
            case 0:
                StartCoroutine(GoblinAttack());
                break;
            default :
                throw new ArgumentOutOfRangeException();
        }
    }

    IEnumerator GoblinAttack()
    {
        _animator.SetTrigger("attack");
        _animator.SetInteger("state", 0);
        _monsterBehavior.state = 4;

        float damage = 10;

        AttackBoundaryCheck(new Vector2(2.1f, 0), new Vector2(3.7f, 2.7f),
            damage,
            new Vector2(3, 6));
        
        yield return new WaitForFixedUpdate();
        
        if (_monsterBehavior.state == 4)
        _monsterBehavior.attackTime = _animator.GetCurrentAnimatorStateInfo(0).length + 1;
    }
}
