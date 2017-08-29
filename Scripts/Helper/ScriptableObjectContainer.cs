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

using System;
using UnityEngine;

namespace GameFramework.Helper
{
    /// <summary>
    /// Container class to allow for arrays of multiple objects that derive from a common base class, 
    /// including dynamic loading.
    /// </summary>
    /// Standard Unity arrays that are to be serialised can only contain items of a specific type 
    /// (and do not support derived classes). The only way we can reference and save an array of 
    /// potentially different items that inherit from a single base class is to use scriptable objects.
    /// 
    /// ScriptableObjects themselves have a limitation in that reference to other GameObjects don't work
    /// in both scene view and when using a prefab - If the ScriptableObject is saved into the scene then we
    /// can't create prefabls as references will be lost, if we save the ScriptableObject to an external asset
    /// then prefab references will not update and always refer the prefab. Saving the ScriptableObject to
    /// an internal string and using custom serialisation / deserialisation also exhibits the same problems
    /// 
    /// This class gives a (ugly, horrible) workaround where you create an array of a non generic derived class of 
    /// ScriptableObjectContainer where ScriptableObjectContainer can contain:
    ///  - A reference to a ScriptableObject asset on disk deriving from type T (exposed through the 
    ///    ScriptableObjectReference property
    ///  - A dynamically created ScriptableObject deriving from type T whose class name is specifying in
    ///    the ClassName property and JSON serialisation data that represents the serialised version of the 
    ///    Scriptable Object is held in the Data property. ClassName must be specified for dynamic creation to work.
    ///    Referencing the serialised object will dynamically serialise and deserialise correctly.
    /// 
    /// The references array on this class will work and update correctly across scene items / prefabs and can be
    /// used for storage of such references by the child class as needed. We need ScriptableObjects rather than
    /// additionally providing further arrays of common types (e.g. float, string) to allow support for arrays of
    /// structs.
    /// 
    /// When Unity supports polymorphic serialisation this class can die a happy death!
    [Serializable]
    public class ScriptableObjectContainer<T> : IScriptableObjectContainerReferences where T : ScriptableObject, IScriptableObjectContainerSyncReferences

    {
        #region As reference to external asset

        /// <summary>
        /// A reference to a scriptableobject that is located as a seperate asset
        /// </summary>
        public T ScriptableObjectReference
        {
            get { return _scriptableObjectReference; }
            set { _scriptableObjectReference = value; }
        }
        [SerializeField]
        T _scriptableObjectReference;

        /// <summary>
        /// Whether to use the ScriptableObjectReference (which could be null) rather than the DynamicScriptableObject
        /// </summary>
        public bool IsReference
        {
            get { return _isReference; }
            set { _isReference = value; }
        }
        [SerializeField]
        bool _isReference = false;

        #endregion As reference to external asset

        #region Dynamically loaded from JSON in Data

        /// <summary>
        /// The class name of the ScriptableObject that this contains. If this is null then it is 
        /// assumed that no dynamic setup should take place.
        /// </summary>
        public string ClassName
        {
            get { return _className; }
            set { _className = value; }
        }
        [SerializeField]
        string _className;


        /// <summary>
        /// An array of Unity object references that can be used for your own purposes.
        /// </summary>
        /// (Ugly) workaround due to the limitations of object reference serialisation.
        public UnityEngine.Object[] ObjectReferences
        {
            get { return _objectReferences; }
            set { _objectReferences = value; }
        }
        [SerializeField]
        UnityEngine.Object[] _objectReferences;


        /// <summary>
        /// Data to use for (de)serialising the scriptable object.
        /// </summary>
        public string Data
        {
            get { return _data; }
            set { _data = value; }
        }
        [SerializeField]
        string _data;


        /// <summary>
        /// A reference to the contained scriptableobject - dynamically created on first access.
        /// </summary>
        public T ScriptableObject
        {
            get
            {
                if (_scriptableObject == null)
                {
                    if (!string.IsNullOrEmpty(ClassName))
                    {
                        _scriptableObject = UnityEngine.ScriptableObject.CreateInstance(ClassName) as T;
                        if (_scriptableObject != null)
                        {
                            if (!string.IsNullOrEmpty(Data))
                                JsonUtility.FromJsonOverwrite(Data, _scriptableObject);
                            _scriptableObject.SetReferencesFromContainer(ObjectReferences);
                        }
                    }
                }
                return _scriptableObject;
            }
            set { _scriptableObject = value; }
        }
        T _scriptableObject;

        #endregion Dynamically loaded from JSON in Data

        // removed for now - ensure this is update in the editor so that things work with Prefabs etc.
        //#region ISerializationCallbackReceiver

        //public void OnBeforeSerialize()
        //{
        //    if (_scriptableObject != null)
        //    {
        //        //Data = JsonUtility.ToJson(ScriptableObject);
        //        //ScriptableObject.PushReferencesToContainer(this);
        //    }
        //}

        //public void OnAfterDeserialize() { }

        //#endregion ISerializationCallbackReceiver
    }


    public interface IScriptableObjectContainerSyncReferences
    {
        void SetReferencesFromContainer(UnityEngine.Object[] objectReferences);
        UnityEngine.Object[] GetReferencesForContainer();
    }

    public interface IScriptableObjectContainerReferences
    {
        UnityEngine.Object[] ObjectReferences { get; set; }
        string Data { get; set; }
    }
}