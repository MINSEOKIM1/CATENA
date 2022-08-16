using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MonsterBehavior : MonoBehaviour
{
   public float turnTime;
   
   public float _curturnTime;
   public float _attackTime;

   public float _stun;

   public Vector2 boxOffset, attackBoundary;

   public bool isGrounded;
   private int _state;
   /*
    * 0 : idle
    * 1 : walk - left
    * 2 : walk - right
    * 3 : hit
    */

   private MonsterDataProcessor _monsterDataProcessor;
   private MonsterHitBoxCheck _monsterHitBoxCheck;
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
      _monsterHitBoxCheck = GetComponentInChildren<MonsterHitBoxCheck>();
      _rigidbody2D = GetComponent<Rigidbody2D>();
      _animator = GetComponentInChildren<Animator>();
   }

   private void FixedUpdate()
   {
      if (_monsterDataProcessor.hp <= 0) return;
      if (_attackTime > 0)
      {
         _attackTime -= Time.deltaTime;
      }

      if (_stun > 0) 
      {
         _stun -= Time.deltaTime;
      }
   
      _curturnTime -= Time.deltaTime;

      if (_stun > 0 || _attackTime > 0)
      {
         _curturnTime = 0;
         return;
      }

      if (_state == 3 && !isGrounded) return;

      if (_curturnTime <= 0)
      {
         Debug.Log("!");
         _curturnTime = turnTime;
         _state = Random.Range(0, 3);
      }

      _animator.SetInteger("state", _state);

      RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(transform.localScale.x, 0, 0),
         Vector2.down, 3, 1 << 6);
      Debug.DrawRay(transform.position + new Vector3(transform.localScale.x, 0, 0), Vector2.down * 3, Color.blue);
      
      if (!hit)
      {
         bool did = false;
         if (_state == 1)
         {
            _state = 2;
            did = true;
         }
         if (_state == 2 && !did) _state = 1;
      }

      if (_attackTime > 0) return;
      if (_state == 1)
      {
         transform.Translate(Vector3.left * (_monsterDataProcessor.monsterInfo.speed * Time.deltaTime));
         transform.localScale = new Vector3(-1, 1, 1);
      } 
      if (_state == 2)
      {
         transform.Translate(Vector3.right * (_monsterDataProcessor.monsterInfo.speed * Time.deltaTime));
         transform.localScale = new Vector3(1, 1, 1);
      }
   }

   public void TakeHit(float damage, Vector2 airborne, float stunTime)
   {
      _monsterDataProcessor.hp -= damage * (100 / (100 + _monsterDataProcessor.monsterInfo.stats[1]));
      _rigidbody2D.velocity = Vector2.zero;
      _rigidbody2D.AddForce(airborne, ForceMode2D.Impulse);
      
      _stun = stunTime;
      _curturnTime = 0;

      StartCoroutine(HitAnimation());
   }

   IEnumerator HitAnimation()
   {
      _animator.SetTrigger("hit");
      yield return new WaitForFixedUpdate();
      _attackTime = _animator.GetCurrentAnimatorStateInfo(0).length;
      _state = 3;
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
}
