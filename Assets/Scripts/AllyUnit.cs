using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class AllyUnit : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool isCommander = false;
    [SerializeField] private float findRadius;
    [SerializeField] private float attackRange;
    [SerializeField] private WeaponPlace weaponPlace;
    [SerializeField] private GameObject damagePrefab;
    [SerializeField] private GameObject ragDollPrefab;
    [SerializeField] private Renderer bodyRenderer;
    [SerializeField] private Material commanderBodyMaterial;
    [SerializeField] private Material normalBodyMaterial;

    private NavMeshAgent _navAgent;
    private Animator _animator;
    private Ray _ray;
    private RaycastHit _hit;
    private Transform _commanderTransform;
    private Damagable _damagable;
    public bool IsInTeam { get; private set; }


    private void Awake()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _navAgent.speed = moveSpeed;
    }
    
    private void Start()
    {
        _damagable = GetComponentInParent<Damagable>();
        _damagable.DieEvent += OnDead;
        _damagable.DamageEvent += OnDamage;

        if (isCommander)
        {
            foreach(AllyUnit allyUnit in GameManager.Instance.AllyUnits)
                GameManager.Instance.SetCommanderAction += allyUnit.SetCommander;
            GameManager.Instance.SetCommanderAction(transform);
            IsInTeam = true;
        }
    }
    private void Update()
    {
        if (!IsInTeam && _commanderTransform)
        {
            if (Vector3.Distance(_commanderTransform.position, transform.position) < findRadius)
                IsInTeam = true;
        }
        else
        {
            if (isCommander)
            {
                if (Input.GetMouseButton(0))
                {
                    _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(_ray, out _hit, 100f))
                    {
                        _navAgent.SetDestination(_hit.point);
                    }
                }
            }
            else
            {
                if (_commanderTransform)
                    _navAgent.SetDestination(_commanderTransform.position);
            }
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

        foreach (EnemyUnit enemyUnit in GameManager.Instance.EnemyUnits)
            if (Vector3.Distance(enemyUnit.transform.position, transform.position) < attackRange)
            {
                transform.LookAt(enemyUnit.transform.position);
                weaponPlace.Attack();
            }
    }

    private void SetCommander(Transform commanderTransform)
    {
        _commanderTransform = commanderTransform;
        if(_commanderTransform == transform) 
        {
            isCommander = true;
            bodyRenderer.material = commanderBodyMaterial;
            CameraController.Instance.SetTarget(_commanderTransform);
        }
    }
    private void OnDead(Damagable damagable)
    {       
        GameManager.Instance.AllyUnits.Remove(this);
        if (isCommander)
        {
            if (GameManager.Instance.AllyUnits.Count > 0)
            {
                bool IsTeamEmpty = true;
                foreach (AllyUnit allyUnit in GameManager.Instance.AllyUnits)
                {
                    if (allyUnit.IsInTeam)
                    {
                        GameManager.Instance.SetCommanderAction?.Invoke(allyUnit.transform);
                        IsTeamEmpty = false;
                        break;
                    }
                }
                if (IsTeamEmpty)
                    GameManager.Instance.Lose();
            }
            else
                GameManager.Instance.Lose();
        }
        Instantiate(ragDollPrefab, transform.position, transform.rotation);
        GameManager.Instance.SetCommanderAction -= SetCommander;
    }

    private void OnDamage(Damagable damagable)
    {
        Instantiate(damagePrefab, transform.position, damagePrefab.transform.rotation);
    }
}
