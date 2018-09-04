using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerPrefExtension : SingletonNoMonoBehaviour<PlayerPrefExtension>
{
    private enum ArrayType
    {
        Float, 
        Int32, 
        Bool,
        String,
        Vector2,
        Vector3,
        Quaternion,
        Color
    }

    private int endianDiff1 = 0;
    private int endianDiff2 = 0;
    private int index = 0;
    private byte[] byteBlock;

    private void Initialize()
    {
        if(System.BitConverter.IsLittleEndian)
        {
            endianDiff1 = 0;
            endianDiff2 = 0;
        }
        else
        {
            endianDiff1 = 3;
            endianDiff2 = 1;
        }

        if (byteBlock == null)
            byteBlock = new byte[4];

        index = 1;
    }

    //Setters

    /// <summary>
    /// Saves a string array to Unity PlayerPrefs.
    /// </summary>
    /// <param name="playerPrefKey">Identifier key for PlayerPrefs.</param>
    /// <param name="stringArray">String array to save.</param>
    public bool SetStringArray(string playerPrefKey, string[] stringArray)
    {
        byte[] bytes = new byte[stringArray.Length + 1];
        bytes[0] = System.Convert.ToByte(ArrayType.String); //Array type identifier

        Initialize();

        //Store the length of each string in the array so that we can extract the correct data later on
        foreach(string str in stringArray)
        {
            if(str == null)
            {
                Debug.LogError("Null entries not allowed when saving to: " + playerPrefKey);
                return false;
            }
            if(str.Length > 255)
            {
                Debug.LogError("Strings cannot be longer than 255 characters when saving to: " + playerPrefKey);
                return false;
            }
            bytes[index++] = (byte)str.Length;
        }

        //Concantonate the entire array into a single string and save it into PlayerPref
        try
        {
            string finalString = System.Convert.ToBase64String(bytes) + "|" + string.Join("", stringArray);
            PlayerPrefs.SetString(playerPrefKey, finalString);
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.ToString());
            return false;
        }

        return true;
    }

    /// <summary>
    /// Saves a string list to Unity PlayerPrefs.
    /// </summary>
    /// <param name="playerPrefKey">Identifier key for PlayerPrefs.</param>
    /// <param name="stringArray">String list to save.</param>
    public bool SetStringArray(string playerPrefKey, List<string> stringArray)
    {
        return SetStringArray(playerPrefKey, stringArray.ToArray());
    }

    //Getters

    /// <summary>
    /// Extracts all the strings from PlayerPref using the given key, and returns a string array containing all the strings.
    /// </summary>
    /// <param name="playerPrefKey">Identifier key for PlayerPrefs.</param>
    public string[] GetStringArray(string playerPrefKey)
    {
        if(!PlayerPrefs.HasKey(playerPrefKey))
        {
            Debug.Log("PlayerPrefs does not contain key: " + playerPrefKey);
            return new string[0];
        }
        else
        {
            string completeString = PlayerPrefs.GetString(playerPrefKey);
            int seperatorIndex = completeString.IndexOf("|", 0);

            if(seperatorIndex < 4)
            {
                Debug.LogError("PlayerPref file '" + playerPrefKey + "' is corrupted.");
                return new string[0];
            }
            byte[] bytes = System.Convert.FromBase64String(completeString.Substring(0, seperatorIndex));
            if((ArrayType)bytes[0] != ArrayType.String)
            {
                Debug.LogError(playerPrefKey + " is not a string array.");
                return new string[0];
            }

            Initialize();

            int numberOfEntries = bytes.Length - 1;
            int stringIndex = seperatorIndex + 1;
            string[] stringArray = new string[numberOfEntries];
            for(int i = 0; i < numberOfEntries; ++i)
            {
                int stringLength = bytes[index++];
                if(stringIndex + stringLength > completeString.Length)
                {
                    Debug.LogError("PlayerPref file '" + playerPrefKey + "' is corrupted.");
                    return new string[0];
                }
                stringArray[i] = completeString.Substring(stringIndex, stringLength);
                stringIndex += stringLength;
            }
            return stringArray;
        }
    }

    //Existence Checks

    /// <summary>
    /// Checks if given value exists within a PlayerPref array using the given key.
    /// </summary>
    /// <param name="playerPrefKey">Identifier key for PlayerPrefs.</param>
    /// <param name="valueToCheck">Value to check for.</param>
    public bool ExistsWithinKey(string playerPrefKey, string valueToCheck)
    {
        if (!PlayerPrefs.HasKey(playerPrefKey))
        {
            Debug.Log("PlayerPrefs does not contain key: " + playerPrefKey);
            return false;
        }
        else
        {
            return GetStringArray(playerPrefKey).ToList().Contains(valueToCheck);
        }
    }
}
