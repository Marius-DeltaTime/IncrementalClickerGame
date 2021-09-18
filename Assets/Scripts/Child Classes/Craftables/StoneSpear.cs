using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneSpear : Craftable
{
    private Craftable _craftable;

    void Awake()
    {
        _craftable = GetComponent<Craftable>();
        Craftables.Add(Type, _craftable);
        SetInitialValues();
    }

    void Start()
    {
        SetDescriptionText("Increases your hunting efficiency.");
    }
}
