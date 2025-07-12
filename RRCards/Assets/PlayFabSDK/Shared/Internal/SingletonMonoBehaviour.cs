using UnityEngine;

namespace PlayFab.Internal
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        private static T _instance;

        public static T instance
        {
            get
            {
                CreateInstance();
                return _instance;
            }
        }

        public static void CreateInstance()
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    var go = new GameObject(typeof(T).Name);
                    _instance = go.AddComponent<T>();
                }
                if (!_instance.initialized)
                {
                    _instance.Initialize();
                    _instance.initialized = true;
                }
            }
        }

        public virtual void Awake ()
        {
            if (Application.isPlaying)
            {
                DontDestroyOnLoad(this);
            }
            if (_instance != null)
            {
                DestroyImmediate (gameObject);
            }
        }

        protected bool initialized;

        protected virtual void Initialize() { }
    }
}
