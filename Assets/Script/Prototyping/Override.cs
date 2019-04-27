using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace CleverCode
{
    public static class MyExtensions
    {
        public static Coroutine Invoke(this MonoBehaviour monoBehaviour, System.Action action, float time)
        {
            return monoBehaviour.StartCoroutine(InvokeImpl(action, time));
        }

        private static IEnumerator InvokeImpl(System.Action action, float time)
        {
            yield return new WaitForSeconds(time);

            action();
        }

        #region Transform & Vector

        public static float SqrMagnitudeWith(this Transform tr, Vector3 target)
        {
            return tr.position.SqrMagnitudeWith(target);
        }

        public static float SqrMagnitudeWith(this Vector3 from, Vector3 to)
        {
            return (from - to).sqrMagnitude;
        }

        public static float XzSqrMagnitudeWith(this Transform tr, Vector3 target)
        {
            return tr.position.XzSqrMagnitudeWith(target);
        }

        public static float XzSqrMagnitudeWith(this Vector3 from, Vector3 to)
        {
            return (from - to).XzSqrMagnitude();
        }

        public static float XzSqrMagnitude(this Vector3 v)
        {
            return v.x * v.x + v.z * v.z;
        }

        public static float XzMagnitude(this Vector3 v)
        {
            return Mathf.Sqrt(v.XzSqrMagnitude());
        }


        public static float XySqrMagnitudeWith(this Transform tr, Vector3 target)
        {
            return tr.position.XySqrMagnitudeWith(target);
        }

        public static float XySqrMagnitudeWith(this Vector3 from, Vector3 to)
        {
            return (from - to).XySqrMagnitude();
        }

        public static float XySqrMagnitude(this Vector3 v)
        {
            return v.x * v.x + v.y * v.y;
        }

        public static float XyMagnitude(this Vector3 v)
        {
            return Mathf.Sqrt(v.XySqrMagnitude());
        }

        public static void SetLocalScaleX(this Transform t, float value)
        {
            Vector3 scale = t.localScale;
            scale.x = value;
            t.localScale = scale;
        }

        public static void SetLocalScaleY(this Transform t, float value)
        {
            Vector3 scale = t.localScale;
            scale.y = value;
            t.localScale = scale;
        }

        public static void SetLocalScaleZ(this Transform t, float value)
        {
            Vector3 scale = t.localScale;
            scale.z = value;
            t.localScale = scale;
        }

        #endregion

        #region Angle and Rotation

        /// <summary>
        /// Rotate toward TARGET in Y at a max of RATE percent
        /// </summary>
        /// <param name="target">transform to look at</param>
        /// <param name="rate">percent of the angle to turn</param>
        public static void RotateTowardPercent(this Transform tr, Vector3 target, float rate)
        {
            Vector3 toTargetVector = target - tr.position;
            toTargetVector.y = 0f;
            float dist = toTargetVector.magnitude;

            if (dist <= 0.001f) return;

            toTargetVector /= dist;

            float currentAngle = Mathf.Acos(Mathf.Clamp(Vector3.Dot(tr.forward, toTargetVector), -1, 1)) * 57.2958f;

            //Right hand rule to check if the direction is to the right or to the left;
            float crossY = Vector3.Cross(tr.forward, toTargetVector).y > 0 ? 1f : -1f;
            float rotationAngle = currentAngle * rate;

            tr.Rotate(Vector3.up * crossY, rotationAngle);
        }

        /// <summary>
        /// Rotate toward TARGET in Y at a max of RATE degree
        /// </summary>
        /// <param name="target">transform to look at</param>
        /// <param name="rate">max angular movement in degrees per seconds</param>
        public static void RotateToward(this Transform tr, Vector3 target, float rate)
        {
            Vector3 toTargetVector = target - tr.position;
            toTargetVector.y = 0f;
            float dist = toTargetVector.magnitude;

            if (dist <= 0.001f) return;

            toTargetVector /= dist;

            float currentAngle = Mathf.Acos(Mathf.Clamp(Vector3.Dot(tr.forward, toTargetVector), -1, 1)) * 57.2958f;

            //Right hand rule to check if the direction is to the right or to the left;
            float crossY = Vector3.Cross(tr.forward, toTargetVector).y > 0 ? 1f : -1f;

            if (currentAngle > rate) tr.Rotate(Vector3.up * crossY, rate);
            else tr.Rotate(Vector3.up * crossY, currentAngle);
        }

        /// <summary>
        /// Rotate toward TARGET in Y only
        /// </summary>
        /// <param name="target">transform to look at</param>
        public static void RotateToward(this Transform tr, Vector3 target)
        {
            tr.RotateToward(target, 180f);
        }

        public static float AngleWith(this Transform tr, Vector3 target)
        {
            Vector3 toTargetVector = target - tr.position;
            toTargetVector.y = 0f;
            toTargetVector = toTargetVector.normalized;

            float currentAngle = Mathf.Acos(Mathf.Clamp(Vector3.Dot(tr.forward, toTargetVector), -1, 1)) * 57.2958f;

            //Right hand rule to check if the direction is to the right or to the left;
            float crossY = Vector3.Cross(tr.forward, toTargetVector).y > 0 ? 1f : -1f;
            return currentAngle * (crossY > 0 ? 1 : -1);
        }

        /// <summary>
        /// Angle in Y from Vector.Forward
        /// </summary>
        public static float Angle(this Vector3 vector)
        {
            vector = vector.normalized;

            float currentAngle = Mathf.Acos(Mathf.Clamp(Vector3.Dot(Vector3.forward, vector), -1, 1)) * 57.2958f;

            //Right hand rule to check if the direction is to the right or to the left;
            float crossY = Vector3.Cross(Vector3.forward, vector).y > 0 ? 1f : -1f;
            return currentAngle * (crossY > 0 ? 1 : -1);
        }

        /// <summary>
        /// Angle from Vector.Forward
        /// </summary>
        public static float AngleWith(this Vector3 forward, Vector3 vector)
        {
            forward = forward.normalized;
            vector = vector.normalized;

            float currentAngle = Mathf.Acos(Mathf.Clamp(Vector3.Dot(forward, vector), -1, 1)) * 57.2958f;

            //Right hand rule to check if the direction is to the right or to the left;
            float crossY = Vector3.Cross(forward, vector).y > 0 ? 1f : -1f;
            currentAngle *= (crossY > 0 ? 1 : -1);
            while (currentAngle > 180) currentAngle -= 360f;
            while (currentAngle < -180) currentAngle += 360f;

            return currentAngle;
        }

        public static void SetLocalRotationX(this Transform t, float value)
        {
            Vector3 rotation = t.localEulerAngles;
            rotation.x = value;
            t.localEulerAngles = rotation;
        }

        public static void SetLocalRotationY(this Transform t, float value)
        {
            Vector3 rotation = t.localEulerAngles;
            rotation.y = value;
            t.localEulerAngles = rotation;
        }

        public static void SetLocalRotationZ(this Transform t, float value)
        {
            Vector3 rotation = t.localEulerAngles;
            rotation.z = value;
            t.localEulerAngles = rotation;
        }

        #endregion

        #region Enum Functions

        /// <summary>
        /// Transform a variable x from 0 to 1 and get the y of the curve
        /// </summary>
        public static float Get(this CurveType curve, float x)
        {
            x = Mathf.Clamp01(x);
            switch (curve)
            {
                case CurveType.Sqrt:
                    return Mathf.Sqrt(x);

                case CurveType.FakeSqrt: //Similar to a square root, but easier on the cpu
                    return 1 - (1 - x) * (1 - x);

                case CurveType.Linear:
                    return x;

                case CurveType.Squared:
                    return x * x;

                case CurveType.SquaredEaseOut:
                    x = 1 - x;
                    return 1 - x * x;

                case CurveType.Cubed:
                    return x * x * x;

                case CurveType.CubedEaseOut:
                    x = 1 - x;
                    return 1 - x * x * x;

                case CurveType.Parabola:
                    return (x - x * x) * 4f;

                case CurveType.SmoothLinear: //Close to linear but with smooth start and end
                    return (3 * x * x - 2 * x * x * x); //3x² - 2x³

                case CurveType.SmoothEnd: //Close to linear but with smooth end
                    return (x + x * x - x * x * x); //x + x² - x³

                default:
                    Debug.Log("Unknown Curve Type");
                    return x;
            }
        }

        public static float DeltaTime(this TimeType timeType)
        {
            switch (timeType)
            {
                case TimeType.Normal:
                    return UnityEngine.Time.deltaTime;

                case TimeType.Unscaled:
                    return UnityEngine.Time.unscaledDeltaTime;

                case TimeType.Fixed:
                    return UnityEngine.Time.fixedDeltaTime;

                default:
                    throw new NotImplementedException();
            }
        }

        public static float Time(this TimeType timeType)
        {
            switch (timeType)
            {
                case TimeType.Normal:
                    return UnityEngine.Time.time;

                case TimeType.Unscaled:
                    return UnityEngine.Time.unscaledTime;

                case TimeType.Fixed:
                    return UnityEngine.Time.fixedTime;

                default:
                    throw new NotImplementedException();
            }
        }

        public static Vector3 Vector(this Axe axe)
        {
            switch (axe)
            {
                case Axe.X:
                    return Vector3.right;

                case Axe.Y:
                    return Vector3.up;

                case Axe.Z:
                    return Vector3.forward;

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion

        #region Random and List

        private static Random _random = new Random();
        
        /// <summary>
        /// Semi Random Fast Shuffle
        /// </summary>
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = _random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        /// Remove any duplicate in the list
        /// </summary>
        public static void RemoveDuplicate<T>(this IList<T> list)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {
                for (int j = i + 1; j < list.Count; j++)
                {
                    if (Equals(list[i], list[j]))
                    {
                        list.RemoveAt(j);
                        j--;
                    }
                }
            }
        }

        /// <summary>
        /// Return any element at random
        /// </summary>
        public static T RandomElement<T>(this IList<T> list)
        {
            if (list.Count == 0)
            {
                return default(T);
            }

            int choice = _random.Next(list.Count);

            return list[choice];
        }

        /// <summary>
        /// Return and remove any element at random
        /// </summary>
        public static T RandomPop<T>(this IList<T> list)
        {
            if (list.Count == 0)
            {
                Debug.LogError("No element in list, returning default");
                return default(T);
            }

            int choice = _random.Next(list.Count);

            T element = list[choice];
            list.RemoveAt(choice);
            return element;
        }

        public static T LastElement<T>(this IList<T> list)
        {
            if (list.Count == 0)
            {
                return default(T);
            }

            return list[list.Count - 1];
        }

        public static T PopLast<T>(this IList<T> list)
        {
            if (list.Count == 0)
            {
                return default(T);
            }

            T last = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            return last;
        }

        public static T RandomEnumElement<T>(this Type enumType)
        {
            string[] names = Enum.GetNames(enumType);
            string choice = names.RandomElement();
            return (T)Enum.Parse(enumType, choice);
        }

        #endregion

        #region String Manipulation

        public static string Fuse(this List<string> strList, string separator = "\n", bool ignoreEmptyEntries = true)
        {
            string linkedString = "";

            for (int i = 0; i < strList.Count; i++)
            {
                if (ignoreEmptyEntries && string.IsNullOrEmpty(strList[i]))
                {
                    continue;
                }

                if (linkedString.Length > 0)
                {
                    linkedString += separator;
                }

                linkedString += strList[i];
            }

            return linkedString;
        }

        public static string UpperFirstCharacter(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            if (str.Length > 1)
            {
                return char.ToUpper(str[0]) + str.Substring(1);
            }

            return str.ToUpper();
        }

        public static string SmallCaps(this string str, int size)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            str = str.ToUpperInvariant();

            int addedSize = 14;
            addedSize += size.ToString().Length;

            str = SetRichSize(str, 0, size);

            for (int i = 1; i < str.Length - 1; i++)
            {
                if (str[i] == ' ')
                {
                    str = SetRichSize(str, i + 1, size);
                    i += addedSize;
                }
            }

            return str;
        }

        public static string SetRichSize(string str, int index, int size)
        {
            string newString = str.Substring(0, index);

            newString += "<size=" + size + ">";
            newString += str[index];
            newString += "</size>";
            newString += str.Substring(index + 1, str.Length - index - 1);

            return newString;
        }

        #endregion

        public static void SetColorAlpha(this Image img, float value)
        {
            Color c = img.color;
            c.a = value;
            img.color = c;
        }

        public static bool IsInRange(this Vector2 vector, float value)
        {
            return value >= vector[0] && value <= vector[1];
        }

        public static float LerpRange(this Vector2 v, float t)
        {
            return Mathf.Lerp(v.x, v.y, t);
        }
    }
}