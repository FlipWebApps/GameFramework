//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

namespace FlipWebApps.GameFramework.Scripts.GameStructure.Weighting {
    /// <summary>
    /// Simple class for holding Distance / Weight values.
    /// </summary>
    [System.Serializable]
    public class DistanceWeightValue
    {
        public int Distance;
        public int Weight;

        public DistanceWeightValue(int distance, int weight)
        {
            Distance = distance;
            Weight = weight;
        }

    }
}