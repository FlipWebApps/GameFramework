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
    /// Container class to allow for arrays of multiple scriptable objects that derive from 
    /// a common base class, including dynamic loading.
    /// </summary>
    /// Standard Unity arrays that are to be serialised can only contain items of a specific type 
    /// (and do not support derived classes). The only way we can reference and save an array of 
    /// potentially different items that inherit from a single base class is to use scriptable objects.
    /// 
    /// ScriptableObjects themselves have a limitation that it we want dynamically setup such a 
    /// collection that references ScriptableObjects then we can't then create a prefab of the gameobject 
    /// as all scriptable object references in a prefab must be to external assets or they will become 
    /// null when part of a prefab (at least verified to be the case upto Unity 5.5).
    /// 
    /// This class gives a workaround where you create an array of a non generic derived class of 
    /// ScriptableObjectContainer where ScriptableObjectContainer can contain either:
    ///  - A reference to a ScriptableObject asset on disk deriving from type T (exposed through the 
    ///    ScriptableObjectReference property
    ///  - A dynamically created ScriptableObject deriving from type T whose class name is specifying in
    ///    the ClassName property and JSON serialisation data that represents the serialised version of the 
    ///    Scriptable Object is held in the Data property. ClassName must be specified for dynamic creation to work.
    ///    Referencing the serialised object will dynamically serialise and deserialise correctly.
    /// 
    /// The Identier and UseScriptableObject parameters can additionally be used to allow for the use of built in types.
    [Serializable]
    public class ScriptableObjectContainer<T> : ISerializationCallbackReceiver where T : ScriptableObject
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
        /// A reference to the contained scriptableobject
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
                        if (_scriptableObject != null && !string.IsNullOrEmpty(Data))
                            JsonUtility.FromJsonOverwrite(Data, _scriptableObject);
                    }
                }
                return _scriptableObject;
            }
            set { _scriptableObject = value; }
        }
        T _scriptableObject;

        #endregion Dynamically loaded from JSON in Data

        #region ISerializationCallbackReceiver

        public void OnBeforeSerialize()
        {
            if (_scriptableObject != null)
                Data = JsonUtility.ToJson(ScriptableObject);
        }

        public void OnAfterDeserialize() { }

        #endregion ISerializationCallbackReceiver
    }
}