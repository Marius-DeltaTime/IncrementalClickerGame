using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ePassive9: Decrease cost of ALL Researchables by a certain %.
public class ePassive9 : EpicPassive
{
    private EpicPassive _epicPassive;
    private float percentageAmount = 0.01f; //1%

    private void Awake()
    {
        _epicPassive = GetComponent<EpicPassive>();
        EpicPassives.Add(Type, _epicPassive);
        description = "Decrease the cost of all Researchables by " + percentageAmount;
    }
    private void AddToBoxCache()
    {
        BoxCache.cachedAllResearchablesCostReduced += percentageAmount;
    }
    public override void InitializePermanentStat()
    {
        base.InitializePermanentStat();

        AddToBoxCache();
    }
}