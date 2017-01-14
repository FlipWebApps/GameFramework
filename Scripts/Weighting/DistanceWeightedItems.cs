//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//
// Please direct any bugs/comments/suggestions to http://www.flipwebapps.com
// 
// The copyright owner grants to the end user a non-exclusive, worldwide, and perpetual license to this Asset
// to integrate only as incorporated and embedded components of electronic games and interactive media and 
// distribute such electronic game and interactive media. End user may modify Assets. End user may otherwise 
// not reproduce, distribute, sublicense, rent, lease or lend the Assets. It is emphasized that the end 
// user shall not be entitled to distribute or transfer in any way (including, without, limitation by way of 
// sublicense) the Assets in any other way than as integrated components of electronic games and interactive media. 

// The above copyright notice and this permission notice must not be removed from any files.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//----------------------------------------------

using GameFramework.Debugging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

namespace GameFramework.Weighting {
    /// <summary>
    /// Used for determining the relating weighting between items over a given distance (or time).
    /// This can be used to control things such as what is displayed, behavious, or other such things.
    /// 
    /// 1. Call AddItem() on all items you are weighting passing a list of DistanceWeight values
    /// 2. Call PrepareForUse() to sync weights.
    /// 3. Call GetItemForDistance() to get a random, weighted item based upon the distance (or time).
    /// 
    /// </summary>
    /// <typeparam name="T">The type of the items that we are putting a weight upon</typeparam>
    public class DistanceWeightedItems<T>
    {
        // count of items added.
        public int ItemCount { get; set; }

        // A list of all passed items that we are weighting.
        readonly List<DistanceWeightedItem> _items = new List<DistanceWeightedItem>();

        // Master list of all distances with weights for each item.
        List<DistanceWithWeights> _distancesWithWeights = new List<DistanceWithWeights>();

        // Set once everything is setup and ready for retreiving weighted items.
#pragma warning disable 414
        bool _isPreparedForUse = false;
#pragma warning restore 414

        /// <summary>
        /// Add an item and update the master list with all distances.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="distanceWeights"></param>
        public bool AddItem(T item, List<DistanceWeightValue> distanceWeights)
        {
            if (distanceWeights == null || distanceWeights.Count == 0) return false;

            _items.Add(new DistanceWeightedItem() { Item = item, DistanceWeights = distanceWeights });
            ItemCount++;

            // add all distance weights passed in
            foreach (var distanceWeight in distanceWeights)
            {
                if (_distancesWithWeights.FirstOrDefault(t => t.Distance == distanceWeight.Distance) == null)
                    _distancesWithWeights.Add(new DistanceWithWeights() { Distance = distanceWeight.Distance });
            }

            return true;
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
                    var distanceWeightValue = item.DistanceWeights.FirstOrDefault(t => t.Distance == distanceWithWeights.Distance);
                    var itemWeight = distanceWeightValue != null ? distanceWeightValue.Weight : lastWeight;

                    // add to end of item weights list (this will maintain the same ordering as in _items for later)
                    distanceWithWeights.ItemWeights.Add(itemWeight);
                    distanceWithWeights.Total += itemWeight;
                    lastWeight = itemWeight;
                }
            }

#if UNITY_EDITOR
            // sanity check
            for (var j = 0; j < _distancesWithWeights.Count; j++)
            {
                var distanceWithWeights = _distancesWithWeights[j];
                if (distanceWithWeights.Total == 0)
                    MyDebug.LogWarningF("All Totals are 0 for distance {0}. This might give unexpected results.", distanceWithWeights.Distance);
            }
#endif

            _isPreparedForUse = true;
        }


        /// <summary>
        /// For the specified distance, return a random item taking into consideration the current weighting.
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        public T GetItemForDistance(int distance)
        {
            Assert.IsTrue(_isPreparedForUse, "You must call PrepareForUse before trying to get values.");

            // loop to find matching distance.
            var distanceWithWeights = GetAssociatedDistance(distance);

            // get item based upon weight
            var weight = Random.Range(0, distanceWithWeights.Total + 1);
            return _items[GetIndexFromWeights(distanceWithWeights.ItemWeights, weight)].Item;
        }


        /// <summary>
        /// Get the item that is associated with the specified distance.
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        public DistanceWithWeights GetAssociatedDistance(int distance)
        {
            var selectedDistanceWithWeights = _distancesWithWeights[0];
            for (var i = 0; i < _distancesWithWeights.Count; i++)
            {
                if (distance < _distancesWithWeights[i].Distance)
                    break;

                selectedDistanceWithWeights = _distancesWithWeights[i];
            }

            return selectedDistanceWithWeights;
        }


        /// <summary>
        /// Given a list of individual item weights and a target weight value, return the index that corresponds 
        /// to teh accumulated item weights that meet target weight.
        /// </summary>
        /// <param name="itemWeights"></param>
        /// <param name="targetWeight"></param>
        /// <returns></returns>
        public int GetIndexFromWeights(List<int> itemWeights, int targetWeight)
        {
            var weightCounter = 0;
            for (int i = 0; i < itemWeights.Count; i++)
            {
                var itemWeight = itemWeights[i];
                if (itemWeight != 0)
                {
                    weightCounter += itemWeight;
                    //Debug.Log(weightCounter + ", " + weight);
                    if (weightCounter >= targetWeight)
                    {
                        {
                            return i;
                        }
                    }
                }
            }

            // should never happen as we should always get a match! We warn about case where all are 0 earlier, warn on others here.
            if (weightCounter != 0)
            {
                MyDebug.LogWarningF("No match for weight {0}. Please report this error as this should never happen!",
                    targetWeight);
                MyDebug.LogWarningF(ToString());
            }
            return 0;
        }


        /// <summary>
        /// Get all distances that have been added
        /// </summary>
        /// <returns></returns>
        public List<int> GetDistances()
        {
            return _distancesWithWeights.Select(t => t.Distance).ToList();
        }


        /// <summary>
        /// Get the total weight for the specified distanse
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        public int GetDistanceTotalWeight(int distance)
        {
            var distanceWithWeights = _distancesWithWeights.FirstOrDefault(t => t.Distance == distance);
            return distanceWithWeights == null ? -1 : distanceWithWeights.Total;
        }


        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var j = 0; j < _distancesWithWeights.Count; j++)
            {
                var distanceWithWeights = _distancesWithWeights[j];
                sb.AppendFormat("Distance: {0}, Total: {1}, Values:", distanceWithWeights.Distance, distanceWithWeights.Total);
                for (var i = 0; i < _items.Count; i++)
                {
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

        public class DistanceWithWeights
        {
            public int Distance;
            public int Total;
            public List<int> ItemWeights = new List<int>();
        }
    }
}