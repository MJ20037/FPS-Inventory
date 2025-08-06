using UnityEngine;
using System.Collections;

public class AmbientLightFlicker : MonoBehaviour
{
    public Light ambientLight;
    public Flashlight flashlight;

    public float flickerIntervalMin = 0.2f;
    public float flickerIntervalMax = 0.5f;

    private float nextFlickerTime;
    private bool isFlickering = false;

    private void Update()
    {
        if (flashlight == null || ambientLight == null)
            return;

        // Only flicker if flashlight is dead
        if (flashlight.IsDead())
        {
            HandleFlicker();
        }
        else
        {
            if (ambientLight.enabled == false)
                ambientLight.enabled = true;

            isFlickering = false;
        }
    }

    void HandleFlicker()
    {
        if (!isFlickering && Time.time >= nextFlickerTime)
        {
            isFlickering = true;
            StartCoroutine(FlickerCoroutine());
            nextFlickerTime = Time.time + Random.Range(flickerIntervalMin, flickerIntervalMax);
        }
    }

    IEnumerator FlickerCoroutine()
    {
        ambientLight.enabled = false;
        yield return new WaitForSeconds(Random.Range(0.05f, 0.1f)); // brief flicker off
        ambientLight.enabled = true;
        isFlickering = false;
    }
}
