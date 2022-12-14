using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class CharacterSkills : MonoBehaviour
{
    private Animator _animator;
    private CharacterDataProcessor _characterDataProcessor;
    private CharacterBehavior _characterBehavior;
    private HitBoxCheck _hitBoxCheck;
    private Rigidbody2D _rigidbody;
    private ExpeditionManager _expeditionManager;

    public float knightDash;
    public float berserkerDash;
    public float lancerJump;
    
    public float arrowSpeed;
    public float bulletSpeed;
    public float shurikenSpeed;

    public GameObject[] effects;
    /*
     * 0 : Explosion0
     */

    private float chargingTime;

    public ThrowingInfos throwingInfos;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
        _characterBehavior = GetComponent<CharacterBehavior>();
        _characterDataProcessor = GetComponent<CharacterDataProcessor>();
        _hitBoxCheck = GetComponentInChildren<HitBoxCheck>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _expeditionManager = transform.parent.GetComponent<ExpeditionManager>();
    }

    private void FixedUpdate()
    {
        chargingTime += Time.deltaTime;
    }

    public void Skill(int num)
    {
        switch (num)
        {
            case 0 : // onehand melee
                StartCoroutine(Attack0());
                break;
            case 1 : // twohand melee
                StartCoroutine(Attack1());
                break;
            case 2 : // bow
                if (_characterBehavior.attackTime == 0)
                {
                    Debug.Log("!!");
                    StopCoroutine(Attack2_shot());
                    StartCoroutine(Attack2_ready());
                    break;
                }
                else
                {
                    Debug.Log("??");
                    StopCoroutine(Attack2_ready());
                    StartCoroutine(Attack2_shot());
                    break;
                }
                
            case 3 : // dagger
                StartCoroutine(Attack3());
                break;
            case 4 : // gun
                StartCoroutine(Attack4());
                break;
            case 5 : // assassin
                StartCoroutine(Attack5());
                break;
            case 6 : // knight skill 0
                StartCoroutine(KnightSkill0());
                break;
            case 7 : // knight skill 1
                StartCoroutine(KnightSkill1());
                break;
            case 8 : // knight skill 2
                StartCoroutine(KnightSkill2());
                break;
            case 9 : 
                StartCoroutine(BerserkerSkill0());
                break;
            case 10 : 
                StartCoroutine(BerserkerSkill1());
                break;
            case 11 : 
                StartCoroutine(BerserkerSkill2());
                break;
            case 12 : 
                StartCoroutine(LancerSkill0());
                break;
            case 13 : 
                StartCoroutine(LancerSkill1());
                break;
            case 14 : 
                StartCoroutine(LancerSkill2());
                break;
            case 15 : 
                StartCoroutine(ThiefSkill0());
                break;
            case 16 : 
                StartCoroutine(ThiefSkill1());
                break;
            case 17 : 
                StartCoroutine(ThiefSkill2());
                break;
            case 18 : 
                StartCoroutine(AssassinSkill0());
                break;
            case 19 : 
                StartCoroutine(AssassinSkill1());
                break;
            case 20 : 
                StartCoroutine(AssassinSkill2());
                break;
            case 21 : 
                StartCoroutine(RangerSkill0());
                break;
            case 22 : 
                StartCoroutine(RangerSkill1());
                break;
            case 23 : 
                StartCoroutine(RangerSkill2());
                break;
            case 24 : 
                StartCoroutine(GunslingerSkill0());
                break;
            case 25 : 
                StartCoroutine(GunslingerSkill1());
                break;
            case 26 : 
                StartCoroutine(GunslingerSkill2());
                break;
                

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void AttackBoundaryCheck(Vector2 offset, Vector2 boxSize, float damage, Vector2 airborne, float stunTime)
    {
        _hitBoxCheck.boxOffset = offset;
        _hitBoxCheck.boxSize = boxSize;
        _hitBoxCheck.damage = damage;
        _hitBoxCheck.airborne = airborne;
        _hitBoxCheck.stunTime = stunTime;
    }

    private Vector2 CalculateKnockback(int num)
    {
        return new Vector2(
            _characterDataProcessor.CharacterData.characterInfo.skills[num].airborne.x,
            _characterDataProcessor.CharacterData.characterInfo.skills[num].airborne.y);
    }

    private float CalculateSkillDamage(int num)
    {
        float damage = _characterDataProcessor.CharacterData.characterInfo.skills[num].defaultDamage;
        for (int i = 0; i < _characterDataProcessor.CharacterData.characterInfo.stats.Length; i++)
        {
            damage += _characterDataProcessor.stats[i] *
                      _characterDataProcessor.CharacterData.characterInfo.skills[num].statCoefficient[i];
        }

        return damage;
    }

    private void InstantiateThrowing(GameObject o, float damage, float speed, float stun, Vector2 airborne, Vector3 dir)
    {
        var arrow = Instantiate(o, transform.position + dir, Quaternion.identity);
        arrow.GetComponent<Throwing>().SetInfo(damage, stun, speed, airborne, dir);
    }
    private void InstantiateThrowing(GameObject o, float damage, float speed, float stun, Vector2 airborne, Vector3 dir, GameObject effect)
    {
        var arrow = Instantiate(o, transform.position + dir, Quaternion.identity);
        arrow.GetComponent<Throwing>().SetInfo(damage, stun, speed, airborne, dir, effect);
    }
    
    private void InstantiateThrowing(GameObject o, float damage, float speed, float stun, Vector2 airborne, Vector3 dir, Vector3 offset)
    {
        var arrow = Instantiate(o, transform.position + dir + offset, Quaternion.identity);
        arrow.GetComponent<Throwing>().SetInfo(damage, stun, speed, airborne, dir);
    }

    private void CooldownAndMp(int n)
    {
        _characterDataProcessor.mp -= _characterDataProcessor.CharacterData.characterInfo.skills[n].mp;
        _expeditionManager.CooldownTimes[_characterBehavior.characterNum, n - 1] =
            _characterDataProcessor.CharacterData.characterInfo.skills[n].coolDownTime;
    }

    public void CharacterDie()
    {
        var eff2 = Instantiate(effects[2], transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
        eff2.GetComponentInChildren<Explosion>().SetInfo(0, 0, Vector2.zero);
    }

    IEnumerator Attack0() // onehand normal attack
    {
        int attackNum = Random.Range(0, 3);

        _animator.SetInteger("weaponNum", 0);
        _animator.SetInteger("attackNum", attackNum);
        _animator.SetTrigger("attack");

        float damage = CalculateSkillDamage(0);
        AttackBoundaryCheck(new Vector2(2.1f, 0), new Vector2(3.7f, 2.7f),
            damage, 
            _characterDataProcessor.CharacterData.characterInfo.skills[0].airborne,
            _characterDataProcessor.CharacterData.characterInfo.skills[0].stunTime);

        yield return new WaitForFixedUpdate();
        
        _characterBehavior.SetAttackTime(_animator.GetCurrentAnimatorStateInfo(0).length);
    }
    
    IEnumerator Attack1() // twohand normal attack
    {
        int attackNum = Random.Range(0, 3);
        
        _animator.SetInteger("weaponNum", 1);
        _animator.SetInteger("attackNum", attackNum);
        _animator.SetTrigger("attack");
        
        float damage = CalculateSkillDamage(0);
        AttackBoundaryCheck(new Vector2(2.1f, 0), new Vector2(5.2f, 4f),
            damage, 
            _characterDataProcessor.CharacterData.characterInfo.skills[0].airborne,
            _characterDataProcessor.CharacterData.characterInfo.skills[0].stunTime);


        yield return new WaitForFixedUpdate();
        
        _characterBehavior.SetAttackTime(_animator.GetCurrentAnimatorStateInfo(0).length);
    }
    
    IEnumerator Attack2_ready() // bow normal attack
    {
        _animator.SetInteger("weaponNum", 2);
        _animator.SetInteger("attackNum", 0);
        _animator.SetTrigger("attack");

        chargingTime = 0;

        yield return new WaitForFixedUpdate();
        _characterBehavior.SetAttackTime(_animator.GetCurrentAnimatorStateInfo(0).length);
    }
    
    IEnumerator Attack2_shot() // bow normal attack
    {
        _animator.SetInteger("attackNum", 1);
        _animator.SetTrigger("attack");

        var arrow = Instantiate(throwingInfos.arrow, transform.position + Vector3.right * transform.localScale.x, Quaternion.identity);
        arrow.GetComponent<Throwing>().SetInfo((int)(CalculateSkillDamage(0) * (chargingTime + 0.2f)), 
            _characterDataProcessor.CharacterData.characterInfo.skills[0].stunTime * (chargingTime),
            arrowSpeed,
            _characterDataProcessor.CharacterData.characterInfo.skills[0].airborne * (chargingTime + 0.2f),
            transform.localScale.x * Vector2.right);

        yield return new WaitForFixedUpdate();
        _characterBehavior.SetAttackTime(_animator.GetCurrentAnimatorStateInfo(0).length);
    }
    
    IEnumerator Attack3() // dagger normal attack
    {
        int attackNum = Random.Range(0, 2);
        
        _animator.SetInteger("weaponNum", 3);
        _animator.SetInteger("attackNum", attackNum);
        _animator.SetTrigger("attack");
        
        float damage = CalculateSkillDamage(0);
        AttackBoundaryCheck(new Vector2(1.2f, 0), new Vector2(2.8f, 2.1f),
            damage, 
            _characterDataProcessor.CharacterData.characterInfo.skills[0].airborne,
            _characterDataProcessor.CharacterData.characterInfo.skills[0].stunTime);


        yield return new WaitForFixedUpdate();
        
        _characterBehavior.SetAttackTime(_animator.GetCurrentAnimatorStateInfo(0).length);
    }
    
    IEnumerator Attack4() // gun
    {
        if (_characterBehavior.characterState == 0)
        {
            _animator.SetInteger("weaponNum", 4);
            _animator.SetInteger("attackNum", 0);
            _animator.SetTrigger("attack");

            yield return new WaitForFixedUpdate();

            _characterBehavior.SetAttackTime(_animator.GetCurrentAnimatorStateInfo(0).length);

            for (int i = 0; i < 2; i++)
            {
                InstantiateThrowing(throwingInfos.bullet, CalculateSkillDamage(0) * 0.3f, bulletSpeed,
                    _characterDataProcessor.CharacterData.characterInfo.skills[0].stunTime * 0,
                    _characterDataProcessor.CharacterData.characterInfo.skills[0].airborne * 0,
                    Vector3.right * transform.localScale.x);

                yield return new WaitForSeconds(0.2f);
            }
        } else if (_characterBehavior.characterState == 1)
        {
            _animator.SetInteger("weaponNum", 4);
            _animator.SetInteger("attackNum", 1);
            _animator.SetTrigger("attack");

            yield return new WaitForFixedUpdate();
        
            _characterBehavior.SetAttackTime(_animator.GetCurrentAnimatorStateInfo(0).length);

            yield return new WaitForSeconds(27 / 60f);
            for (int i = 0; i < 5; i++)
            {
                InstantiateThrowing(throwingInfos.bullet, CalculateSkillDamage(0) * 0.5f, bulletSpeed,
                    _characterDataProcessor.CharacterData.characterInfo.skills[0].stunTime,
                    _characterDataProcessor.CharacterData.characterInfo.skills[0].airborne * 0.4f,
                    Vector3.right * transform.localScale.x + new Vector3(0, (-2 + i) * 0.05f, 0));
            }
        }
        else
        {
            _animator.SetInteger("weaponNum", 4);
            _animator.SetInteger("attackNum", 2);
            _animator.SetTrigger("attack");

            yield return new WaitForFixedUpdate();
        
            _characterBehavior.SetAttackTime(_animator.GetCurrentAnimatorStateInfo(0).length);
            
            yield return new WaitForSeconds(12 / 60f);
            
            for (int i = 0; i < 3; i++)
            {
                InstantiateThrowing(throwingInfos.bullet, CalculateSkillDamage(0) * 0.5f, bulletSpeed,
                    _characterDataProcessor.CharacterData.characterInfo.skills[0].stunTime * 0,
                    _characterDataProcessor.CharacterData.characterInfo.skills[0].airborne * 0,
                    Vector3.right * transform.localScale.x,
                    Vector3.down * (i / 5.0f));

                yield return new WaitForSeconds(0.1f);
            }
        }
    }
    
    IEnumerator Attack5() // assassin normal attack
    {
        int attackNum = Random.Range(0, 2);
        
        _animator.SetInteger("weaponNum", 5);
        _animator.SetInteger("attackNum", attackNum);
        _animator.SetTrigger("attack");

        yield return new WaitForFixedUpdate();
        _characterBehavior.SetAttackTime(_animator.GetCurrentAnimatorStateInfo(0).length);

        yield return new WaitForSeconds(14 / 60f);

        for (int i = 0; i < 3; i++)
        {
            InstantiateThrowing(throwingInfos.shuriken, CalculateSkillDamage(0) * 0.3f, shurikenSpeed
                , _characterDataProcessor.CharacterData.characterInfo.skills[0].stunTime,
                _characterDataProcessor.CharacterData.characterInfo.skills[0].airborne,
                Vector3.right * transform.localScale.x,Vector3.down*(i/5f));

            yield return new WaitForSeconds(0.1f);
        }
    }
    
    IEnumerator KnightSkill0()
    {
        _rigidbody.drag = 5;
        _rigidbody.AddForce(Vector2.right * (transform.localScale.x * knightDash), ForceMode2D.Impulse);
        _animator.SetInteger("characterNum", 0);
        _animator.SetInteger("skillNum", 0);
        _animator.SetTrigger("skill");
        
        CooldownAndMp(1);

        float damage = CalculateSkillDamage(1) * 0.5f;
        AttackBoundaryCheck(new Vector2(1.2f, 0), new Vector2(2.8f, 2.1f),
            damage, 
            CalculateKnockback(1),
            _characterDataProcessor.CharacterData.characterInfo.skills[1].stunTime);


        yield return new WaitForFixedUpdate();
        _characterBehavior.SetAttackTime(_animator.GetCurrentAnimatorStateInfo(0).length);
        _characterBehavior.invincibleTime = _animator.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(22 / 60f);
        _rigidbody.velocity = new Vector2(transform.localScale.x * 2, _rigidbody.velocity.y);
        _rigidbody.drag = 0;
    }
    
    IEnumerator KnightSkill1() 
    {
        _animator.SetInteger("characterNum", 0);
        _animator.SetInteger("skillNum", 1);
        _animator.SetTrigger("skill");

        float damage = CalculateSkillDamage(2);
        AttackBoundaryCheck(new Vector2(0.8f, 0), new Vector2(16f, 4f),
            damage, 
            CalculateKnockback(2),
            _characterDataProcessor.CharacterData.characterInfo.skills[2].stunTime);


        yield return new WaitForFixedUpdate();
        _characterBehavior.SetAttackTime(_animator.GetCurrentAnimatorStateInfo(0).length);
        _characterBehavior.invincibleTime = _animator.GetCurrentAnimatorStateInfo(0).length;
        CooldownAndMp(2);
    }
    
    IEnumerator KnightSkill2() 
    {
        _animator.SetInteger("characterNum", 0);
        _animator.SetInteger("skillNum", 2);
        _animator.SetTrigger("skill");

        yield return new WaitForFixedUpdate();
        _characterBehavior.SetAttackTime(_animator.GetCurrentAnimatorStateInfo(0).length);
        CooldownAndMp(3);
    }
    
    IEnumerator BerserkerSkill0()
    {
        if (_characterBehavior.characterState == 0)
        {
            _animator.SetInteger("characterNum", 1);
            _animator.SetInteger("skillNum", 0);
            _animator.SetTrigger("skill");
            
            float damage = CalculateSkillDamage(1);
            AttackBoundaryCheck(Vector2.zero, new Vector2(16f, 2f),
                damage, 
                _characterDataProcessor.CharacterData.characterInfo.skills[1].airborne,
                _characterDataProcessor.CharacterData.characterInfo.skills[1].stunTime);

            CooldownAndMp(1);
            
            yield return new WaitForFixedUpdate();
            _characterBehavior.SetAttackTime(_animator.GetCurrentAnimatorStateInfo(0).length);
        }
        else
        {
            _animator.SetInteger("characterNum", 1);
            _animator.SetInteger("skillNum", 1);
            _animator.SetTrigger("skill");

            yield return new WaitForFixedUpdate();
            _characterBehavior.SetAttackTime(_animator.GetCurrentAnimatorStateInfo(0).length);
            yield return new WaitForSeconds(0.2f);
            _characterBehavior.canMove = true;
            CooldownAndMp(1);
            
            float damage = CalculateSkillDamage(1) * 0.1f;
            AttackBoundaryCheck(Vector2.zero, new Vector2(16f, 2f),
                damage, 
                _characterDataProcessor.CharacterData.characterInfo.skills[1].airborne,
                _characterDataProcessor.CharacterData.characterInfo.skills[1].stunTime);
            
            yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length * 0.9f);
            _characterBehavior.canMove = false;
        }
    }
    
    IEnumerator BerserkerSkill1()
    {
        CooldownAndMp(2);
        if (_characterBehavior.characterState == 0)
        {
            _rigidbody.AddForce((Vector2.right * (transform.localScale.x * 2) + Vector2.up) * berserkerDash , ForceMode2D.Impulse);
            _animator.SetInteger("characterNum", 1);
            _animator.SetInteger("skillNum", 2);
            _animator.SetTrigger("skill");
            
            float damage = CalculateSkillDamage(2);
            AttackBoundaryCheck(new Vector2(0, 1.5f), new Vector2(8f, 6f),
                damage, 
                _characterDataProcessor.CharacterData.characterInfo.skills[2].airborne,
                _characterDataProcessor.CharacterData.characterInfo.skills[2].stunTime);

            yield return new WaitForFixedUpdate();
            _characterBehavior.SetAttackTime(_animator.GetCurrentAnimatorStateInfo(0).length);
            _characterBehavior.invincibleTime = _animator.GetCurrentAnimatorStateInfo(0).length;

            yield return new WaitForSeconds(17 / 60f);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, 1 << 6);
            if (hit)
            {
                transform.position = hit.point;
                _rigidbody.velocity = Vector2.zero;
            }
            else
            {
                _rigidbody.AddForce(Vector2.down * 50, ForceMode2D.Impulse);
            }
            _rigidbody.velocity = new Vector2(transform.localScale.x * 2, _rigidbody.velocity.y);
        }
        else
        {
            _rigidbody.AddForce((Vector2.right * (transform.localScale.x * 2) + Vector2.up) * berserkerDash , ForceMode2D.Impulse);
            _animator.SetInteger("characterNum", 1);
            _animator.SetInteger("skillNum", 3);
            _animator.SetTrigger("skill");

            float damage = CalculateSkillDamage(2) * 1.2f;
            AttackBoundaryCheck(new Vector2(2.3f, 1.5f), new Vector2(15f, 6f),
                damage, 
                _characterDataProcessor.CharacterData.characterInfo.skills[2].airborne,
                _characterDataProcessor.CharacterData.characterInfo.skills[2].stunTime);


            yield return new WaitForFixedUpdate();
            _characterBehavior.SetAttackTime(_animator.GetCurrentAnimatorStateInfo(0).length);
            _characterBehavior.invincibleTime = _animator.GetCurrentAnimatorStateInfo(0).length;
            
            yield return new WaitForSeconds(23 / 60f);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, 1 << 6);
            if (hit)
            {
                transform.position = hit.point;
                _rigidbody.velocity = Vector2.zero;
            }
            else
            {
                _rigidbody.AddForce(Vector2.down * 50, ForceMode2D.Impulse);
            }
            _rigidbody.velocity = new Vector2(transform.localScale.x * 2, _rigidbody.velocity.y);
        }
    }
    
    IEnumerator BerserkerSkill2()
    {
        CooldownAndMp(3);
        if (_characterBehavior.characterState == 0)
        {
            _animator.SetInteger("characterNum", 1);
            _animator.SetInteger("skillNum", 4);
            _animator.SetTrigger("skill");

            yield return new WaitForFixedUpdate();
            _characterBehavior.SetAttackTime(_animator.GetCurrentAnimatorStateInfo(0).length);

            yield return new WaitForSeconds(0.5f);
            _characterBehavior.characterState = 1;
        }
        else
        {
            _characterBehavior.characterState = 0;
        }
    }
    
    IEnumerator LancerSkill0()
    {
        CooldownAndMp(1);
        float damage = CalculateSkillDamage(1) * 0.1f;
        AttackBoundaryCheck(new Vector2(3.6f, 0), new Vector2(7.7f, 2.4f),
            damage, 
            CalculateKnockback(1),
            _characterDataProcessor.CharacterData.characterInfo.skills[1].stunTime);

        _animator.SetInteger("characterNum", 2);
        _animator.SetInteger("skillNum", 0);
        _animator.SetTrigger("skill");

        yield return new WaitForFixedUpdate();
        _characterBehavior.SetAttackTime(_animator.GetCurrentAnimatorStateInfo(0).length);
        _characterBehavior.invincibleTime = _animator.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(0.5f);
        _characterBehavior.canMove = true;

        yield return new WaitForSeconds(1f);
        _characterBehavior.canMove = false;
    }
    
    IEnumerator LancerSkill1()
    {
        var eff2 = Instantiate(effects[2], transform.position, quaternion.identity);
        eff2.GetComponentInChildren<Explosion>().SetInfo(0, 0, Vector2.zero);

        _characterBehavior.characterState = (_characterBehavior.characterState + 1) % 2;
        CooldownAndMp(2);

        yield return new WaitForFixedUpdate();
    }

    public IEnumerator LancerSkill1_counter()
    {
        _characterBehavior.attackTime = 0.3f;
        var eff = Instantiate(effects[1], 
            transform.position,
            Quaternion.identity);

        var eff2 = Instantiate(effects[2], transform.position, Quaternion.Euler(0, 0, Random.Range(0, 360)));
        eff2.GetComponentInChildren<Explosion>().SetInfo(0, 0, Vector2.zero);
        
        HitBoxCheck h = eff.GetComponentInChildren<HitBoxCheck>();
        Debug.Log(h.damage);
        h.airborne = _characterDataProcessor.CharacterData.characterInfo.skills[2].airborne;
        h.stunTime = _characterDataProcessor.CharacterData.characterInfo.skills[2].stunTime;
        h.damage = CalculateSkillDamage(2);
        
        eff.GetComponentInChildren<Animator>().SetInteger("attackNum", Random.Range(0, 3));

        eff.transform.localScale = transform.localScale;

        yield return new WaitForSeconds(0.5f);
        eff2 = Instantiate(effects[2], eff.transform.position, quaternion.identity);
        eff2.GetComponentInChildren<Explosion>().SetInfo(0, 0, Vector2.zero);
        Destroy(eff);
    }
    
    IEnumerator LancerSkill2()
    {
        CooldownAndMp(3);
        float damage = CalculateSkillDamage(3);
        AttackBoundaryCheck(new Vector2(-2.6f, 0), new Vector2(15f, 9f),
            damage, 
            CalculateKnockback(3),
            _characterDataProcessor.CharacterData.characterInfo.skills[3].stunTime);

        _rigidbody.AddForce(Vector2.up * lancerJump , ForceMode2D.Impulse);
        _animator.SetInteger("characterNum", 2);
        _animator.SetInteger("skillNum", 1);
        _animator.SetTrigger("skill");

        yield return new WaitForFixedUpdate();
        _characterBehavior.SetAttackTime(_animator.GetCurrentAnimatorStateInfo(0).length);
        _characterBehavior.invincibleTime = _animator.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(34 / 60f);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, 
            Vector2.down + Vector2.right * transform.localScale.x, Mathf.Infinity, 1 << 6);
        if (hit)
        {
            transform.position = hit.point;
            _rigidbody.velocity = Vector2.zero;
        }
        else
        {
            _rigidbody.AddForce((Vector2.down + Vector2.right * transform.localScale.x) * 50, ForceMode2D.Impulse);
        }
    }
    
    IEnumerator RangerSkill0()
    {
        CooldownAndMp(1);
        float damage = CalculateSkillDamage(1);

        _animator.SetInteger("characterNum", 3);
        _animator.SetInteger("skillNum", 0);
        _animator.SetTrigger("skill");

        yield return new WaitForFixedUpdate();
        _characterBehavior.SetAttackTime(_animator.GetCurrentAnimatorStateInfo(0).length);
        _characterBehavior.invincibleTime = _animator.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(22 / 60f);
        InstantiateThrowing(throwingInfos.arrow, damage, arrowSpeed, 
            _characterDataProcessor.CharacterData.characterInfo.skills[1].stunTime,
            _characterDataProcessor.CharacterData.characterInfo.skills[1].airborne,
            transform.localScale.x * Vector2.right);
    }
    
    IEnumerator RangerSkill1()
    {
        CooldownAndMp(2);
        _rigidbody.velocity = Vector2.zero;
        
        _rigidbody.AddForce(Vector2.up * 16 + Vector2.left * (6 * transform.localScale.x), ForceMode2D.Impulse);
        float damage = CalculateSkillDamage(2);

        _animator.SetInteger("characterNum", 3);
        _animator.SetInteger("skillNum", 1);
        _animator.SetTrigger("skill");

        yield return new WaitForFixedUpdate();
        _characterBehavior.SetAttackTime(_animator.GetCurrentAnimatorStateInfo(0).length);
        _characterBehavior.invincibleTime = _animator.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(11/ 40f);

        for (int i = 0; i < 5; i++)
        {
            InstantiateThrowing(throwingInfos.arrow, damage, arrowSpeed,
                _characterDataProcessor.CharacterData.characterInfo.skills[2].stunTime,
                _characterDataProcessor.CharacterData.characterInfo.skills[2].airborne,
                (i/3.0f)*transform.localScale.x * Vector2.right + Vector2.down);
            
            Debug.Log(i/3.0f);
        }
    }
    
    IEnumerator RangerSkill2()
    {
        CooldownAndMp(3);
        float damage = CalculateSkillDamage(2);

        _animator.SetInteger("characterNum", 3);
        _animator.SetInteger("skillNum", 2);
        _animator.SetTrigger("skill");

        yield return new WaitForFixedUpdate();
        _characterBehavior.SetAttackTime(_animator.GetCurrentAnimatorStateInfo(0).length);
        _characterBehavior.invincibleTime = _animator.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(38 / 60f);

        for (int i = 0; i < 8; i++)
        {
            InstantiateThrowing(throwingInfos.arrow, damage, arrowSpeed,
                _characterDataProcessor.CharacterData.characterInfo.skills[1].stunTime,
                _characterDataProcessor.CharacterData.characterInfo.skills[1].airborne,
                transform.localScale.x * Vector2.right,
                Vector3.down * (i/8.0f));
            yield return new WaitForSeconds(3 / 60f);
        }
    }
    
    IEnumerator GunslingerSkill0()
    {
        CooldownAndMp(1);
        float damage = CalculateSkillDamage(1);

        _animator.SetInteger("characterNum", 4);
        _animator.SetInteger("skillNum", 0);
        _animator.SetTrigger("skill");
        
        _animator.SetInteger("attackNum",  _characterBehavior.characterState);

        yield return new WaitForFixedUpdate();
        _characterBehavior.SetAttackTime(_animator.GetCurrentAnimatorStateInfo(0).length);

        _characterBehavior.characterState = (_characterBehavior.characterState + 1) % 3;
    }
    IEnumerator GunslingerSkill1()
    {
        CooldownAndMp(2);
        float damage = CalculateSkillDamage(2);

        _animator.SetInteger("characterNum", 4);
        _animator.SetInteger("skillNum", 1);
        _animator.SetTrigger("skill");

        yield return new WaitForFixedUpdate();
        _characterBehavior.SetAttackTime(_animator.GetCurrentAnimatorStateInfo(0).length);

        yield return new WaitForSeconds(22 / 60f);

        for (int i = 0; i < 3; i++)
        {
            InstantiateThrowing(throwingInfos.bomb, damage, 10,
                _characterDataProcessor.CharacterData.characterInfo.skills[2].stunTime,
                _characterDataProcessor.CharacterData.characterInfo.skills[2].airborne,
                Vector3.up + Vector3.right * (transform.localScale.x * ((i+4)/5.0f)), effects[0]);
        }
    }
    IEnumerator GunslingerSkill2()
    {
        CooldownAndMp(3);
        _animator.SetInteger("characterNum", 4);
        _animator.SetInteger("skillNum", 2);
        _animator.SetTrigger("skill");
        
        float damage = CalculateSkillDamage(3) * 0.4f;
        AttackBoundaryCheck(new Vector2(0, 0), new Vector2(13f, 6f),
            damage, 
            _characterDataProcessor.CharacterData.characterInfo.skills[3].airborne,
            _characterDataProcessor.CharacterData.characterInfo.skills[3].stunTime);

        yield return new WaitForFixedUpdate();
        _characterBehavior.SetAttackTime(_animator.GetCurrentAnimatorStateInfo(0).length);
        _characterBehavior.invincibleTime = _animator.GetCurrentAnimatorStateInfo(0).length;
        
        yield return new WaitForSeconds(0.2f);
        _characterBehavior.canMove = true;
        
        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length * 0.9f);
        _characterBehavior.canMove = false;
    }

    
    IEnumerator ThiefSkill0()
    {
        _animator.SetInteger("characterNum", 7);
        _animator.SetInteger("skillNum", 0);
        _animator.SetTrigger("skill");
        
        float damage = CalculateSkillDamage(1);
        AttackBoundaryCheck(new Vector2(-0.4f, 0), new Vector2(8f, 2f),
            damage, 
            _characterDataProcessor.CharacterData.characterInfo.skills[1].airborne,
            _characterDataProcessor.CharacterData.characterInfo.skills[1].stunTime);


        yield return new WaitForFixedUpdate();
        _characterBehavior.SetAttackTime(_animator.GetCurrentAnimatorStateInfo(0).length);


        float dir = transform.localScale.x;
        
        yield return new WaitForSeconds(18 / 60f);
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(
            (Vector2)transform.position + new Vector2(5 * dir, 0),
            new Vector2(8.4f, 5.1f), 0);
        
        int num = 0;
        int k = 0;
        
        float maxDis = 0;
        float dis;

        for (int i = 0; i<collider2Ds.Length; i++)
        {
            if (collider2Ds[i].tag == "Mob")
            {
                num++;
                dis = Vector3.Distance(collider2Ds[i].transform.position, transform.position);
                if (dis > maxDis)
                {
                    k = i;
                    maxDis = dis;
                }
            }
        }
        _rigidbody.velocity = Vector2.zero;
        if (num == 0)
        {
            transform.Translate(transform.localScale.x * 5, 0, 0);
        }
        else
        {
            transform.position = collider2Ds[k].gameObject.transform.position + new Vector3(dir, 0, 0);
            collider2Ds[k].gameObject.GetComponent<MonsterBehavior>().TakeHit(damage, 
                _characterDataProcessor.CharacterData.characterInfo.skills[1].airborne,
                _characterDataProcessor.CharacterData.characterInfo.skills[1].stunTime);
        }

        CooldownAndMp(1);
    }
    
    IEnumerator ThiefSkill1()
    {
        _animator.SetInteger("characterNum", 7);
        _animator.SetInteger("skillNum", 1);
        _animator.SetTrigger("skill");

        float damage = CalculateSkillDamage(2) * 0.2f;
        AttackBoundaryCheck(new Vector2(0.8f, 0), new Vector2(3f, 2f),
            damage, 
            _characterDataProcessor.CharacterData.characterInfo.skills[2].airborne,
            _characterDataProcessor.CharacterData.characterInfo.skills[2].stunTime);


        yield return new WaitForFixedUpdate();
        _characterBehavior.SetAttackTime(_animator.GetCurrentAnimatorStateInfo(0).length);
        CooldownAndMp(2);
    }
    
    IEnumerator ThiefSkill2()
    {
        float damage = CalculateSkillDamage(3);
        AttackBoundaryCheck(new Vector2(0, 0), new Vector2(7f, 3f),
            damage, 
            _characterDataProcessor.CharacterData.characterInfo.skills[3].airborne,
            _characterDataProcessor.CharacterData.characterInfo.skills[3].stunTime);

        
        int skillNum;
        yield return new WaitForFixedUpdate();
        float dir = transform.localScale.x;
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(
            (Vector2)transform.position,
            new Vector2(32f, 32f), 0);

        int kk = 0;
        for (int i = 0; i < collider2Ds.Length; i++)
        {
            MonsterBehavior mb = collider2Ds[i].GetComponent<MonsterBehavior>();
            if (collider2Ds[i].tag == "Mob" && !mb.isGrounded && mb.state == 3)
            {
                kk++;
                _rigidbody.velocity = Vector2.zero;
                skillNum = Random.Range(2, 5);
                _animator.SetInteger("characterNum", 7);
                _animator.SetInteger("skillNum", skillNum);
                _animator.SetTrigger("skill");
                transform.position = collider2Ds[i].gameObject.transform.position;
                int k = Random.Range(0, 2);
                if (k == 0)
                {
                    transform.localScale = new Vector3(dir * -1, transform.localScale.y, transform.localScale.z);
                }
                yield return new WaitForSeconds(1.0f / 10);
                _characterBehavior.SetAttackTime(1.0f / 10);
            }
        }
        
        if (kk > 0) CooldownAndMp(3);
    }
    
    IEnumerator AssassinSkill0()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(
            (Vector2)transform.position + new Vector2(transform.localScale.x * 6, 0), 
            new Vector2(13, 8), 0);
        
        int k = 1000;

        GameObject o = gameObject;

        foreach (var i in collider2Ds)
        {
            if (i.tag != "Mob") continue;

            if (Vector3.Distance(transform.position, i.transform.position) < k)
            {
                k = (int) Vector3.Distance(transform.position, i.transform.position);
                o = i.gameObject;
            }
        }

        if (o != gameObject)
        {
            _animator.SetInteger("characterNum", 8);
            _animator.SetInteger("skillNum", 0);
            _animator.SetTrigger("skill");

            yield return new WaitForFixedUpdate();
            _characterBehavior.SetAttackTime(_animator.GetCurrentAnimatorStateInfo(0).length);

            yield return new WaitForSeconds(15 / 60f);
            var sh = Instantiate(throwingInfos.livingShuriken, transform.position, Quaternion.identity);
            sh.GetComponent<LivingShuriken>().SetInfo(CalculateSkillDamage(1),
                _characterDataProcessor.CharacterData.characterInfo.skills[1].stunTime,
                _characterDataProcessor.CharacterData.characterInfo.skills[1].airborne,
                o);
            CooldownAndMp(1);
        }
    }
    
    IEnumerator AssassinSkill1()
    {
        _animator.SetInteger("characterNum", 8);
        _animator.SetInteger("skillNum", 1);
        _animator.SetTrigger("skill");

        yield return new WaitForFixedUpdate();
        _characterBehavior.SetAttackTime(_animator.GetCurrentAnimatorStateInfo(0).length);
        _characterBehavior.canMove = true;

        yield return new WaitForSeconds(6 / 60f);
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0);
        _rigidbody.AddForce(Vector2.up * 12, ForceMode2D.Impulse);

        for (int i = 0; i < 5; i++)
        {
            InstantiateThrowing(throwingInfos.shuriken, CalculateSkillDamage(2), shurikenSpeed,
                _characterDataProcessor.CharacterData.characterInfo.skills[2].stunTime,
                _characterDataProcessor.CharacterData.characterInfo.skills[2].airborne,
                Vector2.down + 0.5f * (Vector2.left + Vector2.right * (0.5f * i)), effects[0]);
        }
        CooldownAndMp(2);

        _characterBehavior.canMove = false;
    }
    
    IEnumerator AssassinSkill2()
    {
        _animator.SetInteger("characterNum", 8);
        _animator.SetInteger("skillNum", 2);
        _animator.SetTrigger("skill");

        yield return new WaitForFixedUpdate();
        _characterBehavior.SetAttackTime(_animator.GetCurrentAnimatorStateInfo(0).length);

        yield return new WaitForSeconds(0.3f);
        
        var eff = Instantiate(effects[3], 
            transform.position, Quaternion.identity);
        AssassinSkill2 a = eff.GetComponent<AssassinSkill2>();
        
        Debug.Log(a.damage);
        
        a.SetAttack(CalculateSkillDamage(3), shurikenSpeed, 
            _characterDataProcessor.CharacterData.characterInfo.skills[3].stunTime,
            _characterDataProcessor.CharacterData.characterInfo.skills[3].airborne,
            15);
        CooldownAndMp(3);
    }
}
