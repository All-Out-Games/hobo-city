using AO;

public class Singleton<T> where T : Component
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                foreach (var component in Scene.Components<T>())
                {
                    _instance = component;
                    _instance.Awaken();
                    break;
                }
            }
            return _instance;
        }
    }
}
