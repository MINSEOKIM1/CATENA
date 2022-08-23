using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterBehavior : MonoBehaviour
{
    private Vector2 _moving;
    
    public float attackTime;
    public float invincibleTime;
    public bool canMove =  false;
    
    public GameObject damageText;
    public GameObject canvas;
    
    public float damageTextOffset;

    private Rigidbody2D _rigidbody2D;
    
    private ExpeditionManager _expeditionManager;
    private CharacterDataProcessor _characterDataProcessor;
    private CharacterSkills _characterSkills;

    private bool hitAir;
    
    private Animator _animator;

    public int characterState;

    public int characterNum;
    
    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();

        _expeditionManager = transform.parent.GetComponent<ExpeditionManager>();
        _characterDataProcessor = GetComponent<CharacterDataProcessor>();
        _characterSkills = GetComponent<CharacterSkills>();
        
        canvas = GameObject.FindWithTag("Canvas");
    }

    private void GoDisable()
    {
        _moving = Vector2.zero;
        gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (_characterDataProcessor.hp == 0 && _characterDataProcessor.isDead == false)
        {
            _characterDataProcessor.isDead = true;
            _characterSkills.CharacterDie();
            gameObject.SetActive(false);
        }

        if (_characterDataProcessor.isDead) return;
        
        if (attackTime > 0)
        {
            attackTime -= Time.deltaTime;
        }
        else
        {
            attackTime = 0;
            if (_expeditionManager.currentPlayerNum != characterNum)
            {
                invincibleTime = 1f;
                Invoke("GoDisable", 0.2f);
            }
        }

        if (invincibleTime > 0)
        {
            invincibleTime -= Time.deltaTime;
        }
        else
        {
            invincibleTime = 0;
        }

        if (hitAir) attackTime = 0;

        if (_moving.x != 0 && !hitAir)
        {
            _animator.SetBool("isMoving", true);
            if ((_moving.x < 0 && transform.localScale.x > 0) || (_moving.x > 0 && transform.localScale.x < 0))
            {
                if (attackTime == 0)
                {
                    transform.localScale =
                        new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
            }

        }
        else
        {
            _animator.SetBool("isMoving", false);
        }

        if (_animator.GetBool("onAir"))
        {
            if (_rigidbody2D.velocity.y < 0) _animator.SetBool("isFalling", true);
            else _animator.SetBool("isFalling", false);
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1, 1 << 6);
            if (!hit) _animator.SetBool("onAir", true);
        }

        if ((attackTime == 0 || canMove) && !hitAir) 
            transform.Translate(new Vector3(_moving.x, 0, 0) * (_characterDataProcessor.speed * Time.deltaTime));

        // Berserker hp red if state == 1
        if (_characterDataProcessor.CharacterData.characterInfo.characterNum == 1 && characterState == 1) 
        {
            _characterDataProcessor.hp -= Time.fixedDeltaTime * 10;
            if (_characterDataProcessor.hp < _characterDataProcessor.maxHp * 0.1f) characterState = 0;
        }
        
        // Lancer mp red if state == 1
        if (_characterDataProcessor.CharacterData.characterInfo.characterNum == 2 && characterState == 1) 
        {
            _characterDataProcessor.mp -= Time.fixedDeltaTime * _characterDataProcessor.CharacterData.characterInfo.skills[2].mp;
            if (_characterDataProcessor.mp <= 0) characterState = 0;
        }
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        if (_expeditionManager.currentPlayerNum != characterNum) return;
        if (_expeditionManager.currentPlayer == gameObject)
        _moving = value.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext value)
    {
        if (_expeditionManager.currentPlayerNum != characterNum) return;
        if (attackTime == 0 && value.started && !_animator.GetBool("onAir"))
        {
            _rigidbody2D.AddForce(Vector2.up * _characterDataProcessor.jumpPower, ForceMode2D.Impulse);
            _animator.SetBool("isFalling", false);
        }
    }

    public void OnAttack(InputAction.CallbackContext value)
    {
        if (_expeditionManager.currentPlayerNum != characterNum || hitAir) return;
        if (attackTime == 0 && value.started)
        {
            _characterSkills.Skill(_characterDataProcessor.CharacterData.characterInfo.skills[0].skillNum);
        }

        if (_characterDataProcessor.CharacterData.characterInfo.skillNums[0] == 2)
        {
            if (attackTime > 0 && value.canceled && _animator.GetInteger("attackNum") == 0)
            {
                _characterSkills.Skill(2);
            }
        }
    }

    public void SetAttackTime(float time)
    {
        attackTime = time;
    }

    private bool SkillCoolAndMpCheck(int n)
    {
        if (_expeditionManager.CooldownTimes[characterNum, n - 1] == 0 && _characterDataProcessor.mp >=
            _characterDataProcessor.CharacterData.characterInfo.skills[n].mp)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public void OnSkill1(InputAction.CallbackContext value)
    {
        if (_expeditionManager.currentPlayerNum != characterNum|| hitAir) return;
        if (attackTime == 0 && value.started && SkillCoolAndMpCheck(1))
        {
            _characterSkills.Skill(_characterDataProcessor.CharacterData.characterInfo.skills[1].skillNum);
        }
    }
    
    public void OnSkill2(InputAction.CallbackContext value)
    {
        if (_expeditionManager.currentPlayerNum != characterNum|| hitAir) return;
        if (attackTime == 0 && value.started && SkillCoolAndMpCheck(2))
        {
            _characterSkills.Skill(_characterDataProcessor.CharacterData.characterInfo.skills[2].skillNum);
        }
    }
    
    public void OnSkill3(InputAction.CallbackContext value)
    {
        if (_expeditionManager.currentPlayerNum != characterNum|| hitAir) return;
        if (attackTime == 0 && value.started && SkillCoolAndMpCheck(3))
        {
            _characterSkills.Skill(_characterDataProcessor.CharacterData.characterInfo.skills[3].skillNum);
        }
    }

    public void OnAddSkill1(InputAction.CallbackContext value)
    {
        if (_expeditionManager.currentPlayerNum != characterNum|| hitAir) return;
        if (attackTime == 0 && value.started)
        {
            // _characterSkills.Skill(_characterDataProcessor.CharacterData.characterInfo.skillNums[3]);
        }
    }
    
    public void OnAddSkill2(InputAction.CallbackContext value)
    {
        if (_expeditionManager.currentPlayerNum != characterNum|| hitAir) return;
        if (attackTime == 0 && value.started)
        {
            // _characterSkills.Skill(_characterDataProcessor.CharacterData.characterInfo.skillNums[3]);
        }
    }

    public IEnumerator TakeHit(float damage, Vector2 knockback)
    {
        if (_characterDataProcessor.CharacterData.characterInfo.characterNum == 2 && characterState == 1 )
        {
            if (attackTime == 0)
            StartCoroutine(_characterSkills.LancerSkill1_counter());
        }
        else if (invincibleTime == 0)
        {
            _characterDataProcessor.hp -= damage;
            _animator.SetTrigger("hit");
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.AddForce(knockback, ForceMode2D.Impulse);
            attackTime = 1f;
            var text = Instantiate(damageText, canvas.transform);
            text.GetComponent<DamageText>().SetDamage(damage, transform.position + Vector3.up * damageTextOffset);
            text.transform.SetParent(canvas.transform);
            
            yield return new WaitForFixedUpdate();
            attackTime = _animator.GetCurrentAnimatorStateInfo(0).length;
            hitAir = true;
        }
    }
    
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground" || col.gameObject.tag == "Mob")
        {
            Debug.Log(col.contacts[0].normal.y);
            for (int i = 0; i < col.contacts.Length; i++)
            {
                if (col.contacts[i].normal.y > 0.5)
                {
                    _animator.SetBool("onAir", false);
                    hitAir = false;
                }
            }
        }
    }
    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground" || col.gameObject.tag == "Mob")
        {
            Debug.Log(col.contacts[0].normal.y);
            for (int i = 0; i < col.contacts.Length; i++)
            {
                if (col.contacts[i].normal.y > 0.5)
                {
                    _animator.SetBool("onAir", false);
                    hitAir = false;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Ground")
        {
            Debug.Log("ASDGSDG");
            _animator.SetBool("onAir", true);
        }
    }
}
