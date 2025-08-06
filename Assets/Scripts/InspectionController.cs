using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InspectionController : MonoBehaviour
{
    public static InspectionController Instance { get; private set; }

    [Tooltip("Parent under the camera where inspected items go.")]
    public Transform inspectionAnchor;

    [Tooltip("Canvas with Cancel button.")]
    public Canvas inspectionCanvas;

    [Tooltip("Speed multipliers for rotate/zoom.")]
    public float rotationSpeed = 100f;
    public float zoomSpeed = 2f;
    public float minZoom = 0.5f, maxZoom = 3f;

    [Header("Pickup UI")]
    public Button pickupButton;

    private Transform currentItem;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Transform originalParent;
    private float originalScale;

    private Camera mainCam;
    private bool inspecting = false;

    private InspectableItem inspectingItemComp;
    private ItemData inspectingData;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        mainCam = Camera.main;
        inspectionCanvas.gameObject.SetActive(false);
        pickupButton.gameObject.SetActive(false);
    }

    public void StartInspect(InspectableItem item)
    {
        if (inspecting || item == null || item.instance == null || item.instance.data == null)
            return;

        inspecting = true;
        inspectingItemComp = item;
        inspectingData = item.instance.data;

        // Save original transform
        currentItem = item.transform;
        originalParent = currentItem.parent;
        originalPosition = currentItem.position;
        originalRotation = currentItem.rotation;
        originalScale = currentItem.localScale.x;

        // Move item under inspection anchor
        currentItem.SetParent(inspectionAnchor, true);
        currentItem.localPosition = Vector3.zero;
        currentItem.localRotation = Quaternion.identity;

        // Show UI
        inspectionCanvas.gameObject.SetActive(true);
        Time.timeScale = 0f;

        // Enable Pickup button only if the item can be picked
        bool canPickup = item.instance.state == ItemState.Pickupable;
        pickupButton.gameObject.SetActive(canPickup);
    }

    private void Update()
    {
        if (!inspecting || currentItem == null)
            return;

        // Rotate
        if (Input.GetMouseButton(0))
        {
            float h = Input.GetAxis("Mouse X") * rotationSpeed * Time.unscaledDeltaTime;
            float v = -Input.GetAxis("Mouse Y") * rotationSpeed * Time.unscaledDeltaTime;
            currentItem.Rotate(mainCam.transform.up, h, Space.World);
            currentItem.Rotate(mainCam.transform.right, v, Space.World);
        }

        // Zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.001f)
        {
            float scaleFactor = 1 + scroll * zoomSpeed;
            float current = currentItem.localScale.x * scaleFactor;
            current = Mathf.Clamp(current, minZoom, maxZoom);
            currentItem.localScale = Vector3.one * current;
        }
    }

    public void OnPickup()
    {
        if (inspectingData == null || inspectingItemComp == null)
            return;

        bool added = InventoryUIManager.Instance.AddItem(inspectingData, 1);
        if (!added)
        {
            Debug.LogWarning("Could not pick up – inventory full.");
            return;
        }

        // Mark item as picked
        inspectingItemComp.instance.state = ItemState.DroppableOnly;
        inspectingItemComp.gameObject.SetActive(false);

        bool isFlashlight = inspectingData.isFlashlight;
        if(isFlashlight)
            inspectingItemComp.gameObject.SetActive(true);

        // End inspection
        CancelInspect();

        if (isFlashlight)
        {
            PlayerInventory.Instance.EnableFlashlight();
        }
    }

    public void CancelInspect()
    {
        if (!inspecting) return;
        inspecting = false;

        // Restore item’s transform
        currentItem.SetParent(originalParent, true);
        currentItem.position = originalPosition;
        currentItem.rotation = originalRotation;
        currentItem.localScale = Vector3.one * originalScale;

        // Reset UI
        inspectionCanvas.gameObject.SetActive(false);
        pickupButton.gameObject.SetActive(false);
        Time.timeScale = 1f;

        // Reset inspection references
        
        currentItem = null;
        inspectingItemComp = null;
        inspectingData = null;
    }
}
