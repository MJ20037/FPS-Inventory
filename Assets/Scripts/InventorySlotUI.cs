using UnityEngine;
using UnityEngine.UI;
public class InventorySlotUI : MonoBehaviour
{
    public Button dropButton;
    private ItemData itemData;
    private int quantity;

    public void Setup(ItemData data, int qty)
    {
        itemData = data;
        quantity = qty;
        dropButton.onClick.AddListener(OnDrop);
    }

    void OnDrop()
    {
        InventoryUIManager.Instance.DropItem(itemData);
    }
}
