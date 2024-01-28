using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueDisplay : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text valueText;
    private void Awake()
    {
        slider.onValueChanged.AddListener(UpdateDisplayValue);
    }
    private void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(UpdateDisplayValue);
    }
    private void UpdateDisplayValue(float _newValue)
    {
        valueText.text = Mathf.RoundToInt(_newValue).ToString();
    }
}
