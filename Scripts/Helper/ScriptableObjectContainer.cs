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
    /// Container class to allow for arrays of multiple scriptable objects that derive from a common base class.
    /// </summary>
    /// Due to Unity serialisation limitations, the only way we can reference and save an array of potentially different items that 
    /// inherit from a single base class is to use scriptable objects. These themselves have a limitation that it we want dynamically 
    /// setup such a collection that references ScriptableObjects then we can't then create a prefab of the gameobject as all 
    /// scriptable object references in a prefab must be to external assets or they will become null when part of a prefab (at least
    /// verified to be the case upto Unity 5.5).
    /// This class gives a workaround where you can create an array of a non generic derived class of ScriptableObjectContainer.
    /// By specifying a class name of a ScriptableObject that inherits from the generic parameter along with JSON data that represents 
    /// the serialised version of the Scriptable Objects data you can then achieve the affect of having an array of different classes
    /// that derive from a single base class that are automatically serialised and deserialised correctly.
    [Serializable]
    public class ScriptableObjectContainer<T> : ISerializationCallbackReceiver where T : ScriptableObject
    {
        #region Variables
        /// <summary>
        /// The class name of the condition.
        /// </summary>
        public string ClassName
        {
            get { return _className; }
            set { _className = value; }
        }
        [SerializeField]
        string _className;

        /// <summary>
        /// The data associated with this condition
        /// </summary>
        public string Data
        {
            get { return _data; }
            set { _data = value; }
        }
        [SerializeField]
        string _data;
        #endregion Variables

        public T Condition
        {
            get
            {
                if (_condition == null)
                {
                    if (!string.IsNullOrEmpty(ClassName))
                    {
                        _condition = ScriptableObject.CreateInstance(ClassName) as T;
                        if (_condition != null && !string.IsNullOrEmpty(Data))
                            JsonUtility.FromJsonOverwrite(Data, _condition);
                    }
                }
                return _condition;
            }
            set { _condition = value; }
        }

        T _condition;

        public void OnBeforeSerialize()
        {
            if (_condition != null)
                Data = JsonUtility.ToJson(Condition);
        }

        public void OnAfterDeserialize() { }
    }
}