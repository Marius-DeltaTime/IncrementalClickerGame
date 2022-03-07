//ePassive8: Decrease cost of ALL Craftables by a certain %.
public class lPassive8 : LegendaryPassive
{
    private LegendaryPassive _legendaryPassive;
    private float percentageAmount = 0.01f; //1%

    private void Awake()
    {
        _legendaryPassive = GetComponent<LegendaryPassive>();
        LegendaryPassives.Add(Type, _legendaryPassive);
        description = string.Format("Decrease cost of all the Craftables by {0}%", percentageAmount*100);
    }
    private void AddToBoxCache()
    {
        BoxCache.cachedAllCraftablesCostReduced += percentageAmount;
    }
    public override void InitializePermanentStat()
    {
        base.InitializePermanentStat();

        AddToBoxCache();
    }
}
