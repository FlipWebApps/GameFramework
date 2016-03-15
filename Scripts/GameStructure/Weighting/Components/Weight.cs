//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.GameStructure.Weighting.Components {
    public class Weight : MonoBehaviour
    {
        public DistanceWeight[] Weights;

        public int GetWeight(int distance)
        {
            int lastWeight = 0;
            for (int i = 0; i < Weights.Length; i++)
            {
                if (Weights[i].Distance > distance)
                {
                    break;
                }
                lastWeight = Weights[i].Weight;
            }
            return lastWeight;

            //for (int i = 1; i < Weights.Length; i++)
            //{
            //    if (Weights[i].Distance > distance)
            //    {
            //        return Weights[i - 1].Weight;
            //    }
            //}
            //// default return the first (last)
            //if (Weights.Length > 0 && Weights[Weights.Length - 1].Weight >= distance)
            //    return Weights[Weights.Length - 1].Weight;
            //else
            //    return 0;
        }

        [System.Serializable]
        public class DistanceWeight
        {
            public int Distance;
            public int Weight;
        }
    }
}