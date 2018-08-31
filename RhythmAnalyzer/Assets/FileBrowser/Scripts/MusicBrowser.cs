using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GracesGames.SimpleFileBrowser.Scripts;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class MusicBrowser : SingletonMonoBehaviour<MusicBrowser>
{
    /// <summary>
    /// Occurs when a file has been selected in the browser.
    /// </summary>
    public event Action<string> FileSelected;

    private AudioImporter importer;
    private AudioSource audioSource;

    //File browser prefab
    public GameObject FileBrowserPrefab;
    //Orientation
    public ViewMode orientation = ViewMode.Landscape;

    //List of allowed file extension
    public string[] FileExtensions;

    //String to hold values
    private string textToWrite;

    private void Start()
    {
        if (GameObject.Find("Canvas") == null)
        {
            Debug.LogError("Make sure there is a canvas GameObject present in the Hierarcy.");
        }

        //Get components for audio importer and source
        importer = GetComponent<AudioImporter>();
        audioSource = GetComponent<AudioSource>();

        importer.Loaded += OnLoaded;
        FileSelected += OnFileSelected;
    }

    /// <summary>
    /// Opens the file explorer according to the mode provided. Defaults to read mode.
    /// </summary>
    /// <param name="fileBrowserMode">File browser mode.</param>
    private void OpenFileBrowser(FileBrowserMode fileBrowserMode = FileBrowserMode.Read)
    {
        //Create the file browser and name it
        GameObject fileBrowserObject = Instantiate(FileBrowserPrefab, transform);
        fileBrowserObject.name = "FileBrowser";

        //Set file explorer orientation
        FileBrowser fileBrowserScript = fileBrowserObject.GetComponent<FileBrowser>();
        fileBrowserScript.SetupFileBrowser(orientation);

        //Setup file explorer according to mode
        if (fileBrowserMode == FileBrowserMode.Write)
        {
            fileBrowserScript.SaveFilePanel("DemoText", FileExtensions);

            // Subscribe to OnFileSelect event
            fileBrowserScript.OnFileSelect += SaveFileUsingPath;
        }
        else
        {
            fileBrowserScript.OpenFilePanel(FileExtensions);

            // Subscribe to OnFileSelect event
            fileBrowserScript.OnFileSelect += LoadFileUsingPath;
        }
    }

    /// <summary>
    /// Allows Unity Button to open file explorer using boolean parameter. Defaults to read mode.
    /// </summary>
    /// <param name="write">Boolean representation of write mode. Write=True, Read=False.</param>
    public void OpenFileBrowser(bool write = false)
    {
        if(write)
            OpenFileBrowser(FileBrowserMode.Write);
        else
            OpenFileBrowser(FileBrowserMode.Read);
    }

    /// <summary>
    /// Creates a file at the filepath provided with the contents of textToSave.
    /// </summary>
    /// <param name="path">Location to save the file.</param>
    private void SaveFileUsingPath(string path)
    {
        // Make sure path and _textToSave is not null or empty
        if (!String.IsNullOrEmpty(path) && !String.IsNullOrEmpty(textToWrite))
        {
            BinaryFormatter bFormatter = new BinaryFormatter();
            // Create a file using the path
            FileStream file = File.Create(path);
            // Serialize the data (textToSave)
            bFormatter.Serialize(file, textToWrite);
            // Close the created file
            file.Close();
        }
        else
        {
            Debug.Log("Invalid path or empty file given");
        }
    }

    /// <summary>
    /// Opens file at the filepath provided.
    /// </summary>
    /// <param name="path">Location of the file.</param>
    private void LoadFileUsingPath(string path)
    {
        if (path.Length != 0)
        {
            Debug.Log("LoadFileUsingPath(string path). Path given=" + path);
            //Invokes OnFileSelected event
            //if (FileSelected != null)
            //FileSelected.Invoke(path);

            importer.Import(path);
        }
        else
        {
            Debug.Log("Invalid path given");
        }
    }

    /// <summary>
    /// Sets the audio clip within the specified audio source.
    /// </summary>
    /// <param name="clip">AudioClip to pass to the audio source.</param>
    private void OnLoaded(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    /// <summary>
    /// Imports file from specified filepath using AudioImporter.
    /// </summary>
    /// <param name="path">Location of the file.</param>
    private void OnFileSelected(string path)
    {
        Debug.Log("OnFileSelected(string path). Path given=" + path);
        importer.Import(path);
    }
}
