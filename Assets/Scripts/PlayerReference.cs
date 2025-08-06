using UnityEngine;

public class PlayerReference : MonoBehaviour
{
    public static PlayerReference Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }
}
