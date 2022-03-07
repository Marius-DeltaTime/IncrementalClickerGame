//ePassive1: Reduce time it takes to research by a certain %.

public class lPassive1 : LegendaryPassive
{
    private LegendaryPassive _legendaryPassive;
    private float percentageAmount = 0.05f; //5%

    private void Awake()
    {
        _legendaryPassive = GetComponent<LegendaryPassive>();
        LegendaryPassives.Add(Type, _legendaryPassive);
        description = string.Format("Reduces time it takes to research by {0}%", percentageAmount * 100);
    }
    private void AddToBoxCache()
    {
        BoxCache.cachedResearchTimeReductionAmount += percentageAmount;
    }
    public override void InitializePermanentStat()
    {
        base.InitializePermanentStat();

        AddToBoxCache();
    }
}
