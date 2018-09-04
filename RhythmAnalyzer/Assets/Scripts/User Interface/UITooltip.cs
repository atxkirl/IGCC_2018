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
            //Sizing of tooltip
            Vector3 tooltipSize = new Vector3(GetComponent<RectTransform>().sizeDelta.x, GetComponent<RectTransform>().sizeDelta.y, 0f);
            //Tooltip offset
            Vector3 tooltipOffsets = new Vector3(GetComponent<RectTransform>().sizeDelta.x * 0.55f, -GetComponent<RectTransform>().sizeDelta.y, 0f);
            
            //Constrain tooltip position to within canvas, taking into account the tooltip sizing
            Vector3 tempPos = Input.mousePosition + tooltipOffsets;

            //Tooltip will be too much to the right
            if (tempPos.x + tooltipSize.x > canvas.GetComponent<RectTransform>().sizeDelta.x)
            {
                tooltipOffsets.x = -GetComponent<RectTransform>().sizeDelta.x * 0.55f;
            }
            //Tooltip will be too low
            if (tempPos.y - tooltipSize.y < 0f)
            {
                tooltipOffsets.y = GetComponent<RectTransform>().sizeDelta.y;
            }

            transform.position = Input.mousePosition + tooltipOffsets;
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