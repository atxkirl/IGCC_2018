using UnityEngine;

/// <summary>
/// Abstract class for creating Singleton classes with Monobehaviour. Usage: Replace 'T' with the class name.
/// </summary>
public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            //Try to find an existing instance if necessary
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(T)) as T;
            }

            //If the instance is still null, create a new instance of object
            if (!_instance)
            {
                GameObject go = new GameObject();
                _instance = go.AddComponent<T>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance)
        {
            Destroy(this);
        }
    }

    //This literally does nothing except reference the object so that you can set it inactive and still have Instance refer to this.
    public void CreateReference()
    {
        Debug.Log("Reference made.");
    }
}