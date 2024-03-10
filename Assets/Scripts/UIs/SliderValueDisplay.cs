using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderValueDisplay : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text valueText;
    [SerializeField] private Sprite pressDownImage;
    [SerializeField] private Sprite releaseUpImage;
    private Image handleImage;
    private void Awake()
    {
        slider.onValueChanged.AddListener(UpdateDisplayValue);
        handleImage = slider.handleRect.GetComponent<Image>();
    }
    private void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(UpdateDisplayValue);
    }
    internal void UpdateDisplayValue(float _newValue)
    {
        valueText.text = Mathf.RoundToInt(_newValue).ToString();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (handleImage != null)
            handleImage.sprite = pressDownImage;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (handleImage != null)
            handleImage.sprite = releaseUpImage;
    }
}
