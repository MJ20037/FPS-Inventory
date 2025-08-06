using UnityEngine;

public enum ItemState
{
    Inspectable,
    Pickupable,
    Usable_Persistent,
    Usable_Consumable,
    DroppableOnly
}

[CreateAssetMenu(fileName = "NewItemData", menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject
{

    [Header("Basic Info")]
    public string itemName;
    [TextArea] public string description;
    public Sprite icon;

    [Header("Type")]
    public bool stackable;
    public bool isConsumable;
    public bool isFlashlight;

    [Header("Runtime State")]
    public ItemState startingState = ItemState.Inspectable;
    public GameObject prefab;

    [Header("Usability Settings")]
    public bool canBeDropped = true;

    public bool inspectableInLevel1 = true;
    public bool pickupableInLevel2;
    public bool pickupableInLevel3;
    public bool usableInLevel3;
}
