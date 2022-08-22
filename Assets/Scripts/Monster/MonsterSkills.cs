using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkills : MonoBehaviour
{
    protected Animator _animator;
    protected MonsterHitBoxCheck _monsterHitBoxCheck;
    protected MonsterBehavior _monsterBehavior;

    public Vector2 offset, boxSize;
    public float damage;
    public Vector2 airborne;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube((Vector2)transform.position + transform.localScale.x * offset, boxSize);
    }

    protected void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _monsterHitBoxCheck = GetComponentInChildren<MonsterHitBoxCheck>();
        _monsterBehavior = GetComponent<MonsterBehavior>();
    }
    
    protected void AttackBoundaryCheck(Vector2 _offset, Vector2 _boxSize, float _damage, Vector2 _airborne)
    {
        _monsterHitBoxCheck.boxOffset = _offset;
        _monsterHitBoxCheck.boxSize = _boxSize;
        _monsterHitBoxCheck.damage = _damage;
        _monsterHitBoxCheck.airborne = _airborne;
    }

    public void Skill(int n)
    {
        switch (n)
        {
            case 0:
                StartCoroutine(GoblinAttack());
                break;
            case 1:
                StartCoroutine(BoarAttack());
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

        AttackBoundaryCheck(offset, boxSize,
            damage,
            airborne);
        
        yield return new WaitForFixedUpdate();
        
        if (_monsterBehavior.state == 4)
        _monsterBehavior.attackTime = _animator.GetCurrentAnimatorStateInfo(0).length + 1;
    }

    IEnumerator BoarAttack()
    {
        _animator.SetTrigger("attackReady");
        _animator.SetInteger("state", 0);
        _monsterBehavior.state = 4;

        float damage = 10;

        AttackBoundaryCheck(offset, boxSize, damage, airborne);
        
        yield return new WaitForFixedUpdate();
        
        if (_monsterBehavior.state == 4)
        _monsterBehavior.attackTime = _animator.GetCurrentAnimatorStateInfo(0).length + 2;

        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length / _animator.GetCurrentAnimatorStateInfo(0).speed + 0.5f);

        if (_monsterBehavior.state == 4)
            _animator.SetTrigger("attack");
        
        yield return new WaitForFixedUpdate();

        if (_monsterBehavior.state == 4)
        _monsterBehavior.attackTime = _animator.GetCurrentAnimatorStateInfo(0).length + 1;
        else yield break;

        bool attacked = true;
        while(_monsterBehavior.attackTime > 0 && _monsterBehavior.state == 4){
            if(_monsterHitBoxCheck.AttackBoundaryCheck(attacked)){
                attacked = false;
            }
            _monsterBehavior.Rush();
            yield return new WaitForFixedUpdate();
        }
        
        if(_monsterBehavior.attackTime < 0)
            _animator.SetTrigger("attackStop");
    }
}