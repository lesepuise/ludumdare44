using System;
using UnityEngine;

namespace CleverCode.Animation
{
    public class CleverMoveSimple : MonoBehaviour
    {
        public Axe axe;
        public float totalMovement;
        public float duration;

        public bool useCurve;

        public AnimationCurve curve;
        public WrapMode curveWrap;

        private float _t;

        void Awake()
        {
            curve.postWrapMode = curveWrap;
        }

        void Update()
        {
            Vector3 old = EvaluateMovement(_t);

            _t += Time.deltaTime / duration;

            Vector3 @new = EvaluateMovement(_t);

            Vector3 diff = @new - old;

            transform.position += diff;
        }

        public Vector3 EvaluateMovement(float t)
        {
            float x;
            if (useCurve)
            {
                x = curve.Evaluate(t);
            }
            else
            {
                x = t;
            }

            x *= totalMovement;

            return axe.Vector() * x;
        }
    }
}