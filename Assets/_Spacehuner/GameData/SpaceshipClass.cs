using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="Spaceship Class" , menuName = "Game Data/Spaceship")]
public class SpaceshipClass : ScriptableObject
{
    public string Name;
    public SpaceshipType SpaceshipType;
    public int NumMinePerDay;
    public Sprite Sprite;
    public GameObject Prefab;
    
}

public enum SpaceshipType {
    Common = 1,
    Uncommon = 2,
    Rare = 3,
    Epic = 4,
    Legend =5
}