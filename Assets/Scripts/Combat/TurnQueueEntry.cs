public class TurnQueueEntry
{
    public CombatModule combatModule;
    public HealthModule healthModule;
    public float initiative;

    public TurnQueueEntry(CombatModule combat, HealthModule health, float initiative)
    {
        this.combatModule = combat;
        this.healthModule = health;
        this.initiative = initiative;
    }
}