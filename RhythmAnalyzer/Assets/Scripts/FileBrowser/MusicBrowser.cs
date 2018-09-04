using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GracesGames.SimpleFileBrowser.Scripts;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;

public class MusicBrowser : SingletonMonoBehaviour<MusicBrowser>
{
    private AudioImporter importer;
    private AudioSource audioSource;

    //File browser prefab
    public GameObject fileBrowserPrefab;
    //Orientation
    public ViewMode orientation = ViewMode.Landscape;

    //List of allowed file extension
    public string[] fileExtensions;
    //Final List of all known URLs
    public string[] fileURLs;
    //List of default URLs
    public string[] defaultURLs;
    //List of imported audio clips
    public List<AudioClip> importedAudio;

    //String to hold values
    private string textToWrite;
    //PlayerPref key (DO NOT CHANGE)
    private string playerPrefKey = "MusicList";

    private void Start()
    {
        if (GameObject.Find("Canvas") == null)
        {
            Debug.LogError("Make sure there is a canvas GameObject present in the Hierarcy.");
        }

        //Get components for audio importer and source
        importer = GetComponent<AudioImporter>();
        audioSource = GetComponent<AudioSource>();

        //Default URLs
        if (defaultURLs.Length > 0)
        {
            //Check PlayerPrefs to see if list already has been created
            if (PlayerPrefs.HasKey(playerPrefKey))
            {
                string[] existingURLs = PlayerPrefExtension.Instance.GetStringArray(playerPrefKey);
                List<string> newURLs = existingURLs.ToList();

                //Add all new unique URLs to the list
                foreach (string url in defaultURLs)
                {
                    if (!existingURLs.Contains(url))
                    {
                        newURLs.Add(url);
                    }
                }

                //Update PlayerPrefs
                PlayerPrefExtension.Instance.SetStringArray(playerPrefKey, newURLs.ToArray());
            }
            else
            {
                //PlayerPrefs does not contain any file URLs, so we create a new PlayerPref list
                PlayerPrefExtension.Instance.SetStringArray(playerPrefKey, defaultURLs);
            }
        }

        //Get list of file URLs
        fileURLs = PlayerPrefExtension.Instance.GetStringArray(playerPrefKey);
    }

    private void Update()
    {
        //WARNING: THIS REMOVES PLAYERPREFS DATA ABOUT SONG URLS
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            //Debug.Log("NUKING PLAYERPREFS FOR KEY: " + playerPrefKey);
            //PlayerPrefs.DeleteKey(playerPrefKey);
        }
    }

    /// <summary>
    /// Opens the file explorer according to the mode provided. Defaults to read mode.
    /// </summary>
    /// <param name="fileBrowserMode">File browser mode.</param>
    private void OpenFileBrowser(FileBrowserMode fileBrowserMode = FileBrowserMode.Read)
    {
        //Create the file browser and name it
        GameObject fileBrowserObject = Instantiate(fileBrowserPrefab, transform);
        fileBrowserObject.name = "FileBrowser";

        //Set file explorer orientation
        FileBrowser fileBrowserScript = fileBrowserObject.GetComponent<FileBrowser>();
        fileBrowserScript.SetupFileBrowser(orientation);

        //Setup file explorer according to mode
        if (fileBrowserMode == FileBrowserMode.Write)
        {
            fileBrowserScript.SaveFilePanel("DemoText", fileExtensions);

            // Subscribe to OnFileSelect event
            fileBrowserScript.OnFileSelect += SaveFileUsingPath;
        }
        else
        {
            fileBrowserScript.OpenFilePanel(fileExtensions);

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

            //Save URL to a list of songs
            //URLs should all be unique
            if(!PlayerPrefExtension.Instance.ExistsWithinKey(playerPrefKey, path))
            {
                //Add new URL into list
                List<string> urls = new List<string>();
                urls = PlayerPrefExtension.Instance.GetStringArray(playerPrefKey).ToList();
                urls.Add(path);
                //Save updated list to PlayerPrefs
                PlayerPrefExtension.Instance.SetStringArray(playerPrefKey, urls.ToArray());
                //Get list of file URLs
                fileURLs = PlayerPrefExtension.Instance.GetStringArray(playerPrefKey);
            }
        }
        else
        {
            Debug.Log("Invalid path given");
        }
    }
}
