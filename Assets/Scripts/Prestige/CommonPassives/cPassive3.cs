using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Start next/each run with a certain number of a random Building.
public class cPassive3 : CommonPassive
{
    private CommonPassive _commonPassive;
    private BuildingType buildingTypeChosen;
    private uint permanentAmount = 1, prestigeAmount = 5;

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
    private void AddToBoxCache(uint selfCountIncreaseAmount)
    {
        if (!BoxCache.cachedBuildingSelfCountModified.ContainsKey(buildingTypeChosen))
        {
            BoxCache.cachedBuildingSelfCountModified.Add(buildingTypeChosen, selfCountIncreaseAmount);
        }
        else
        {
            BoxCache.cachedBuildingSelfCountModified[buildingTypeChosen] += selfCountIncreaseAmount;
        }
    }
    private void ModifyStatDescription(uint selfCountIncreaseAmount)
    {
        if (selfCountIncreaseAmount > 1)
        {
            description = string.Format("Start each run with {0} additional {1}'s", selfCountIncreaseAmount, Building.Buildings[buildingTypeChosen].actualName);
        }
        else
        {
            description = string.Format("Start each run with an additional '{1}'", selfCountIncreaseAmount, Building.Buildings[buildingTypeChosen].actualName);
        }
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
