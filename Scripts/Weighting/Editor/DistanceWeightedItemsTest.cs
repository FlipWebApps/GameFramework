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

using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace GameFramework.Weighting
{
    public class DistanceWeightedItemsTest
    {

        [Test]
        public void ItemWithoutWeightsNotAdded()
        {
            // Arrange
            var distanceWeightedItems = new DistanceWeightedItems<string>();
            distanceWeightedItems.AddItem("Item", new List<DistanceWeightValue>());

            // Assert
            Assert.AreEqual(0, distanceWeightedItems.ItemCount);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(4)]
        public void ItemCountLikeItemsAdded(int count)
        {
            // Arrange
            var distanceWeightedItems = new DistanceWeightedItems<string>();
            for (var i = 0; i < count; i++)
            {
                distanceWeightedItems.AddItem("Item" + i,
                    new List<DistanceWeightValue>() {
                new DistanceWeightValue(2, 10) });
            }

            // Assert
            Assert.AreEqual(count, distanceWeightedItems.ItemCount);
        }

        [Test]
        public void DistancesSavedAndUnique()
        {
            // Arrange
            var distanceWeightedItems = new DistanceWeightedItems<string>();

            // Act
            distanceWeightedItems.AddItem("Item1",
                new List<DistanceWeightValue>() {
                new DistanceWeightValue(1, 10),
                new DistanceWeightValue(2, 10) });
            distanceWeightedItems.AddItem("Item2",
                new List<DistanceWeightValue>() {
                new DistanceWeightValue(2, 10),
                new DistanceWeightValue(4, 10) });

            // Assert
            Assert.AreEqual(distanceWeightedItems.GetDistances().Distinct().Count(), distanceWeightedItems.GetDistances().Count);
        }

        [TestCase(1, 10)]
        [TestCase(2, 20)]
        [TestCase(4, 15)]
        public void AllDistancesSavedAndGapsFilled(int distance, int result)
        {
            // Arrange
            var distanceWeightedItems = new DistanceWeightedItems<string>();

            // Act
            distanceWeightedItems.AddItem("Item1",
                new List<DistanceWeightValue>() {
                new DistanceWeightValue(1, 10),
                new DistanceWeightValue(2, 10) });
            distanceWeightedItems.AddItem("Item2",
                new List<DistanceWeightValue>() {
                new DistanceWeightValue(2, 10),
                new DistanceWeightValue(4, 5) });
            distanceWeightedItems.PrepareForUse();

            // Assert
            Assert.AreEqual(result, distanceWeightedItems.GetDistanceTotalWeight(distance));
        }


        [TestCase(0, 1)]
        [TestCase(1, 1)]
        [TestCase(2, 2)]
        [TestCase(3, 2)]
        [TestCase(4, 4)]
        [TestCase(5, 4)]
        public void GetAssociatedItemForDistance(int distance, int result)
        {
            // Arrange
            var distanceWeightedItems = new DistanceWeightedItems<string>();
            distanceWeightedItems.AddItem("Item1",
                new List<DistanceWeightValue>() {
                new DistanceWeightValue(1, 10),
                new DistanceWeightValue(2, 10),
                new DistanceWeightValue(4, 0) });
            distanceWeightedItems.AddItem("Item2",
                new List<DistanceWeightValue>() {
                new DistanceWeightValue(2, 10),
                new DistanceWeightValue(4, 5) });
            distanceWeightedItems.PrepareForUse();

            // Act
            var associatedDistance = distanceWeightedItems.GetAssociatedDistance(distance).Distance;

            // Assert
            Assert.AreEqual(result, associatedDistance);
        }

        // total weight = 20
        [TestCase(0, 1)]
        [TestCase(5, 1)]
        [TestCase(8, 2)]
        [TestCase(15, 2)]
        [TestCase(16, 4)]
        [TestCase(18, 4)]
        [TestCase(20, 4)]
        public void GetIndexFromWeights(int targetWeight, int result)
        {
            // Arrange
            var distanceWeightedItems = new DistanceWeightedItems<string>();
            var weights = new List<int> { 0, 5, 10, 0, 5 };

            // Act
            var associatedDistance = distanceWeightedItems.GetIndexFromWeights(weights, targetWeight);

            // Assert
            Assert.AreEqual(result, associatedDistance);
        }

        //Note - can only test where we know to expect a fixed value back
        [TestCase(0, "Item1")]
        [TestCase(1, "Item1")]
        [TestCase(4, "Item2")]
        [TestCase(5, "Item2")]
        public void GetItemForDistance(int distance, string result)
        {
            // Arrange
            var distanceWeightedItems = new DistanceWeightedItems<string>();

            // Act
            distanceWeightedItems.AddItem("Item1",
                new List<DistanceWeightValue>() {
                new DistanceWeightValue(1, 10),
                new DistanceWeightValue(2, 10),
                new DistanceWeightValue(4, 0) });
            distanceWeightedItems.AddItem("Item2",
                new List<DistanceWeightValue>() {
                new DistanceWeightValue(2, 10),
                new DistanceWeightValue(4, 5) });
            distanceWeightedItems.PrepareForUse();
            //Debug.Log(distanceWeightedItems.ToString());

            // Assert
            Assert.AreEqual(result, distanceWeightedItems.GetItemForDistance(distance));
        }
    }
}