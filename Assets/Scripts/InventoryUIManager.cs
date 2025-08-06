using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUIManager : MonoBehaviour
{
    public static InventoryUIManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] GameObject slotPrefab;
    [SerializeField] Transform slotParent;
    [SerializeField] GameObject inventoryUICanvas;

    [Header("Item Info Panel")]
    [SerializeField] private GameObject itemInfoPanel;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemDescriptionText;
    [SerializeField] private Button useButton;
    [SerializeField] private Button dropButton;

    [Header("Grid Settings")]
    public int totalSlots = 20;

    // Internal slot data
    private List<InventorySlot> slots = new List<InventorySlot>();
    private InventorySlot currentSelectedSlot;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;

        BuildGrid();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            ToggleInventory();
    }

    private void BuildGrid()
    {
        // Clear existing (if reloading)
        foreach (Transform child in slotParent)
            Destroy(child.gameObject);
        slots.Clear();

        // Instantiate slots
        for (int i = 0; i < totalSlots; i++)
        {
            GameObject go = Instantiate(slotPrefab, slotParent);
            var slot = go.GetComponent<InventorySlot>();
            if (slot == null)
                slot = go.AddComponent<InventorySlot>();
            slot.Clear();
            slots.Add(slot);
        }

        inventoryUICanvas.SetActive(false);
        itemInfoPanel.SetActive(false);
    }

    public void ToggleInventory()
    {
        inventoryUICanvas.SetActive(!inventoryUICanvas.activeSelf);
        if (!inventoryUICanvas.activeSelf)
            itemInfoPanel.SetActive(false);
        if (inventoryUICanvas.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public bool AddItem(ItemData data, int amount = 1)
    {
        if (data.stackable)
        {
            var existing = slots.Find(s => s.HasItem(data));
            if (existing != null)
            {
                existing.AddCount(amount);
                return true;
            }
        }

        var empty = slots.Find(s => s.IsEmpty);
        if (empty != null)
        {
            empty.SetItem(data, amount);
            return true;
        }

        Debug.LogWarning("Inventory is full!");
        return false;
    }

    public void ShowItemDetails(ItemData data, InventorySlot slot)
    {
        currentSelectedSlot = slot;

        itemNameText.text = data.itemName;
        itemDescriptionText.text = data.description;

        // Show/Hide Use button
        bool canBeUsed = data.isConsumable;
        useButton.gameObject.SetActive(canBeUsed);
        useButton.onClick.RemoveAllListeners();
        if (canBeUsed)
            useButton.onClick.AddListener(() => UseItem(data));

        // Show/Hide Drop button
        canBeUsed = data.canBeDropped;
        dropButton.gameObject.SetActive(canBeUsed);
        dropButton.onClick.RemoveAllListeners();
        if (canBeUsed)
            dropButton.onClick.AddListener(() => DropItem(data));

        itemInfoPanel.SetActive(true);
    }

    public void UseItem(ItemData data)
    {
        Debug.Log("Used item: " + data.itemName);
        if (data.itemName == "Battery")
        {
            PlayerInventory.Instance.flashlight?.Recharge(25f); // Recharge flashlight
        }

        if (data.stackable)
            currentSelectedSlot.ReduceCount(1);
        else
            currentSelectedSlot.Clear();

        itemInfoPanel.SetActive(false);
    }

    public void DropItem(ItemData data)
    {
        Debug.Log("Dropped item: " + data.itemName);

        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player && data.prefab)
        {
            Vector3 dropPosition = player.position + new Vector3(0, 0, 2);
            Instantiate(data.prefab, dropPosition, Quaternion.identity);
        }

        if (data.stackable)
            currentSelectedSlot.ReduceCount(1);
        else
            currentSelectedSlot.Clear();

        itemInfoPanel.SetActive(false);
    }
    

}
