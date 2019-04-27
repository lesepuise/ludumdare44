using UnityEngine;
using System.Collections;

namespace CleverCode
{
    public class DestroyIn : MonoBehaviour
    {
        public float destroyIn;
        public bool ignoreTimeScale;

        private float _StartTime;

        void Start()
        {
            _StartTime = ignoreTimeScale ? Time.unscaledTime : Time.time;
        }

        void Update()
        {
            if (_StartTime + destroyIn < (ignoreTimeScale ? Time.unscaledTime : Time.time)) Destroy(gameObject);
        }
    }
}
