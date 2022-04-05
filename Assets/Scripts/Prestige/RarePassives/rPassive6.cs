using System.Collections.Generic;
using UnityEngine;

// Decrease cost of a random Craftable by a certain %.
public class rPassive6 : RarePassive
{
    private RarePassive _rarePassive;
    private CraftingType craftingTypeChosen;
    private float permanentAmount = 0.036f, prestigeAmount = 0.180f;

    private void Awake()
    {
        _rarePassive = GetComponent<RarePassive>();
        RarePassives.Add(Type, _rarePassive);
    }
    private void ChooseRandomCrafting()
    {
        List<CraftingType> craftingTypesInCurrentRun = new List<CraftingType>();

        foreach (var craft in Craftable.Craftables)
        {
            if (craft.Value.isUnlocked)
            {
                craftingTypesInCurrentRun.Add(craft.Key);
            }
        }
        if (craftingTypesInCurrentRun.Count >= Prestige.workersUnlockedInPreviousRun.Count)
        {
            _index = Random.Range(0, craftingTypesInCurrentRun.Count);
            craftingTypeChosen = craftingTypesInCurrentRun[_index];
        }
        else
        {
            _index = Random.Range(0, Prestige.craftablesUnlockedInPreviousRun.Count);
            craftingTypeChosen = Prestige.craftablesUnlockedInPreviousRun[_index];
        }
    }
    private void AddToBoxCache(float percentageAmount)
    {
        if (!BoxCache.cachedCraftableCostReduced.ContainsKey(craftingTypeChosen))
        {
            BoxCache.cachedCraftableCostReduced.Add(craftingTypeChosen, percentageAmount);
        }
        else
        {
            BoxCache.cachedCraftableCostReduced[craftingTypeChosen] += percentageAmount;
        }
    }
    private void ModifyStatDescription(float percentageAmount)
    {
        description = string.Format("Decrease the cost of crafting '{0}' by {1}%", Craftable.Craftables[craftingTypeChosen].actualName, percentageAmount * 100);
    }
    public override void InitializePermanentStat()
    {
        ChooseRandomCrafting();
        ModifyStatDescription(permanentAmount);
        AddToBoxCache(permanentAmount);
    }
    public override void InitializePrestigeStat()
    {
        ChooseRandomCrafting();
        ModifyStatDescription(prestigeAmount);
    }
    public override void InitializePrestigeButtonCrafting(CraftingType craftingType)
    {
        AddToBoxCache(prestigeAmount);
    }
    public override CraftingType ReturnCraftingType()
    {
        return craftingTypeChosen;
    }
}
