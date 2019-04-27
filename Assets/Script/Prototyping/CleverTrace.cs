using UnityEngine;
using System.Collections;

namespace CleverCode
{
    public class CleverTrace : MonoBehaviour
    {
        public bool debugAwake = true;
        public bool debugStart;
        public bool debugEnable;
        public bool debugDisable = true;
        public bool debugDestroy;

        void Awake()
        {
            if (debugAwake)
                Debug.Log("Trace : " + name + " - Awake", gameObject);
        }

        void Start()
        {
            if (debugStart)
                Debug.Log("Trace : " + name + " - Start", gameObject);
        }

        void OnEnable()
        {
            if (debugEnable)
                Debug.Log("Trace : " + name + " - Enable", gameObject);
        }

        void OnDisable()
        {
            if (debugDisable)
                Debug.Log("Trace : " + name + " - Disable", gameObject);
        }

        void OnDestroy()
        {
            if (debugDestroy)
                Debug.Log("Trace : " + name + " - Destroy", gameObject);
        }
    }
}