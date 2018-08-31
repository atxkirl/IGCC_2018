using System.Collections;
using UnityEngine;

/// <summary>
/// An importer for mobile platforms that uses Unity's WWW or UnityWebRequest for importing audio files.
/// </summary>
[AddComponentMenu("Audio/MobileImporter")]
public class MobileImporter : AudioImporter
{
    protected override IEnumerator Load(string uri)
    {

#if UNITY_5_4_OR_NEWER
#if UNITY_2017_1_OR_NEWER
        using (var request = UnityEngine.Networking.UnityWebRequestMultimedia.GetAudioClip(uri.ToString(), AudioType.MPEG))
#else
        using (var request = UnityEngine.Networking.UnityWebRequest.GetAudioClip(uri.ToString(), AudioType.MPEG))
#endif
        {
            var operation = request.SendWebRequest();

            while (!operation.isDone)
            {   
                yield return null;
                OnProgress(request.downloadProgress);
            }
                        
            if (request.isNetworkError)
            {
                OnError(request.error);
                yield break;
            }

            AudioClip audioClip = UnityEngine.Networking.DownloadHandlerAudioClip.GetContent(request);

            if (audioClip == null)
                yield break;            

            OnLoaded(audioClip);
        }
#else
        using (WWW www = new WWW(uri))
        {
            while (!www.isDone)
            {
                yield return null;
                OnProgress(www.progress);
            }

            if (!string.IsNullOrEmpty(www.error))
            {
                OnError(www.error);
                yield break;
            }

            AudioClip audioClip = www.audioClip;

            if (audioClip == null)
                yield break;

            OnLoaded(audioClip);
        }
#endif

    }

    protected override IEnumerator LoadStreaming(string uri, int initialLength)
    {
        Debug.LogWarning("MobileImporter does not support streaming.");
        yield return StartCoroutine(Load(uri));
    }
}
