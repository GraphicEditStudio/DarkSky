using UnityEngine;

namespace Utils
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static object _lockObject = new object();

        private static T instance;

        public static T Instance => instance;

        [SerializeField] private bool dontDestroyOnLoad = true;
        #region Unity Hooks

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            GetInstance();
            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
            OnAwake();
        }

        private void Start()
        {
            OnStart();
        }

        #endregion Unity Hooks

        protected virtual void OnAwake()
        {

        }

        protected virtual void OnStart()
        {

        }
        private static T GetInstance()
        {
            lock (_lockObject)
            {
                if (instance) return instance;

                instance = FindFirstObjectByType<T>();
                if (instance) return instance;

                GameObject go = new GameObject($"instance_{typeof(T)}", typeof(T));
                instance = go.GetComponent<T>();
                return instance;
            }
        }
    }
}