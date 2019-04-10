namespace NewEngine.Utils
{
    public class Singleton<T>
    {
        private static T sInstance = System.Activator.CreateInstance<T>();
        public static T Instance
        {
            get
            {
                return sInstance;
            }
        }
    }
}
