/*
Simple level of detail script.
It allows for up to 2 lower level of detail.
If nothing is assigned to lod0, this game object will be assigned to lod0.
It is ok to only have 1 lower level og detail, lod2 will simply be ignored.
If Cull Mesh is checked the mesh will be culled when max distance is reached.
The lod changes between the specified distances.
*/
/*
Usage:
Create new empty gameobject and attach the script to it
put your model with it's levels of detail in the scene and attach them to the script LOD slots
attach your camera or player to the script at "scene camera" slot

*/

using System.Collections;
using UnityEngine;

namespace GameFramework.Display.Other.Components
{
    /// <summary>
    /// Deprecated: Please see LevelOfDetail instead
    /// </summary>
    [AddComponentMenu("Game Framework/Display/Other/LOD")]
    [HelpURL("http://www.flipwebapps.com/game-framework/")]
    public class LOD : MonoBehaviour
    {

        public GameObject lod0;
        public GameObject lod1;
        public GameObject lod2;
        public GameObject SceneCamera;
        public bool CullMesh;
        public float CheckInterval = 1f;
        public float Dist1 = 100;
        public float Dist2 = 300;
        public float Dist3 = 500;
        float _distance;

        void Start()
        {
            Debug.LogWarning("This component is deprecated. Please replace with the LevelOfDetail component. This component will be removed in a future version.");
            StartCoroutine(DistanceCheck());
        }

        IEnumerator DistanceCheck()
        {
            while (true)
            {
                _distance = Vector3.Distance(SceneCamera.transform.position, transform.position);

                if (_distance < Dist1)
                {
                    lod0.SetActive(true);
                    if (lod1)
                        lod1.SetActive(false);
                    if (lod2)
                        lod2.SetActive(false);

                }
                else if (_distance > Dist1 && _distance < Dist2 && lod1)
                {
                    lod0.SetActive(false);
                    if (lod1)
                        lod1.SetActive(true);
                    if (lod2)
                        lod2.SetActive(false);

                }
                else if (_distance > Dist2 && _distance < Dist3 && lod2)
                {
                    lod0.SetActive(false);
                    if (lod1)
                        lod1.SetActive(false);
                    if (lod2)
                        lod2.SetActive(true);
                }
                else if (_distance > Dist3 && CullMesh)
                {
                    lod0.SetActive(false);
                    if (lod1)
                        lod1.SetActive(false);
                    if (lod2)
                        lod2.SetActive(false);

                }
                yield return new WaitForSeconds(CheckInterval);
            }

        }
    }
}