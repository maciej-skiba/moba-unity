using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Minion : MonoBehaviour
{
    protected Champion _champion;
    protected Animator _animator;
    protected NavMeshAgent _myAgent;
    protected GameObject _attackTarget;
    protected HealthAndStatus _healthAndStatus;
    protected DetectPlayer _playerDetecter;
    protected float _timeClicked = 0;
    protected float _attackAvailableTime = 0;
    protected float _minionDamage = 10.0f;
    protected float _minionRange = 1.5f;
    protected bool _isAttacking = false;
    protected bool _isSwinging = false;
    protected bool _isAgentMoving = false;
    protected string _guid;

    public delegate void AttackDelegate(GameObject gObject);
    public static event AttackDelegate AttackEvent;

    protected virtual void Awake()
    {
        _champion = FindObjectOfType<Champion>();
        _attackTarget = _champion.gameObject;
        _animator = GetComponent<Animator>();
        _myAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        AttackEvent += DealDamage;
        _playerDetecter = GetComponent<DetectPlayer>();
        _healthAndStatus = GetComponent<HealthAndStatus>();
        _guid = Guid.NewGuid().ToString();
    }
    protected void Start()
    {
        _animator.SetFloat("AttackSpeed", _champion.attackAnimationSpeed);
    }
    protected void Update()
    {
        if (_healthAndStatus.isStunned)
        {
            AgentStop();
            return;
        }
        if (_playerDetecter.followingPlayer)
        {
            //check if target is not Dead
            if (_champion == null)
            {
                AgentStop();
                _playerDetecter.followingPlayer = false;
            }
            else
            {
                //if target in attack range = attack, else = move to target
                Movement.LookAtTarget(gameObject, _attackTarget);
                var distanceToAttacker = Vector3.Distance(transform.position, _attackTarget.transform.position);
                if (distanceToAttacker <= _minionRange)
                {
                    _myAgent.ResetPath();
                    _isAgentMoving = false;
                    if (_attackAvailableTime <= Time.time)
                    {
                        Attack(_attackTarget);
                    }
                }
                else
                {
                    //Debug.Log("Distance to attacker = " + Vector3.Distance(transform.position, attackTarget.transform.position));
                    if (!_isAgentMoving)
                    {
                        _animator.SetBool("isAgentAttacking", false);
                        _timeClicked = Time.time;
                        _isAgentMoving = true;
                        _animator.SetBool("isAgentMoving", true);
                        _myAgent.SetDestination(_attackTarget.transform.position);
                    }
                }
            }
        }
        else
        {
            AgentStop();
        }

        //if agent has reached its destination - stay
        if (_timeClicked + 0.2f < Time.time)
        {
            if (!_myAgent.hasPath && _animator.GetBool("isAgentMoving"))
            {
                AgentStop();
            }
        }
    }
    protected void AgentStop()
    {
        _isAttacking = false;
        _isSwinging = false;
        _isAgentMoving = false;
        _animator.SetBool("isAgentAttacking", false);
        _animator.SetBool("isAgentMoving", false);
        _myAgent.ResetPath();
    }

    //Perform attack 
    protected void Attack(GameObject target)
    {
        _isAttacking = true;
        _animator.SetBool("isAgentMoving", false);
        _animator.SetBool("isAgentAttacking", true);
        if (!_isSwinging)
            StartCoroutine(CoSwing(target));
    }

    protected void DealDamage(GameObject target)
    {
        if (target != null)
            target.GetComponent<HealthAndStatus>().health -= _minionDamage;
    }

    //attack in the swing moment, not in the start of attack animation
    IEnumerator CoSwing(GameObject target)
    {
        _isSwinging = true;
        float swingTime = (_champion.hitTime / _champion.attackAnimationSpeed);
        yield return new WaitForSeconds(swingTime);
        if (_isAttacking)
        {
            AttackEvent(target);
            _attackAvailableTime = Time.time + _champion.attackSpeed - swingTime;
            _isSwinging = false;
        }
    }
}
