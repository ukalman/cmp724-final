using UnityEngine;
using UnityEngine.Serialization;

public enum AmmoType
{
    None,       // Melee silahlar için
    Normal,     // Mermi, kurşun
    Fire,       // Alevli mermiler
    Energy      // Lazer/plazma
}

[CreateAssetMenu(menuName = "Items/Ammo")]
public class AmmoConfig : ItemConfig
{
    public AmmoType ammoType;
    public int initialStack = 50;

    private void OnEnable()
    {
        itemType = ItemType.Misc; // İstersen ayrı bir ItemType.Ammo da tanımlayabiliriz
    }
}
