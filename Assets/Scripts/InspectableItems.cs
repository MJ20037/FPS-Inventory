using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider))]
public class InspectableItem : MonoBehaviour, IPointerClickHandler
{
    public ItemInstance instance;

    // These track persistent state
    public bool IsPicked { get; private set; }
    public bool IsUsed { get; private set; }
    public bool IsDropped { get; private set; }

    private void Awake()
    {
        instance = GetComponent<ItemInstance>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsPicked) return;

        switch (instance.state)
        {
            case ItemState.Inspectable:
            case ItemState.Pickupable:
                InspectionController.Instance.StartInspect(this);
                break;
            default:
                break;
        }
    }

    // Call this when player inspects and confirms pickup
    public void Pickup()
    {
        if (InventoryUIManager.Instance.AddItem(instance.data))
        {
            IsPicked = true;
            IsDropped = false;
            gameObject.SetActive(false);
        }
    }

    public void Drop(Vector3 dropPosition)
    {
        IsDropped = true;
        transform.position = dropPosition;
        gameObject.SetActive(true);
    }

    public void Use()
    {
        IsUsed = true;

        if (instance.data.isFlashlight)
            PlayerInventory.Instance.EnableFlashlight();

        if (instance.data.isConsumable)
            InventoryUIManager.Instance.UseItem(instance.data);
    }

    // Called when loading game to restore item state
    public void ApplySaveData(ItemSaveData saveData)
    {
        IsPicked = saveData.isPicked;
        IsUsed = saveData.isUsed;
        IsDropped = saveData.isDropped;

        if (IsPicked && !IsDropped)
        {
            gameObject.SetActive(false);
        }
        else if (IsDropped)
        {
            transform.position = saveData.droppedPosition;
            gameObject.SetActive(true);
        }

        if (IsUsed && instance.data.isFlashlight)
        {
            PlayerInventory.Instance.EnableFlashlight();
        }
    }

    // Returns the save data for this item
    public ItemSaveData ToSaveData()
    {
        return new ItemSaveData
        {
            itemName = instance.data.itemName,
            isPicked = IsPicked,
            isUsed = IsUsed,
            isDropped = IsDropped,
            droppedPosition = transform.position
        };
    }

}
