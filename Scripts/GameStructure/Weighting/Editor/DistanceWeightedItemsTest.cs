using UnityEngine;
using UnityEditor;
using NUnit.Framework;
using FlipWebApps.GameFramework.Scripts.GameStructure.Weighting;
using System.Collections.Generic;
using System.Linq;

public class DistanceWeightedItemsTest {


    [Test]
    public void DistancesSavedAndUnique(int itemCount)
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

    //TODO - can only test where we know to expect a fixed value back
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

        // Assert
        Assert.AreEqual(result, distanceWeightedItems.GetItemForDistance(distance));
    }
}
