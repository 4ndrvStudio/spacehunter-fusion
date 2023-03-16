using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Inventory : MonoBehaviour
{
    public List<MineralClass> items = new List<MineralClass>();
    public List<GameObject> _button = new List<GameObject>();
    public static Inventory instance;

    public int inventoryMaxSlot = 50;

    public Transform itemsContent;
    public GameObject inventoryItem;
    private void Awake()
    {
        if(instance != null)
        {

            return;
        }
        instance = this;
    }
    public void itemIsNull()
    {
        bool isNull;
        string itemName = GetComponent<MineralClass>().itemName;
        if (itemName == null)
        {
            isNull = true;     
        }
        if (itemName!= null)
        {
            isNull = false;
        }
    }
    public void Add(MineralClass item)
    {
        items.Add(item);
       // Debug.Log(items.Count);
    }

    public void ListItem(MineralClass item)
    {

         GameObject obj = Instantiate(inventoryItem, itemsContent);
         var itemIcon = obj.transform.GetChild(1).GetComponent<Image>();
        itemIcon.sprite = item.icon;
        _button.Add(obj);
      
    }

}
