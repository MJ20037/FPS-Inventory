using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{
    public Image icon;
    public TMP_Text countText;

    private ItemData currentItem;
    private int count;

    public bool IsEmpty => currentItem == null;

    public void SetItem(ItemData item, int amount)
    {
        currentItem = item;
        count = amount;

        icon.sprite = item.icon;
        icon.enabled = true;
        UpdateCountText();
    }

    public void AddCount(int amount)
    {
        count += amount;
        UpdateCountText();
    }

    public void ReduceCount(int amount)
    {
        count -= amount;
        if (count <= 0)
            Clear();
        else
            UpdateCountText();
    }

    public void Clear()
    {
        currentItem = null;
        count = 0;
        icon.sprite = null;
        icon.enabled = false;
        countText.text = "";
    }

    public bool HasItem(ItemData item)
    {
        return currentItem == item;
    }

    private void UpdateCountText()
    {
        countText.text = currentItem.stackable ? count.ToString() : "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsEmpty) return;
        InventoryUIManager.Instance.ShowItemDetails(currentItem, this);
    }
}
