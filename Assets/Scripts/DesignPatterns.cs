using UnityEngine;

namespace DesignPatterns
{
    /// <summary>
    ///     Inherit to create a single, global-accessible instance of a class
    /// </summary>
    /// <typeparam name="T">Class to create a single, global-accessible instance from</typeparam>
    public class Singleton<T> : MonoBehaviour
        where T : MonoBehaviour
    {
        private static T _instance = null;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<T>();
                    if (_instance == null) // if no instance of class T is found then create one!
                    {
                        var singletonObj = new GameObject();
                        singletonObj.name = typeof(T).ToString();
                        _instance = singletonObj.AddComponent<T>();
                    }
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance != null) // if there exists already an instance of T, then destroy this GameObject!
            {
                Destroy(gameObject);
                return;
            }

            _instance = GetComponent<T>();
        }
    }

    /// <summary>
    ///     Inherit to create a single, global-accessible instance of a class
    ///     This singleton doesn't cause any errors when application is quiting.
    /// </summary>
    /// <typeparam name="T">Class to create a single, global-accessible instance from</typeparam>
    public class SafeSingleton<T> : MonoBehaviour
        where T : MonoBehaviour
    {
        private static T _instance = null;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<T>();
                return _instance;
            }
        }

        // when unsubscribing from events, make sure that Instance isn't null
        protected virtual void Awake()
        {
            _instance = GetComponent<T>();
        }
    }
}
