using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetClickItem : MonoBehaviour
{

    public GameObject imageFocus;


    public void ToggleActive()
    {
        Inventory.instance._button.ForEach(btn =>
        {
            btn.GetComponent<SetClickItem>().imageFocus.SetActive(false);
        });
        this.imageFocus.SetActive(true);
    }

    //public void SelectItem()
    //{
    //    //Inventory.instance.UnselectAll();
    //    imageFocus.gameObject.SetActive(true);
    //}
    //public void UnselectItem()
    //{
    //    imageFocus.gameObject.SetActive(false);
    //}
}
