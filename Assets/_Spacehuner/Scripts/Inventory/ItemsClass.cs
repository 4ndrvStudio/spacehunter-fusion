using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName ="New Item", menuName ="GameData/Mineral")]
public class ItemsClass : ScriptableObject
{
    public string name = "New Item";
    public List<ScriptableObject> vld = new List<ScriptableObject>();
    public Sprite icon = null;
    public bool isDefaultItem = false;
}
