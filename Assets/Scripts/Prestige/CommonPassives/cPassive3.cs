using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cPassive3 : CommonPassive
{
    private CommonPassive _commonPassive;

    private void Awake()
    {
        _commonPassive = GetComponent<CommonPassive>();
        CommonPassives.Add(Type, _commonPassive);
    }
    public override void ExecutePassive()
    {
        base.ExecutePassive();
        Debug.Log("This for for Common 3 specifically");
    }
}