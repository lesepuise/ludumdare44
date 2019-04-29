using UnityEngine;
using System.Collections;

namespace CleverCode
{
    public class Follower : MonoBehaviour
    {
        public Transform toFollow;

        public bool detach;
        public bool follow;

        public bool persist;
        public bool ignoreY;
        //public bool faceCamera;

        public bool keepStartingOffset;
        public Vector3 offset;

        public float rotateSpeed;
        public float rotateAroundSpeed;

        private bool _Initialised;

        void Start()
        {
            Init();
        }

        //Messaged
        void ResetOffset()
        {
            if (toFollow)
            {
                offset = transform.position - toFollow.position;
            }
            else
            {
                offset = transform.position - transform.parent.position;
            }
        }

        public void Init()
        {
            if (_Initialised) return;
            _Initialised = true;

            if (follow && !toFollow)
            {
                toFollow = transform.parent;
            }

            if (keepStartingOffset && toFollow)
            {
                offset = transform.position - toFollow.position;
            }

            if (detach && transform.parent)
            {
                transform.parent = transform.parent.parent;
            }
        }

        void OnDisable()
        {
            _Initialised = false;
        }

        void LateUpdate()
        {
            if (!_Initialised) Init();

            if (follow && !toFollow)
            {
                if (!persist)
                    Destroy(gameObject);

                return;
            }

            float timeRatio = 1f;

            //if (faceCamera) transform.forward = scr_GOList.Instance.GetCameraScript().GetVectorFromTarget();

            if (rotateAroundSpeed > 0)
            {
                offset = Quaternion.Euler(0f, rotateAroundSpeed * Time.deltaTime * timeRatio, 0f) * offset;
            }

            if (rotateSpeed > 0)
            {
                transform.Rotate(0f, rotateSpeed * Time.deltaTime * timeRatio, 0f);
            }

            if (follow)
            {
                if (ignoreY)
                {
                    Vector3 pos = toFollow.position;
                    pos.y = transform.position.y;
                    transform.position = pos + offset;
                }
                else
                    transform.position = toFollow.position + offset;
            }
        }
    }
}