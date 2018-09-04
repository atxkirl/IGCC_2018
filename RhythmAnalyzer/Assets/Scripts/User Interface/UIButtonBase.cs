using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Base class for all UI Buttons which provides audio feedback and tooltips.
/// </summary>
public class UIButtonBase : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text buttonText;
    public string buttonName = "";
    public string buttonDescription = "";
    public bool toolTipEnabled = true;

    private Canvas canvas;

    protected virtual void Start()
    {
        canvas = FindObjectOfType<Canvas>();

        buttonText.text = buttonName;
    }

    protected virtual void Update()
    {
        buttonText.text = buttonName;
    }

    //PUBLIC FUNCTIONS

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(toolTipEnabled)
            UITooltip.Instance.EnterHover(buttonDescription, gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (toolTipEnabled)
            UITooltip.Instance.ExitHover();
    }
}
