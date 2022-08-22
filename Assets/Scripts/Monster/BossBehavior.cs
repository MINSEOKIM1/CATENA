using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BossBehavior : MonsterBehavior
{
    //state 0-dealtime 1-teleport 2-attacking
    private void FixedUpdate()
    {
        if (_monsterDataProcessor.hp <= 0)
        {
            if (!_isDead)
            {
                _isDead = true;
                _animator.SetTrigger("dead");
                _boxcollider2D.isTrigger = true;
                _rigidbody2D.gravityScale = 0;
                Invoke("Delete", 5f);
            }
            return;
        }

        if (attackTime > 0 && _stun <= 0)
        {
            attackTime -= Time.deltaTime;
        }

        if (_stun > 0) 
        {
            _stun -= Time.deltaTime;
        }


        if (_stun > 0 || attackTime > 0)
        {
            return;
        }

        if (state < 1)
        {
            ((BossSkills) _monsterSkills).Skill(2);
        }

    }


    public override void TakeHit(float damage, Vector2 airborne, float stunTime)
    {
        Debug.Log("new TakeHit Working");
        
        if(state == 1) return;
        if (_isDead) return;
        _monsterDataProcessor.hp -= damage * (100 / (100 + _monsterDataProcessor.monsterInfo.stats[1]));

        if (stunTime > _stun) _stun = stunTime;

        var text = Instantiate(damageText, canvas.transform);
        text.GetComponent<DamageText>()
            .SetDamage(damage, transform.position + Vector3.up * damageTextOffset * Random.Range(1, 1.2f));
        text.transform.SetParent(canvas.transform);
    }
}
