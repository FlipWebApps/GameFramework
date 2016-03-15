//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.GameStructure.Weighting { 
    public class WeightHelper
    {
//        public List<Ground> Grounds;
//        int TotalGroundWeighting;

//        public List<Obstacle> Obstacles;

//        List<Transform> AddedGroundTransforms;
//        float NextGroundTransformPosition = 0;
//        int GroundDisplayedCounter = -1;        // -1 for extra start space.

//        //List<Transform> GroundPrefabInstances;
//        //Transform BomPrefabInstance;

//        void Start()
//        {
//            //TODO ALLOCATE INSTANCES HERE AND USE THESE INSTEAD OF ALLOCATING DURING THE GAME.
//            // preallocate ground prefabs (3 of each)
//            //GroundPrefabInstances = new List<Transform>();
//            Grounds = new List<Ground>();
//            for (int i = 0; i < GroundPrefabs.Length; i++)
//            {
//                Transform prefab = GroundPrefabs[i];
//                Grounds.Add(prefab.GetComponent<Ground>());
//            //    for (int i = 0; i < 3; i++)
//            //    {
//            //        Transform newPrefab = (Transform)Instantiate(prefab);
//            //        newPrefab.transform.parent = this.transform;
//            //    }
//            }

//            //TODO ALLOCATE INSTANCES HERE AND USE THESE INSTEAD OF ALLOCATING DURING THE GAME.
//            Obstacles = new List<Obstacle>();
//            for (int i = 0; i < ObstaclePrefabs.Length; i++)
//            {
//                Transform prefab = ObstaclePrefabs[i];
//                Obstacles.Add(prefab.GetComponent<Obstacle>());
//            }

//            // setup floor.
//            AddedGroundTransforms = new List<Transform>();
//            NextGroundTransformPosition = 7;
//            CreateGround(11);
//        }


//        private void CreateGround(int count)
//        {
//            for (int i = 0; i < count; i++)
//            {
//                Transform newPrefab = CreateAndAddGround();

//                // create and add obstacle prefabs
//                CreateAndAddObstanclesToGround(newPrefab);

//                // update position.
//                NextGroundTransformPosition += newPrefab.renderer.bounds.size.z;
//            }
//        }

//        private Transform CreateAndAddGround()
//        {
//            // find the prefab that we want to add (no bom on 0).
//            Transform prefab = null;
//            if ((GroundDisplayedCounter) % BomPrefabSpacing == 0 && GroundDisplayedCounter > 0)
//            {
//                prefab = BomPrefab;
//            }
//            else
//            {
//                if (GroundDisplayedCounter == -1)
//                {
//                    prefab = StartGroundPrefab;
//                }
//                else
//                {
//                    int weight = Random.Range(1, 101);
//                    int weightCounter = 0;

//                    //Sanity Check
//#if UNITY_EDITOR
//                    for (int i = 0; i < Grounds.Count; i++)
//                    {
//                        weightCounter += Grounds[i].GetWeight(GroundDisplayedCounter);
//                    }
//                    if (weightCounter != 100)
//                    {
//                        MyDebug.LogWarningF("GroundHandler, CreateAndAddGround: Warning weights dont add up to 100 ({0}) at {1}", weightCounter, GroundDisplayedCounter);
//                    }
//                    weightCounter = 0; // reset
//#endif


//                    for (int i = 0; i < Grounds.Count; i++)
//                    {
//                        weightCounter += Grounds[i].GetWeight(GroundDisplayedCounter);
//                        Debug.Log(weightCounter + ", " + weight);
//                        if (weightCounter >= weight)
//                        {
//                            prefab = GroundPrefabs[i];
//                            break;
//                        } 
//                    }

//                    //int weightValue = Random.Range(0, TotalGroundWeighting);
//                    //int weightCounter = 0;
//                    //for (int j = 0; j < Grounds.ToArray().Length; j++)
//                    //{
//                    //    weightCounter += Grounds[j].Weighting;
//                    //    if (weightValue < weightCounter)
//                    //    {
//                    //        prefab = GroundPrefabs[j];
//                    //        break;
//                    //    }
//                    //}
//                }
//            }

//            // Create prefab
//            Transform newPrefab = (Transform)Instantiate(prefab);
//            newPrefab.transform.parent = this.transform;
//            newPrefab.position = new Vector3(0, 0, NextGroundTransformPosition);

//            // update counter
//            GroundDisplayedCounter++;

//            // add to list
//            AddedGroundTransforms.Add(newPrefab);
//            return newPrefab;
//        }


//        private Transform CreateAndAddObstanclesToGround(Transform newPrefab)
//        {
//            // add obstacles but only after first ground
//            if (AddedGroundTransforms.Count > 1)
//            {
//                int weight = Random.Range(1, 101);
//                int weightCounter = 0;

//                //Sanity Check
//#if UNITY_EDITOR
//                for (int i = 0; i < Obstacles.Count; i++)
//                {
//                    weightCounter += Obstacles[i].GetWeight(GroundDisplayedCounter);
//                }
//                if (weightCounter != 100)
//                {
//                    MyDebug.LogWarningF("GroundHandler, AddObstanclesToGround: Warning weights dont add up to 100 ({0}) at {1}", weightCounter, GroundDisplayedCounter);
//                }
//                weightCounter = 0; // reset
//#endif

//                for (int i = 0; i < Obstacles.Count; i++)
//                {
//                    weightCounter += Obstacles[i].GetWeight(GroundDisplayedCounter);
//                    Debug.Log(weightCounter + ", " + weight);
//                    if (weightCounter >= weight)
//                    {
//                        Transform obstaclePrefab = (Transform)Instantiate(ObstaclePrefabs[i]);
//                        obstaclePrefab.transform.parent = newPrefab.transform;
//                        obstaclePrefab.position = new Vector3(0, 0, NextGroundTransformPosition);
//                        return obstaclePrefab;
//                    }
//                }
//            }
//            return null;
//        }


//        void Update()
//        {
//            LoopingScroll(AddedGroundTransforms);
//        }


//        private void LoopingScroll(List<Transform> backgroundPart)
//        {
//            // Get the first object.
//            // The list is ordered from left (x position) to right.
//            Transform firstChild = backgroundPart.FirstOrDefault();

//            if (firstChild != null)
//            {
//                // Check if the child is already (partly) before the camera.
//                // We test the position first because the IsVisibleFrom
//                // method is a bit heavier to execute.
//                if (firstChild.position.z < Camera.main.transform.position.z)
//                {
//                    // If the child is already on the left of the camera,
//                    // we test if it's completely outside and needs to be
//                    // recycled.
//                    if (firstChild.renderer.IsVisibleFrom(Camera.main) == false)
//                    {
//                        // add a new one
//                        CreateGround(1);

//                        // and remove the old part
//                        backgroundPart.Remove(firstChild);
//                        Destroy(firstChild.gameObject);
//                    }
//                }
//            }
//        }
    }
}