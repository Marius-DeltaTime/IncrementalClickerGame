using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// Decrease cost of random Researchable by a certain %.
public class rPassive7 : RarePassive
{
    private RarePassive _rarePassive;
    private float percentageAmount = 0.036f; // 3.6%
    private ResearchType researchTypeChosen;

    private void Awake()
    {
        _rarePassive = GetComponent<RarePassive>();
        RarePassives.Add(Type, _rarePassive);
    }
    private void ChooseRandomResearchable()
    {
        List<ResearchType> researchTypesInCurrentRun = new List<ResearchType>();

        foreach (var researchable in Researchable.Researchables)
        {
            if (researchable.Value.isUnlocked)
            {
                researchTypesInCurrentRun.Add(researchable.Key);
            }
        }
        if (researchTypesInCurrentRun.Count >= Prestige.researchablesUnlockedInPreviousRun.Count)
        {
            _index = Random.Range(0, researchTypesInCurrentRun.Count);
            researchTypeChosen = researchTypesInCurrentRun[_index];
        }
        else
        {
            _index = Random.Range(0, Prestige.researchablesUnlockedInPreviousRun.Count);
            researchTypeChosen = Prestige.researchablesUnlockedInPreviousRun[_index];
        }
        description = string.Format("Decrease the cost to research '{0}' by {1}%", Researchable.Researchables[researchTypeChosen].actualName, percentageAmount*100);

        AddToBoxCache();
    }
    private void AddToBoxCache()
    {
        if (!BoxCache.cachedResearchableCostReduced.ContainsKey(researchTypeChosen))
        {
            BoxCache.cachedResearchableCostReduced.Add(researchTypeChosen, percentageAmount);
        }
        else
        {
            BoxCache.cachedResearchableCostReduced[researchTypeChosen] += percentageAmount;
        }
    }
    public override void InitializePermanentStat()
    {
        base.InitializePermanentStat();

        ChooseRandomResearchable();
    }
}
