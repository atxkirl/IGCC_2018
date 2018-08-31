using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Base class for all UI Buttons which provides audio feedback and tooltips.
/// </summary>
[ExecuteInEditMode]
public class UIButtonBase : MonoBehaviour
{
    public Text buttonText;
    public string buttonName;
    public string buttonDescription;

    private Canvas canvas;

    private void Start()
    {
        canvas = FindObjectOfType<Canvas>();

        //Set name of button
        buttonText.text = buttonName;
    }

    //PUBLIC FUNCTIONS

    public void EnterHover()
    {
        UITooltip.Instance.EnterHover(buttonDescription, gameObject);
    }

    public void ExitHover()
    {
        UITooltip.Instance.ExitHover();
    }
}
