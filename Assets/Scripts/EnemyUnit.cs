using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]

public class EnemyUnit : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float findRadius;
    [SerializeField] private float attackRange;
    [SerializeField] private WeaponPlace weaponPlace;
    [SerializeField] private GameObject damagePrefab;
    [SerializeField] private GameObject ragDollPrefab;

    private NavMeshAgent _navAgent;
    private Animator _animator;
    private Transform _target;
    private Damagable _damagable;

    private void Awake()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _damagable = GetComponent<Damagable>();
        _navAgent.speed = moveSpeed;
    }

    private void Start()
    {
        _damagable.DamageEvent += OnDamage;
        _damagable.DieEvent += OnDead;
    }

    private void Update()
    {
        if (!_target)
            FindTarget();
        else
        {
            Walk();
        }
        foreach (AllyUnit allyUnit in GameManager.Instance.AllyUnits)
            if (Vector3.Distance(allyUnit.transform.position, transform.position) < attackRange)
            {
                transform.LookAt(allyUnit.transform.position);
                weaponPlace.Attack();
            }
    }

    private void FindTarget()
    {
        foreach (AllyUnit allyUnit in GameManager.Instance.AllyUnits)
            if(allyUnit.IsInTeam)
                if (Vector3.Distance(allyUnit.transform.position, transform.position) < findRadius)
                    _target = allyUnit.transform;
    }

    private void Walk()
    {
        _navAgent.SetDestination(_target.position);
        if (Mathf.Approximately(_navAgent.velocity.magnitude, 0f))
        {
            CharacterAnimController.Walk(_animator, false);
        }
        else
        {
            CharacterAnimController.Walk(_animator, true);
            float forwardSpeed = _navAgent.velocity.magnitude;
            CharacterAnimController.SpeedWalk(_animator, forwardSpeed);
        }
    }

    private void OnDead(Damagable damagable)
    {
        Instantiate(ragDollPrefab, transform.position, transform.rotation);
        GameManager.Instance.EnemyUnits.Remove(this);
    }

    private void OnDamage(Damagable damagable)
    {
        Instantiate(damagePrefab, transform.position, damagePrefab.transform.rotation);
    }
}
