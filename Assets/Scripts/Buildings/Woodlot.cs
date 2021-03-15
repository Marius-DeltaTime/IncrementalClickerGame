﻿using System.Collections.Generic;
using UnityEngine;

public class Woodlot : Building
{
    private Building _building;

    private void Awake()
    {
        _building = GetComponent<Building>();
        Buildings.Add(_Type, _building);
        
    }
    private void Start()
    {       
        SetInitialValues();
        SetDescriptionText();         
        // DisplayConsole();
    }
    private void DisplayConsole()
    {
        foreach (KeyValuePair<BuildingType, Building> kvp in Buildings)
        {
            Debug.Log(string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value));
        }
    }
    private void Update()
    {
        UpdateResourceCosts();
    }
}