using System.Collections.Generic;

public enum BodyPartType
{
    Head,
    Torso,
    Arm,
    Leg
}

[System.Serializable]
public class BodyPartData
{
    public BodyPartType partType;
    public float accuracyPenalty;
    public float criticalBonus;
    public float damageMultiplier;
    public string debuffDescription;

    public BodyPartData(BodyPartType partType, float accuracyPenalty, float criticalBonus, float damageMultiplier, string debuffDescription)
    {
        this.partType = partType;
        this.accuracyPenalty = accuracyPenalty;
        this.criticalBonus = criticalBonus;
        this.damageMultiplier = damageMultiplier;
        this.debuffDescription = debuffDescription;
    }
}


public static class BodyPartLibrary
{
    public static readonly Dictionary<BodyPartType, BodyPartData> Parts = new Dictionary<BodyPartType, BodyPartData>
    {
        { BodyPartType.Head,  new BodyPartData(BodyPartType.Head,  -20.0f,  25.0f, 1.5f, "Stunned") },
        { BodyPartType.Torso, new BodyPartData(BodyPartType.Torso,   0.0f,   0.0f, 1.0f, "No effect") },
        { BodyPartType.Arm,   new BodyPartData(BodyPartType.Arm,   -10.0f,  10.0f, 1.0f, "Accuracy lowered") },
        { BodyPartType.Leg,   new BodyPartData(BodyPartType.Leg,   -10.0f,   5.0f, 1.0f, "Agility lowered") },
    };

    public static BodyPartData GetData(BodyPartType type)
    {
        return Parts.TryGetValue(type, out var data) ? data : null;
    }
}
