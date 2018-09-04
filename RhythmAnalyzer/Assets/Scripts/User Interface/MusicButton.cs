using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MusicButton : UIButtonBase, IPointerClickHandler
{
    protected override void Start()
    {
        base.Start();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Stop any preview audio and reset audio clip
        if (MusicPreview.Instance.audioSource.isPlaying)
        {
            MusicPreview.Instance.audioSource.Stop();
            MusicPreview.Instance.audioSource.clip = null;
        }

        //Import new audio
        MusicPreview.Instance.audioImporter.Import(buttonDescription);
        MusicPreview.Instance.playImmediate = true;
    }
}
