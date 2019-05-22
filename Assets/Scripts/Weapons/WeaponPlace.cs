using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPlace : MonoBehaviour {

    [SerializeField] private Transform weaponPlaceTransform;
    [SerializeField] private float takingDistance;
    [SerializeField] private MeleeWeapon startedWeapon;
    [SerializeField] private MeleeWeapon defaultWeapon;
    [SerializeField] private Animator animator;


    private MeleeWeapon _currentWeapon;
    private Collider _currentWeaponCollider;
    private Damagable _damagable;

    private void Awake()
    {
        _damagable = GetComponent<Damagable>();
    }

    private void Start()
    {
        _damagable.DieEvent += OnUnitDead;
        if (startedWeapon)
            CreateWeapon(startedWeapon);
        else if (defaultWeapon)
            CreateWeapon(defaultWeapon);
    }

    private void SetWeapon(MeleeWeapon weapon)
    {
        Destroy(_currentWeapon.gameObject);
        _currentWeapon = weapon;
        _currentWeapon.transform.position = weaponPlaceTransform.position;
        _currentWeapon.transform.rotation = weaponPlaceTransform.rotation;
        _currentWeapon.transform.SetParent(weaponPlaceTransform);

        GameManager.Instance.FreeWeapons.Remove(weapon);
        CharacterAnimController.MeleeOneHandedWeapon(animator, true);
        _currentWeaponCollider = _currentWeapon.gameObject.GetComponent<Collider>();
    }

    private void Update()
    {
        if (_currentWeapon.WeaponType == defaultWeapon.WeaponType && GameManager.Instance.FreeWeapons.Count>0)
        {
            foreach (MeleeWeapon freeWeapon in GameManager.Instance.FreeWeapons)
                if (Vector3.Distance(freeWeapon.transform.position, transform.position) < takingDistance)
                {
                    SetWeapon(freeWeapon);
                    break;
                }
        }
    }

    private void OnUnitDead(Damagable damagable)
    {
        if (_currentWeapon.WeaponType != defaultWeapon.WeaponType)
        {
            _currentWeapon.transform.SetParent(null);
            GameManager.Instance.FreeWeapons.Add(_currentWeapon);
        }
    }

    private void CreateWeapon(MeleeWeapon weaponPrefab)
    {
        _currentWeapon = Instantiate(weaponPrefab, weaponPlaceTransform.position, weaponPlaceTransform.rotation);        
        _currentWeapon.transform.SetParent(weaponPlaceTransform); // по другому привязать не получается
        _currentWeaponCollider = _currentWeapon.gameObject.GetComponent<Collider>();
        if (_currentWeapon.WeaponType == defaultWeapon.WeaponType)
        {
            CharacterAnimController.MeleeOneHandedWeapon(animator, false);
        }
        else
        {
            CharacterAnimController.MeleeOneHandedWeapon(animator, true);
        }

    }

    public void Attack()
    {
        if(_currentWeapon.WeaponType!=defaultWeapon.WeaponType)
        {
            CharacterAnimController.AttackPalkoi(animator);
        }
        else
        {
            CharacterAnimController.AttackHand(animator);
        }
    }

    public void AttackStartEvent()
    {
        _currentWeapon.AttackCollider.enabled = true;
    }

    public void AttackStopEvent()
    {
        _currentWeapon.AttackCollider.enabled = false;
    }
}

