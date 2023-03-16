using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGetDame : MonoBehaviour
{
    public int heal = 30;

   public void OnCollisionEnter(Collision other) {
        if(other.collider.tag == "Weapon") {
            other.collider.enabled = false;
            heal -= other.collider.gameObject.GetComponent<Weapon>().GetDame();
        }
   }

 
}
