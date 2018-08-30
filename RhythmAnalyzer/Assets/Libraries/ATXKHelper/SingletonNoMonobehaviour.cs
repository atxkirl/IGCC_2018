public abstract class SingletonNoMonoBehaviour<T> where T : SingletonNoMonoBehaviour<T>, new()
{
    private static T _instance = new T();

    public static T Instance
    {
        get
        {
            return _instance;
        }
    }
}