namespace Client.Runtime.Utils
{
    public class ServiceLocator<T>
    {
        private static T _instance;

        public static T Get()
        {
            return _instance;
        }

        public static void Add(T instance)
        {
            _instance = instance;
        }
    }
}