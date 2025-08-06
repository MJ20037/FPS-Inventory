using UnityEngine;
using System.Collections;
using TMPro;

public enum ProgressionLevel
{
    Level1,
    Level2,
    Level3
}
public class ProgressionManager : MonoBehaviour
{
    public static ProgressionManager Instance;

    public ProgressionLevel currentLevel = ProgressionLevel.Level1;
    [SerializeField] private float level2Time = 60f;
    [SerializeField] private float level3Time = 120f;
    [SerializeField] private TextMeshProUGUI levelMessageText;
    [SerializeField] ItemInstance[] items; 

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else Instance = this;
    }

    private void Start()
    {
        StartCoroutine(ProgressLevels());
    }

    IEnumerator ProgressLevels()
    {
        yield return new WaitForSeconds(level2Time);
        currentLevel = ProgressionLevel.Level2;
        levelMessageText.text = "4 items are now pickupable.";
        foreach (var item in items)
        {
            if (item.data.pickupableInLevel2)
            {
                item.state = ItemState.Pickupable;
            }
        }
        yield return new WaitForSeconds(5);
        levelMessageText.text = "";

        yield return new WaitForSeconds(level3Time - level2Time);
        currentLevel = ProgressionLevel.Level3;
        levelMessageText.text = "Remaining items are pickupable. Some items are now usable.";
        foreach (var item in items)
        {
            if (item.data.pickupableInLevel3)
            {
                item.state = ItemState.Pickupable;
            }
            if (item.data.usableInLevel3)
            {
                item.data.isConsumable = true;
            }
        }
        yield return new WaitForSeconds(5);
        levelMessageText.text = "";
    }

}
