//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.Debugging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

namespace FlipWebApps.GameFramework.Scripts.GameStructure.Weighting {
    /// <summary>
    /// Used for determining the relating weighting between items over a given distance (or time).
    /// This can be used to control things such as what is displayed, behavious, or other such things.
    /// 
    /// 1. Call AddItem() on all items you are weighting passing a list of DistanceWeight values
    /// 2. Call PrepareForUsa() to sync weights.
    /// 3. Call GetItemForDistance() to get a random, weighted item based upon the distance (or time).
    /// 
    /// </summary>
    /// <typeparam name="T">The type of the items that we are putting a weight upon</typeparam>
    public class DistanceWeightedItems<T>
    {
        // A list of all passed items that we are weighting.
        List<DistanceWeightedItem> _items = new List<DistanceWeightedItem>();

        // Master list of all distances with weights for each item.
        List<DistanceWithWeights> _distancesWithWeights = new List<DistanceWithWeights>();

        // Set once everything is setup and ready for retreiving weighted items.
        bool isPreparedForUse = false;


        /// <summary>
        /// Add an item and update the master list with all distances.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="distanceWeights"></param>
        public void AddItem(T item, List<DistanceWeightValue> distanceWeights)
        {
            _items.Add(new DistanceWeightedItem() { Item = item, DistanceWeights = distanceWeights });

            // add all distance weights passed in
            foreach (var distanceWeight in distanceWeights)
            {
                if (_distancesWithWeights.FirstOrDefault(t => t.Distance == distanceWeight.Distance) == null)
                    _distancesWithWeights.Add(new DistanceWithWeights() { Distance = distanceWeight.Distance });
            }
        }


        /// <summary>
        /// Sync all items and distances so for each distance every item has a corresponding weight. We need this
        /// to be able to easily look up items for a given distance.
        /// </summary>
        public void PrepareForUse()
        {
            // sort distances
            _distancesWithWeights = _distancesWithWeights.OrderBy(t => t.Distance).ToList();

            // fill distances for all items
            for (var i = 0; i < _items.Count; i++)
            {
                var item = _items[i];

                // process for all distances.
                int lastWeight = 0;
                for (var j = 0; j < _distancesWithWeights.Count; j++)
                {
                    var distanceWithWeights = _distancesWithWeights[j];

                    // it this item has a matching distance setting then get matching weight from item, if not then use last weight.
                    int itemWeight;
                    var distanceWeightValue = item.DistanceWeights.FirstOrDefault(t => t.Distance == distanceWithWeights.Distance);
                    itemWeight = distanceWeightValue != null ? distanceWeightValue.Weight : lastWeight;

                    // add to end of item weights list (this will maintain the same ordering as in _items for later)
                    distanceWithWeights.ItemWeights.Add(itemWeight);
                    distanceWithWeights.Total += itemWeight;
                    lastWeight = itemWeight;
                }
            }

            isPreparedForUse = true;
        }


        /// <summary>
        /// For the specified distance, return a random item taking into consideration the current weighting.
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        public T GetItemForDistance(int distance)
        {
            Assert.IsTrue(isPreparedForUse, "You must call PrepareForUse before trying to get values.");

            // loop to find matching distance.
            var selectedDistanceWithWeights = _distancesWithWeights[0];
            for (var i = 0; i < _distancesWithWeights.Count; i++)
            {
                if (distance <= _distancesWithWeights[i].Distance)
                    break;

                selectedDistanceWithWeights = _distancesWithWeights[i];
            }

            // get item based upon weight
            int weight = Random.Range(0, selectedDistanceWithWeights.Total + 1);
            int weightCounter = 0;
            for (int i = 0; i < selectedDistanceWithWeights.ItemWeights.Count; i++)
            {
                var itemWeight = selectedDistanceWithWeights.ItemWeights[i];
                weightCounter += itemWeight;
                Debug.Log(weightCounter + ", " + weight);
                if (weightCounter >= weight)
                {
                    return _items[i].Item;
                }
            }

            // should never happen as we should always get a match!
            MyDebug.Log("No item found for given distance. Please report this error as this should never happen!");
            return default(T);
        }


        public List<int> GetDistances()
        {
            return _distancesWithWeights.Select(t => t.Distance).ToList();
        }


        public int GetDistanceTotalWeight(int distance)
        {
            return _distancesWithWeights.FirstOrDefault(t => t.Distance == distance).Total;
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (var i = 0; i < _items.Count; i++)
            {
                for (var j = 0; j < _distancesWithWeights.Count; j++)
                {
                    var distanceWithWeights = _distancesWithWeights[j];
                    sb.Append(distanceWithWeights.ItemWeights[i]);
                    sb.Append(", ");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }


        class DistanceWeightedItem
        {
            internal T Item;
            internal List<DistanceWeightValue> DistanceWeights;
        }

        class DistanceWithWeights
        {
            public int Distance;
            public int Total;
            public List<int> ItemWeights = new List<int>();
        }
    }
}