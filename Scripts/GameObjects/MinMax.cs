using System;

namespace FlipWebApps.GameFramework.Scripts.GameObjects
{
    [Serializable]
    public struct MinMax
    {
        public int Min;
        public int Max;
        public int Difference { get { return Max - Min; } }
    }

    [Serializable]
    public struct MinMaxf
    {
        public float Min;
        public float Max;
        public float Difference { get { return Max - Min; } }
    }
}