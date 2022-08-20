using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MonsterBehavior : MonoBehaviour
{
   public float turnTime;

   public GameObject damageText;
   public GameObject canvas;

   public float damageTextOffset;
   
   public float curturnTime;
   public float attackTime;

   public float _stun;

   public Vector2 boxOffset, attackBoundary;

   public bool isGrounded;
   public int state;

   private bool _isDead;
   /*
    * 0 : idle
    * 1 : walk - left
    * 2 : walk - right
    * 3 : hit
    * 4 : attack
    */

   private MonsterDataProcessor _monsterDataProcessor;
   private MonsterSkills _monsterSkills;
   private Animator _animator;
   private Rigidbody2D _rigidbody2D;
   
   private void OnDrawGizmos()
   {
      Gizmos.color = Color.red;
      Gizmos.DrawWireCube((Vector2)transform.position + boxOffset, attackBoundary);
   }

   private void Start()
   {
      _monsterDataProcessor = GetComponent<MonsterDataProcessor>();
      _monsterSkills = GetComponent<MonsterSkills>();
      
      _rigidbody2D = GetComponent<Rigidbody2D>();
      _animator = GetComponentInChildren<Animator>();
   }

   private void FixedUpdate()
   {
      if (_monsterDataProcessor.hp <= 0)
      {
         if (!_isDead)
         {
            _isDead = true;
            _animator.SetTrigger("dead");
            GetComponent<BoxCollider2D>().isTrigger = true;
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
   
      curturnTime -= Time.deltaTime;

      if (_stun > 0 || attackTime > 0)
      {
         curturnTime = 0;
         return;
      }

      if (state == 3 && !isGrounded) return;

      if (state < 3)
      {
         Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(
            (Vector2)transform.position + boxOffset,
            attackBoundary, 0);

         foreach (var i in collider2Ds)
         {
            if (i.tag == "Player")
            {
               _monsterSkills.Skill(0);
               if (i.transform.position.x < transform.position.x)
               {
                  transform.localScale = new Vector3(-1, 1, 1);
               }
               else
               {
                  transform.localScale = new Vector3(1, 1, 1);
               }
               return;
            }
         }
      }


      if (curturnTime <= 0)
      {
         Debug.Log("!");
         curturnTime = turnTime;
         state = Random.Range(0, 3);
      }

      _animator.SetInteger("state", state);

      RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(transform.localScale.x, 0, 0),
         Vector2.down, 3, 1 << 6);
      Debug.DrawRay(transform.position + new Vector3(transform.localScale.x, 0, 0), Vector2.down * 3, Color.blue);
      
      if (!hit)
      {
         bool did = false;
         if (state == 1)
         {
            state = 2;
            did = true;
         }
         if (state == 2 && !did) state = 1;
      }

      if (attackTime > 0) return;
      if (state == 1)
      {
         transform.Translate(Vector3.left * (_monsterDataProcessor.monsterInfo.speed * Time.deltaTime));
         transform.localScale = new Vector3(-1, 1, 1);
      } 
      if (state == 2)
      {
         transform.Translate(Vector3.right * (_monsterDataProcessor.monsterInfo.speed * Time.deltaTime));
         transform.localScale = new Vector3(1, 1, 1);
      }
   }

   public void TakeHit(float damage, Vector2 airborne, float stunTime)
   {
      if (_isDead) return;
      _monsterDataProcessor.hp -= damage * (100 / (100 + _monsterDataProcessor.monsterInfo.stats[1]));
      _rigidbody2D.velocity = Vector2.zero;
      
      _rigidbody2D.AddForce(airborne, ForceMode2D.Impulse);
      
      if (stunTime > _stun) _stun = stunTime;
      curturnTime = 0;
      
      var text = Instantiate(damageText, canvas.transform);
      text.GetComponent<DamageText>()
         .SetDamage(damage, transform.position + Vector3.up * damageTextOffset * Random.Range(1, 1.2f));
      text.transform.SetParent(canvas.transform);

      StartCoroutine(HitAnimation());
   }

   IEnumerator HitAnimation()
   {
      _animator.SetTrigger("hit");
      yield return new WaitForFixedUpdate();
      attackTime = _animator.GetCurrentAnimatorStateInfo(0).length;
      state = 3;
      _animator.SetInteger("state", 3);
   }

   private void OnCollisionEnter2D(Collision2D col)
   {
      if (col.contacts[0].normal.y > 0.9f && col.gameObject.tag == "Ground")
      {
         isGrounded = true;
      }
   }
   
   private void OnCollisionExit2D(Collision2D col)
   {
      if (col.gameObject.tag == "Ground")
      {
         isGrounded = false;
      }
   }

   void Delete()
   {
      Destroy(gameObject);
   }
}
