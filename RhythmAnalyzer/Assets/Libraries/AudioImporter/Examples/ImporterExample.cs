using UnityEngine;

public class ImporterExample : MonoBehaviour
{
    public FileBrowserMobile browser;
    public AudioImporter importer;
    public AudioSource audioSource;

    void Awake()
    {
        browser.FileSelected += OnFileSelected;
        importer.Loaded += OnLoaded;
    }
    
    private void OnFileSelected(string path)
    {
        Debug.Log("FileBrowserMobile Path: " + path);
        importer.ImportStreaming(path);
    }

    private void OnLoaded(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
