using System.Collections.Generic;
using UnityEngine;

// cPassive 1: Reduce time it takes to research stuff by a certain %.
// cPassive 2: Increase a random Worker's Multiplier by a certain %.
// cPassive 3: Start next/each run with a certain number of a random Building.
// cPassive 4: Start next/each run with a certain number of Workers.
// cPassive 5: Increase production of a random production Building by a certain %.
// cPassive 6: Decrease cost of a random Craftable by a certain %.
// cPassive 7: Decrease cost of random Researchable by a certain %.
// cPassive 8: Increase storage limit of a random Storage Building by a certain %.
public enum CommonType
{
    Passive1,
    Passive2,
    Passive3,
    Passive4,
    Passive5,
    Passive6,
    Passive7,
    Passive8
}

public class CommonPassive : MonoBehaviour
{
    // Maybe just list
    public static Dictionary<CommonType, CommonPassive> CommonPassives = new Dictionary<CommonType, CommonPassive>();
    public CommonType Type;
    [System.NonSerialized] public string description;
    protected int _index;

    public virtual void InitializePermanentStat()
    {
        
    }
}

