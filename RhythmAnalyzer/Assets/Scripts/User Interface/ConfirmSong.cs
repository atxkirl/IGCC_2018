using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ConfirmSong : UIButtonBase, IPointerClickHandler
{
    public GameObject audioController;
    public string gameScene;

    public void OnPointerClick(PointerEventData eventData)
    {
        //Audio has been selected
        if(audioController.GetComponent<AudioSource>().clip != null)
        {
            //Change scene to game scene
            SceneManager.LoadScene(gameScene);
        }
    }
}
