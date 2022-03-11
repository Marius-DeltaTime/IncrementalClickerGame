using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Start next/each run with a certain number of a random Building.
public class rPassive3 : RarePassive
{
    private RarePassive _rarePassive;

    private uint _selfCountIncreaseAmount = 4;
    private BuildingType buildingTypeChosen;

    private void Awake()
    {
        _rarePassive = GetComponent<RarePassive>();
        RarePassives.Add(Type, _rarePassive);
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
        if (_selfCountIncreaseAmount > 1)
        {
            description = string.Format("Start each run with an additional {0} {1}'s", _selfCountIncreaseAmount, Building.Buildings[buildingTypeChosen].actualName);
        }
        else
        {
            description = string.Format("Start each run with an additional {1}", _selfCountIncreaseAmount, Building.Buildings[buildingTypeChosen].actualName);
        }
     
        AddToBoxCache();
    }
    private void AddToBoxCache()
    {
        if (!BoxCache.cachedBuildingSelfCountModified.ContainsKey(buildingTypeChosen))
        {
            BoxCache.cachedBuildingSelfCountModified.Add(buildingTypeChosen, _selfCountIncreaseAmount);
        }
        else
        {
            BoxCache.cachedBuildingSelfCountModified[buildingTypeChosen] += _selfCountIncreaseAmount;
        }
    }
    public override void InitializePermanentStat()
    {
        base.InitializePermanentStat();

        ChooseRandomBuilding();
    }
}
