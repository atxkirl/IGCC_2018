using UnityEngine;
using UnityEngine.UI;

public class UITooltip : SingletonMonoBehaviour<UITooltip>
{
    private Text tooltipText;
    private Canvas canvas;
    private GameObject objectUsingTooltip;

    private void Start()
    {
        tooltipText = transform.GetChild(0).GetComponent<Text>();
        canvas = FindObjectOfType<Canvas>();

        ExitHover();
    }

    private void Update()
    {
        if (objectUsingTooltip != null && !objectUsingTooltip.activeInHierarchy)
        {
            ExitHover();
        }
        else if(objectUsingTooltip)
        {
            transform.position = Input.mousePosition + new Vector3(GetComponent<RectTransform>().sizeDelta.x * 0.5f, -GetComponent<RectTransform>().sizeDelta.y, 0f);
        }
    }

    //PUBLIC FUNCTIONS

    public void EnterHover(string textToDisplay, GameObject objectDisplaying)
    { 
        gameObject.GetComponent<CanvasRenderer>().SetAlpha(1.0f);
        tooltipText.enabled = true;
        tooltipText.text = textToDisplay;

        objectUsingTooltip = objectDisplaying;
    }

    public void ExitHover()
    {
        objectUsingTooltip = null;

        gameObject.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
        tooltipText.enabled = false;
        tooltipText.text = "";
    }
}