using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="PortalClass", menuName ="Game Data/Portal")]
public class PortalClass : ScriptableObject
{
    public PortalName Name;
    public int NameSize;
    public GameObject Prefab;
    public string NamePortal;
}

public enum PortalName
{
    None,
    Planet_1,
    Planet_2,
    Planet_3,
    Planet_4,
    Planet_5,
    Planet_6,
}
