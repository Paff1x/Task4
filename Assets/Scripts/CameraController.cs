using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    private Transform _target;
    public static CameraController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (_target != null)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(_target.position.x, offset.y, _target.position.z + offset.z), moveSpeed);
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
        transform.position = new Vector3(_target.position.x, offset.y, _target.position.z + offset.z);
        transform.LookAt(_target);
    }
}
