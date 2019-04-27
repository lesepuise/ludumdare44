using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CleverCode
{
    public static class CleverMath
    {
        /// <summary>
        /// Return an index randomly weighted by the size of each elements. Return -1 on an empty list or empty sum.
        /// </summary>
        public static int WeightedProbability(List<int> chanceList)
        {
            if (chanceList.Count == 0)
            {
                //Debug.LogError("Weighted list empty");
                return -1;
            }
            int sum = 0;
            for (int i = 0; i < chanceList.Count; i++)
            {
                sum += chanceList[i];
            }

            if (sum <= 0)
            {
                //Debug.LogError("Weighted list with 0 probability");
                return -1;
            }

            int pick = Random.Range(0, sum);

            for (int i = 0; i < chanceList.Count; i++)
            {
                if (pick < chanceList[i]) return i;
                pick -= chanceList[i];
            }

            Debug.LogError("Problem with weighted probability");
            return -1;
        }

        /// <summary>
        /// Return an index randomly weighted by the size of each elements. Return -1 on an empty list or empty sum.
        /// </summary>
        public static int WeightedProbability(List<float> chanceList)
        {
            if (chanceList.Count == 0)
            {
                //Debug.LogError("Weighted list empty");
                return -1;
            }
            float sum = 0;
            for (int i = 0; i < chanceList.Count; i++)
            {
                sum += chanceList[i];
            }

            if (sum <= 0)
            {
                //Debug.LogError("Weighted list with 0 probability");
                return -1;
            }

            float pick = Random.Range(0, sum);

            for (int i = 0; i < chanceList.Count; i++)
            {
                if (pick < chanceList[i]) return i;
                pick -= chanceList[i];
            }

            Debug.LogError("Problem with weighted probability");
            return -1;
        }

        /// <summary>
        /// Advance T toward Goal in totalTime, for a frame
        /// </summary>
        public static void TimedLerp(ref float t, float goal, float totalTime)
        {
            if (Math.Abs(t - goal) < 0.0001f)
            {
                t = goal;
                return;
            }

            float dir = goal > t ? 1 : -1;
            float diff = Mathf.Abs(goal - t);
            float movement = Time.deltaTime / totalTime;

            if (movement > diff)
            {
                t = goal;
            }
            else
            {
                t += movement * dir;
            }
        }
    }
}