using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossSkills : MonsterSkills
{

    [SerializeField] Animator smallfire;

    [SerializeField] GameObject fireball;
    [SerializeField] GameObject lightning;
    [SerializeField] GameObject explode;

    [SerializeField] List<Transform> teleportSpots;
    [SerializeField] List<Transform> lightningSpots;

    public float dealtime;
    public float telelporttime;
    public float attackdelaytime;
    public float chargetime;
    
    public float spelldelay;
    public Vector2 spelloffset;
    


    public new void Skill(int n)
    {
        if(n != 2){
            return;
        }
        else{
          StartCoroutine(WizardAttack());
        }
    }

    public IEnumerator WizardAttack(){
        _monsterBehavior.state = 2;
        _animator.SetTrigger("teleport");
        
        while(!_animator.GetCurrentAnimatorStateInfo(0).IsName("WizardTeleportEffect0")){
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length / _animator.GetCurrentAnimatorStateInfo(0).speed + telelporttime);
        
        int type1 = Random.Range(0,teleportSpots.Count);

        transform.position = teleportSpots[type1].position;
        
        _animator.SetTrigger("teleportEnd");
        
        while(!_animator.GetCurrentAnimatorStateInfo(0).IsName("WizardTeleportEffect0")){
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length / _animator.GetCurrentAnimatorStateInfo(0).speed + attackdelaytime);
        _monsterBehavior.state = 3;
        
        int type2 = Random.Range(0,3);

        _monsterBehavior.attackTime = _animator.GetCurrentAnimatorStateInfo(0).length + 100;
                
        switch(type2){
            //파이어볼 투사체
            case 0 :
                _animator.SetTrigger("attack");
                smallfire.SetTrigger("red");
                while(!_animator.GetCurrentAnimatorStateInfo(0).IsName("WizardAttack1")){
                    yield return new WaitForFixedUpdate();
                }
                yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length / _animator.GetCurrentAnimatorStateInfo(0).speed + chargetime);
                
                _animator.SetTrigger("shoot");
                smallfire.SetTrigger("stop");
                while(!_animator.GetCurrentAnimatorStateInfo(0).IsName("WizardAttack2")){
                    yield return new WaitForFixedUpdate();
                }   
                yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length / _animator.GetCurrentAnimatorStateInfo(0).speed - spelldelay);
                
                int fireballNum = 10;
                for(int i = 0 ; i < fireballNum ; i++){
                    Instantiate(fireball, transform.position + new Vector3(spelloffset.x * transform.localScale.x, spelloffset.y, 0), Quaternion.Euler(0, 0, i * 360 / fireballNum));
                }
                yield return new WaitForSeconds(spelldelay);
                
                break;
            //세로 번개
            case 1 :
                _animator.SetTrigger("attack");
                smallfire.SetTrigger("blue");
                while(!_animator.GetCurrentAnimatorStateInfo(0).IsName("WizardAttack1")){
                    yield return new WaitForFixedUpdate();
                }   
                yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length / _animator.GetCurrentAnimatorStateInfo(0).speed + chargetime);
                
                _animator.SetTrigger("shoot");
                smallfire.SetTrigger("stop");
                while(!_animator.GetCurrentAnimatorStateInfo(0).IsName("WizardAttack2")){
                    yield return new WaitForFixedUpdate();
                }   
                yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length / _animator.GetCurrentAnimatorStateInfo(0).speed);
                StartCoroutine(LightningInstance());
                break;
            //주변 폭발
            case 2 :
                _animator.SetTrigger("attack");
                smallfire.SetTrigger("purple");
                while(!_animator.GetCurrentAnimatorStateInfo(0).IsName("WizardAttack1")){
                    yield return new WaitForFixedUpdate();
                }   
                yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length / _animator.GetCurrentAnimatorStateInfo(0).speed + chargetime);
                
                _animator.SetTrigger("shoot");
                smallfire.SetTrigger("stop");
                while(!_animator.GetCurrentAnimatorStateInfo(0).IsName("WizardAttack2")){
                    yield return new WaitForFixedUpdate();
                }   
                yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length / _animator.GetCurrentAnimatorStateInfo(0).speed - spelldelay);
                
                Instantiate(explode, transform.position + (Vector3)spelloffset, Quaternion.identity);
                yield return new WaitForSeconds(spelldelay);
                break;

            default :
                break;
        }
        _monsterBehavior.state = 0;                
        _monsterBehavior.attackTime = dealtime;

    }

    IEnumerator LightningInstance(){
        int lightningNum = lightningSpots.Count;
        for(int i = 0 ; i < lightningNum ; i++){
            Instantiate(lightning, lightningSpots[i].position , Quaternion.identity);
            yield return new WaitForSeconds(0.1f);    
        }
    }

}
