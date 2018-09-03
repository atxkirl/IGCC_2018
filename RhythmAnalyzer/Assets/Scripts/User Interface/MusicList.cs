using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EZObjectPools;

public class MusicList : MonoBehaviour
{
    public GameObject buttonPrefab;
    public MusicBrowser browser;

    private List<string> urls;
    private EZObjectPool objectPool;

    private void Start()
    {
        objectPool = EZObjectPool.CreateObjectPool(buttonPrefab, "MusicButtonSpawner", 10, true, true, false);

        StartCoroutine(GenerateButtons());
    }

    private IEnumerator GenerateButtons()
    {
        while(true)
        {
            urls = new List<string>();
            urls = browser.fileURLs.ToList();

            //Deactivate all buttons
            foreach (GameObject obj in objectPool.ObjectList)
            {
                obj.SetActive(false);
            }

            //Generate buttons
            foreach (string url in urls)
            {
                GameObject button;
                objectPool.TryGetNextObject(transform.position, transform.rotation, out button);

                string songName;

                if (url.Contains('/'))
                    songName = url.Substring(url.LastIndexOf('/') + 1);
                else
                    songName = url.Substring(url.LastIndexOf('\\') + 1);

                button.GetComponent<UIButtonBase>().buttonName = songName;
                button.GetComponent<UIButtonBase>().buttonDescription = url;
                button.GetComponent<UIButtonBase>().toolTipEnabled = false;

                button.transform.SetParent(this.transform);
                button.SetActive(true);
            }

            //Update music list every second
            yield return new WaitForSeconds(1f);
        }
    }
}
