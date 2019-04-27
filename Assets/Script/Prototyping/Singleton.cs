using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CleverCode
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        protected void SetSingleton(T newInstance)
        {
            if (Instance != null)
            {
                Debug.LogError("Singleton for " + typeof(T) + "already exist, destroying the old one");
                Destroy(Instance.gameObject);
            }

            Instance = newInstance;
        }

        protected virtual void OnDestroy()
        {
            Instance = null;
        }

        protected void Awake()
        {
            SetSingleton(this as T);

            Init();
        }

        protected virtual void Init() { }
    }
    
    public abstract class TrueSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    CreateSingleton();
                }
                return _instance;
            }
        }

        private static void CreateSingleton()
        {
            GameObject singleton = new GameObject(typeof(T) + " - Singleton");
            _instance = singleton.AddComponent<T>();

            DontDestroyOnLoad(singleton);
        }
    }
}