using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject slotsHolder;
    [SerializeField] private ItemClass itemToAdd;
    [SerializeField] private ItemClass itemToRemove;

    public List<ItemClass> items = new List<ItemClass>();

    private GameObject[] slots;
    public void Start()
    {
        slots = new GameObject[slotsHolder.transform.childCount];
        for(int i = 0; i< slotsHolder.transform.childCount; i++)
        {
            slots[i] = slotsHolder.transform.GetChild(i).gameObject;
        }
        ReFreshUI();
        Add(itemToAdd);
        Remove(itemToRemove);
    }
    public void ReFreshUI()
    {
        for(int i =0; i <slots.Length; i ++)
        {
            try
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].itemIcon;
            }
            catch
            {

            }
        }
    }
    public void Add(ItemClass item)
    {
        items.Add(item);
    }
    public void Remove(ItemClass item)
    {
        items.Remove(item); 
    }
}
