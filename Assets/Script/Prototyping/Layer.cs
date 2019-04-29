using UnityEngine;
using System.Collections;

namespace CleverCode
{
    public static class Layer
    {
        public static int Ball { get; private set; }
        public static int OuterBall { get; private set; }
        public static int Obstacle { get; private set; }
        public static int Scenery { get; private set; }

        public static void Init()
        {
            Ball = LayerMask.NameToLayer("Ball");
            OuterBall = LayerMask.NameToLayer("OuterBall");
            Obstacle = LayerMask.NameToLayer("Obstacle");
            Scenery = LayerMask.NameToLayer("Scenery");
        }
    }
}