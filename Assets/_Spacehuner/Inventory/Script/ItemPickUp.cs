using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemPickUp : MonoBehaviour
{
    public MineralClass item;
    int numb = 0;
    void PickUp()
    {
        if (Inventory.instance.items.Count == Inventory.instance.inventoryMaxSlot)
        {
            return;
        }

        if (Inventory.instance.items.Count <= Inventory.instance.inventoryMaxSlot - 1)
        {
            if(item!= null)
            {
                Inventory.instance.Add(item);
                Inventory.instance.ListItem(item);
            }

        }
        
    }
    private void OnMouseDown()
    {
        PickUp();
    }
    
}
