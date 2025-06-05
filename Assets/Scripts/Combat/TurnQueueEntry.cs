public class TurnQueueEntry
{
    public CombatModule combatModule;
    public HealthModule healthModule;
    public int initiative;

    public TurnQueueEntry(CombatModule combat, HealthModule health, int initiative)
    {
        this.combatModule = combat;
        this.healthModule = health;
        this.initiative = initiative;
    }
}