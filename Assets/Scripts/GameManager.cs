using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private Transform winPlaceTransform;
    [SerializeField] private float winDistance;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    internal List<AllyUnit> AllyUnits = new List<AllyUnit>();
    internal List<EnemyUnit> EnemyUnits = new List<EnemyUnit>();
    internal List<MeleeWeapon> FreeWeapons = new List<MeleeWeapon>();
    public Action<Transform> SetCommanderAction;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        EnemyUnits.AddRange(FindObjectsOfType<EnemyUnit>());
        AllyUnits.AddRange(FindObjectsOfType<AllyUnit>());
    }

    private void Update()
    {
        foreach (AllyUnit allyUnit in AllyUnits)
            if (Vector3.Distance(allyUnit.transform.position, winPlaceTransform.position) < winDistance)
            {
                winPanel.SetActive(true);
            }
    }

    public void Lose()
    {
        losePanel.SetActive(true);
    }
}

