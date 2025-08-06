using UnityEngine;

public class ItemInstance : MonoBehaviour
{
    [Tooltip("Static data shared by all instances")]
    public ItemData data;

    [Tooltip("Runtime state for this particular instance")]
    [HideInInspector]
    public ItemState state;

    private void Awake()
    {
        // Initialize this instance’s state from the ScriptableObject default
        state = data.startingState;
    }
}
