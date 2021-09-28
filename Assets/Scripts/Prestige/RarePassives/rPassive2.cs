using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rPassive2 : RarePassive
{
    private RarePassive _rarePassive;

    private void Awake()
    {
        _rarePassive = GetComponent<RarePassive>();
        RarePassives.Add(Type, _rarePassive);
    }

    public override void ExecutePassive()
    {
        base.ExecutePassive();
    }
}