using UnityEngine;
using UnityEngine.Analytics;

namespace CleverCode.Animation
{
    public class CleverRotate : MonoBehaviour
    {
        public float rotationSpeed = 40f;
        public Axe rotationAxe = Axe.Y;
        public TimeType time;
        public bool randomRotationDirection;
        public bool detachAndFollow;
        private Transform _ToFollow;

        public bool globalAxe;
        public bool late;

        private void OnEnable()
        {
            if (randomRotationDirection && Random.value < 0.5f)
            {
                rotationSpeed = -rotationSpeed;
            }
        }

        private void Update()
        {
            if (!late)
            {
                Rotate();
            }
        }

        private void Start()
        {
            if (detachAndFollow)
            {
                _ToFollow = transform.parent;
                if (_ToFollow)
                    transform.parent = _ToFollow.parent;
            }
        }

        private void LateUpdate()
        {
            if (late)
            {
                Rotate();
            }

            if (detachAndFollow)
            {
                transform.position = _ToFollow.transform.position;
            }
        }

        private void Rotate()
        {
            float speed = rotationSpeed;

            speed *= Time.deltaTime;

            Vector3 axeVector = rotationAxe.Vector();

            if (globalAxe)
            {
                axeVector = transform.TransformDirection(axeVector);
            }

            transform.Rotate(axeVector, speed);
        }
    }
}
