using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour {


    [SerializeField] private Collider attackCollider;
    public WeaponType WeaponType;

    public Collider AttackCollider { get { return attackCollider; } }

    private void Start()
    {
        if(!transform.parent && WeaponType != WeaponType.Fist)
            GameManager.Instance.FreeWeapons.Add(this);
    }


}
[System.Serializable]
public enum WeaponType { Fist, Bat }