using UnityEngine;

public class FirePointLightSimulator : MonoBehaviour
{
    [SerializeField] private float blinkingFrequency = 1f;
    [SerializeField] private float minIntensity = 0.8f, maxIntensity = 2.5f;

    private float nextIntensity = 0f, currIntensity = 0f;
    private Light pointLight;

    private void Awake()
    {
        pointLight = GetComponent<Light>();
    }
    private void Update()
    {
        pointLight.intensity = GetIntensity();
    }
    private float GetIntensity()
    {
        currIntensity = Mathf.Lerp(currIntensity, nextIntensity, blinkingFrequency * Time.deltaTime);
        if (nextIntensity - currIntensity < 0.05f)
            nextIntensity = Utils.RandomRange(minIntensity, maxIntensity);

        return currIntensity;
    }
}
