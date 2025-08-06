using UnityEngine;
using UnityEngine.UI;


public class Flashlight : MonoBehaviour
{
    public Light flashlightLight;
    public float maxBattery = 100f;
    public float batteryDrainRate = 10f;
    public float battery;
    public bool isOn = false;
    public float lightIntensity=20f;
    public Slider batterySlider;

    private void Start()
    {
        battery = maxBattery;
        flashlightLight.enabled = false;
    }

    private void Update()
    {
        if (isOn && battery > 0)
        {
            battery -= batteryDrainRate * Time.deltaTime;
            battery = Mathf.Max(battery, 0);
            UpdateLightIntensity();
        }

        if (battery <= 0 && isOn)
            Toggle(false);
        batterySlider.value = battery / maxBattery;

    }

    public void Toggle(bool state)
    {
        isOn = state && battery > 0;
        flashlightLight.enabled = isOn;
    }

    public void Toggle()
    {
        Toggle(!isOn);
    }

    public bool IsDead()
    {
        return battery <= 0f;
    }

    public void Recharge(float amount)
    {
        battery = Mathf.Clamp(battery + amount, 0, maxBattery);
        batterySlider.value = battery / maxBattery;
    }

    void UpdateLightIntensity()
    {
        float intensityFactor = battery / maxBattery;
        flashlightLight.intensity = Mathf.Lerp(0.1f, lightIntensity, intensityFactor);
    }
}
