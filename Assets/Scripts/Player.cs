using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject ragDollPrefab;
    public static Player Instance { get; private set; }
    private NavMeshAgent navAgent;
    private Animator animator;
    private Ray _ray;
    private RaycastHit _hit;

    private void Awake()
    {
        Instance = this;
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        navAgent.speed = moveSpeed;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out _hit, 100f))
            {
                navAgent.speed = 5;
                navAgent.SetDestination(_hit.point);
                navAgent.speed = moveSpeed;
            }
        }
        if (Mathf.Approximately(navAgent.velocity.magnitude, 0f))
        {
            CharacterAnimController.Walk(animator, false);
        }
        else
        {
            CharacterAnimController.Walk(animator, true);
            float forwardSpeed = navAgent.velocity.magnitude;
            CharacterAnimController.SpeedWalk(animator, forwardSpeed);
        }
    }

    private void OnDestroy()
    {
        Instantiate(ragDollPrefab, transform.position, transform.rotation);
    }
}
