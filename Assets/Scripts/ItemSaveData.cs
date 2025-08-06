using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ItemSaveData
{
    public string itemName;
    public bool isPicked;
    public bool isUsed;
    public bool isDropped;
    public Vector3 droppedPosition;
}

[System.Serializable]
public class GameSaveData
{
    public List<ItemSaveData> savedItems = new List<ItemSaveData>();
}