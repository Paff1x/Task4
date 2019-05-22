using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Door : MonoBehaviour
{
    [SerializeField] float openingAngle;
    [SerializeField] float timeBeforeOpening;
    [SerializeField] float animationTime;
    [SerializeField] Collider openingCollider;
    void Start()
    {
        if(!openingCollider)
            StartCoroutine(OpenDelay());
    }

    private IEnumerator OpenDelay()
    {
        yield return new WaitForSeconds(timeBeforeOpening);
        transform.DORotate(new Vector3(transform.rotation.eulerAngles.x, openingAngle, transform.rotation.y), animationTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        transform.DORotate(new Vector3(transform.rotation.eulerAngles.x, openingAngle, transform.rotation.y), animationTime);
    }

}
