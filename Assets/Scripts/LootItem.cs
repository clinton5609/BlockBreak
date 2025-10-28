using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootItem
{
    public GameObject itemPrefab; // object to drop
    [Range(0, 100)] public float dropChance; // drop chances 
    
}
