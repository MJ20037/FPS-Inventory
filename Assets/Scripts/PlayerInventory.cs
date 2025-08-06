using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;
    public Flashlight flashlight;

    private void Awake()
    {
        Instance = this;
    }

    public void EnableFlashlight()
    {
        flashlight.enabled = true;
        flashlight.gameObject.transform.SetParent(Camera.main.transform);
        flashlight.gameObject.transform.localPosition = new Vector3(0, -0.6f, 1f);
        flashlight.GetComponent<Rigidbody>().useGravity = false;
        flashlight.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void Update()
    {
        if (flashlight != null && Input.GetKeyDown(KeyCode.F))
        {
            flashlight.Toggle();
        }
        if (Input.GetKeyDown(KeyCode.K)) SaveSystem.SaveGame();
        if (Input.GetKeyDown(KeyCode.L)) SaveSystem.LoadGame();
    }
}
