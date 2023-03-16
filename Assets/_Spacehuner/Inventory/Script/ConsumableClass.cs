using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "new Tool class", menuName = "Item/Consumable")]

public class ConsumableClass : ItemClass
{
    [Header("Consumable")]
    public float healthAdd;
    public override ItemClass GetItem() { return this; }
    public override ToolClass GetTool() { return null; }
    public override MiscClass GetMisc() { return null; }
    public override ConsumableClass GetConsumable() { return this; }
}
