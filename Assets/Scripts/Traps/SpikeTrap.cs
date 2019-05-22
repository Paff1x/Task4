using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] Transform spikeTransform;
    //[SerializeField] Bullet bullet;
    [SerializeField] float attackSpeed;
    [SerializeField] float animationSpeed;

    private void Start()
    {
        StartCoroutine(AttackDelay());
    }

    private IEnumerator AttackDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackSpeed);
            Attack();
        }
    }
    private void Attack()
    {
        spikeTransform.DOLocalMoveY(0f, animationSpeed / 5);
        StartCoroutine(WaitUntilStop());
        
    }

    private IEnumerator WaitUntilStop()
    {
        yield return new WaitUntil(() => !DOTween.IsTweening(spikeTransform));
        //Instantiate(bullet.gameObject, spikeTransform);
        yield return new WaitForSeconds(animationSpeed);
        spikeTransform.DOLocalMoveY(-1.8f, animationSpeed);
    }
}
