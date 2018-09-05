using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseButton : MenuButton
{ 
    public override void OnPointerClick(PointerEventData eventData)
    {
        //Toggle menu
        menu.SetActive(!menu.activeSelf);
        //Toggle pause state
        GameController.Instance.gamePaused = menu.activeSelf;
    }
}
