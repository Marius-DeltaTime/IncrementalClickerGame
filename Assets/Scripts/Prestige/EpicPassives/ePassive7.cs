using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ePassive7: Increase production of ALL production Buildings by a certain %.
public class ePassive7 : EpicPassive
{
    private EpicPassive _epicPassive;
    private float percentageAmount = 0.01f; // 1%

    private void Awake()
    {
        _epicPassive = GetComponent<EpicPassive>();
        EpicPassives.Add(Type, _epicPassive);
        description = "Increase production of all Buildings by " + percentageAmount;
    }
    private void AddToBoxCache()
    {
        BoxCache.cachedAllBuildingMultiplierAmount += percentageAmount;
    }
    public override void InitializePermanentStat()
    {
        base.InitializePermanentStat();

        AddToBoxCache();
    }
}