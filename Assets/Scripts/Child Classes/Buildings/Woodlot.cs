﻿using System.Collections.Generic;
using UnityEngine;

public class Woodlot : Building
{
    private Building _building;

    void Awake()
    {
        _building = GetComponent<Building>();
        Buildings.Add(Type, _building);
        SetInitialValues();
    }
    void Start()
    {   
        SetDescriptionText();
    }
}