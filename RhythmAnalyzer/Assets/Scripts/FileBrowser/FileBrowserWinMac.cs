//using UnityEngine;
//using SFB;
//using System;

///// <summary>
///// File browser for Windows and MacOS.
///// </summary>
//public class FileBrowserWinMac : MonoBehaviour
//{
//    /// <summary>
//    /// Occurs when a file has been selected in the browser.
//    /// </summary>
//    public event Action<string> FileSelected;

//    private string filePath;
//    private AudioImporter importer;
//    private AudioSource audioSource;

//    private void Start()
//    {
//        importer = GetComponent<AudioImporter>();
//        audioSource = GetComponent<AudioSource>();

//        importer.Loaded += OnLoaded;
//        FileSelected += OnFileSelected;
//    }

//    private void OnLoaded(AudioClip clip)
//    {
//        audioSource.clip = clip;
//        audioSource.Play();
//    }

//    private void OnFileSelected(string path)
//    {
//        importer.Import(path);
//    }

//    /// <summary>
//    /// Combines all the filepaths into one string and logs it to Unity console.
//    /// </summary>
//    private void DebugFilePath(string[] paths)
//    {
//        if (paths.Length == 0)
//            return;

//        string result = "";
//        foreach (string p in paths)
//        {
//            result += p + "\n";
//        }

//        Debug.Log("FileBrowserWinMac Path: " + result);
//    }

//    /// <summary>
//    /// Opens OS-specific file browser with the ability to select multiple files. Allows selecting for all file types.
//    /// </summary>
//    public void OpenFile(bool multiSelect = false)
//    {
//        string[] pathsReturned = StandaloneFileBrowser.OpenFilePanel("Open File", "", "", multiSelect);
//        DebugFilePath(pathsReturned);

//        //Load each file
//        if(multiSelect)
//        {

//        }
//        //Load single file
//        else
//        {
//            filePath = pathsReturned[0];

//            if (FileSelected != null)
//                FileSelected.Invoke(filePath);
//        }
//    }

//    /// <summary>
//    /// Opens OS-specific file browser with the ability to select multiple files. Only allows specific file types according to input.
//    /// </summary>
//    /// <param name="extensionsAllowed"> File extensions that can be selected. </param>
//    public void OpenFile(string extensionsAllowed, bool multiSelect = false)
//    {
//        string[] pathsReturned = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensionsAllowed, multiSelect);
//        DebugFilePath(pathsReturned);

//        //Load each file
//        if (multiSelect)
//        {

//        }
//        //Load single file
//        else
//        {
//            filePath = pathsReturned[0];

//            if (FileSelected != null)
//                FileSelected.Invoke(filePath);
//        }
//    }
//}
