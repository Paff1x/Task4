using System;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private HealthBar healthBar;
    public UnitType unitType;


    public float Health { get; private set; }
    public float HealthMax { get { return maxHealth; } }

    public Action<Damagable> DamageEvent;
    public Action<Damagable> DieEvent;

    private void Awake()
    {
        Health = maxHealth;

    }

    private void OnTriggerEnter(Collider other)
    {
        var damageProvider = other.GetComponentInParent<DamageProvider>();
        var damagable = other.GetComponentInParent<Damagable>();
        if (!damagable || unitType != damagable.unitType )
        {
            if (damageProvider != null)
            {
                Health -= damageProvider.Damage;
                EnableHealthBar();
                DamageEvent?.Invoke(this);


                if (Health <= 0)
                {
                    DieEvent?.Invoke(this);

                    Destroy(gameObject);
                }
                if (damageProvider.DestroyAfterCollide)
                    Destroy(damageProvider.gameObject);
            }
            if(damagable)
            {
                var meleeWeapon = other.GetComponent<MeleeWeapon>();
                meleeWeapon.AttackCollider.enabled = false;
            }
        }
    }

    private void EnableHealthBar()
    {
        if (healthBar)
        {
            healthBar.gameObject.SetActive(true);
            healthBar = null;
        }
    }

    [Serializable]
    public enum UnitType { AllyUnit, EnemyUnit }
}
