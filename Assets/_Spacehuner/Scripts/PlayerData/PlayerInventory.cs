using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SH.PlayerData
{
    public class PlayerInventory
    {
        public InventoryData Data { private set; get; }

        public void Setup(InventoryData data)
        {
            this.Data = data;
        }

        public int GetAmount(int id)
        {
            int amount = 0;
            Data.DctItem.TryGetValue(id, out amount);
            return amount;
        }

        public void AddItem(int id, int amount)
        {
            if(Data.DctItem.ContainsKey(id))
            {
                int currentAmount = Data.DctItem[id];
                Data.DctItem[id] = currentAmount + amount;
            }
            else
            {
                Data.DctItem[id] = amount;
            }
        }

        public bool SubItem(int id, int amount)
        {
            if (Data.DctItem.TryGetValue(id, out int currentAmount))
            {
                if (currentAmount < amount)
                    return false;
                Data.DctItem[id] = currentAmount - amount;
                return true;
            }
            return false;
        }
    }


    public class InventoryData
    {
        public int AmountCell;
        public Dictionary<int, int> DctItem = new Dictionary<int, int>();
    }
}
