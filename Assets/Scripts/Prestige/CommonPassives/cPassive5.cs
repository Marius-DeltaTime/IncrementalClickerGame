using System.Collections.Generic;
using UnityEngine;

// Increase production of a random production Building by a certain %.
public class cPassive5 : CommonPassive
{
    private CommonPassive _commonPassive;
    private BuildingType buildingTypeChosen;
    private float permanentAmount = 0.01f, prestigeAmount = 0.05f;

    private void Awake()
    {
        _commonPassive = GetComponent<CommonPassive>();
        CommonPassives.Add(Type, _commonPassive);
    }
    private void ChooseRandomBuilding()
    {
        List<BuildingType> buildingTypesInCurrentRun = new List<BuildingType>();

        foreach (var building in Building.Buildings)
        {
            if (building.Value.isUnlocked)
            {
                buildingTypesInCurrentRun.Add(building.Key);
            }
        }
        if (buildingTypesInCurrentRun.Count >= Prestige.buildingsUnlockedInPreviousRun.Count)
        {
            _index = Random.Range(0, buildingTypesInCurrentRun.Count);
            buildingTypeChosen = buildingTypesInCurrentRun[_index];
        }
        else
        {
            _index = Random.Range(0, Prestige.buildingsUnlockedInPreviousRun.Count);
            buildingTypeChosen = Prestige.buildingsUnlockedInPreviousRun[_index];
        }
    }
    private void AddToBoxCache(float percentageAmount)
    {
        if (!BoxCache.cachedBuildingMultiplierModified.ContainsKey(buildingTypeChosen))
        {
            BoxCache.cachedBuildingMultiplierModified.Add(buildingTypeChosen, percentageAmount);
        }
        else
        {
            BoxCache.cachedBuildingMultiplierModified[buildingTypeChosen] += percentageAmount;
        }
    }
    private void ModifyStatDescription(float percentageAmount)
    {
        description = string.Format("Increase production of building '{0}' by {1}%", Building.Buildings[buildingTypeChosen].actualName, percentageAmount * 100);
    }
    public override void InitializePermanentStat()
    {
        ChooseRandomBuilding();
        ModifyStatDescription(permanentAmount);
        AddToBoxCache(permanentAmount);
    }
    public override void InitializePrestigeStat()
    {
        ChooseRandomBuilding();
        ModifyStatDescription(prestigeAmount);
    }
    public override void InitializePrestigeButtonBuilding(BuildingType buildingType)
    {
        AddToBoxCache(prestigeAmount);
    }
    public override BuildingType ReturnBuildingType()
    {
        return buildingTypeChosen;
    }
}
