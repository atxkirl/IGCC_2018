using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : UIButtonBase, IPointerClickHandler
{
    public GameObject menu;
    public bool startActive;

    protected override void Start()
    {
        base.Start();
        menu.SetActive(startActive);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        menu.SetActive(!menu.activeSelf);
    }
}
