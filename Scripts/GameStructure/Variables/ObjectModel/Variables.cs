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
using GameFramework.Localisation.ObjectModel;
using GameFramework.Preferences;
using UnityEngine;

namespace GameFramework.GameStructure.Variables.ObjectModel
{
    /// <summary>
    /// Holds a list of customisable variables
    /// </summary>
    [Serializable]
    public class Variables
    {
        public enum VariableType
        {
            Bool,
            Float,
            Int,
            String
        }

        #region Editor Parameters

        /// <summary>
        /// An array of BoolVariables
        /// </summary>
        BoolVariable[] BoolVariables
        {
            get
            {
                return _boolVariables;
            }
            set
            {
                _boolVariables = value;
            }
        }
        [Tooltip("An array of BoolVariables.")]
        [SerializeField]
        BoolVariable[] _boolVariables = new BoolVariable[0];

        /// <summary>
        /// An array of IntVariables
        /// </summary>
        IntVariable[] IntVariables
        {
            get
            {
                return _intVariables;
            }
            set
            {
                _intVariables = value;
            }
        }
        [Tooltip("An array of IntVariables.")]
        [SerializeField]
        IntVariable[] _intVariables = new IntVariable[0];

        /// <summary>
        /// An array of FloatVariables
        /// </summary>
        FloatVariable[] FloatVariables
        {
            get
            {
                return _floatVariables;
            }
            set
            {
                _floatVariables = value;
            }
        }
        [Tooltip("An array of FloatVariables.")]
        [SerializeField]
        FloatVariable[] _floatVariables = new FloatVariable[0];

        /// <summary>
        /// An array of StringVariable
        /// </summary>
        StringVariable[] StringVariables
        {
            get
            {
                return _stringVariables;
            }
            set
            {
                _stringVariables = value;
            }
        }
        [Tooltip("An array of StringVariable.")]
        [SerializeField]
        StringVariable[] _stringVariables = new StringVariable[0];

        #endregion Editor Parameters

        #region Load / Save
        /// <summary>
        /// Load saved values from preferences or set to default if not found. 
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="useSecurePrefs"></param>
        public void Load(string prefix = "", bool? useSecurePrefs = null)
        {
            foreach (var variable in StringVariables)
            {
                if (variable.PersistChanges)
                    variable.Value = PreferencesFactory.GetString(prefix + variable.Tag, variable.Value, useSecurePrefs);
                else
                    variable.Value = variable.DefaultValue;
            }
        }

        /// <summary>
        /// Update PlayerPrefs with values that should be saved.
        /// </summary>
        /// Note: This does not call PlayerPrefs.Save()
        /// <param name="prefix"></param>
        /// <param name="useSecurePrefs"></param>
        public void UpdatePlayerPrefs(string prefix = "", bool? useSecurePrefs = null)
        {
            foreach (var variable in StringVariables)
            {
                if (variable.PersistChanges)
                    PreferencesFactory.SetString(prefix + variable.Tag, variable.Value, useSecurePrefs);
            }
        }
        #endregion Load / Save

        #region get
        /// <summary>
        /// Return a BoolVariable with the given tag.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public BoolVariable GetBool(string tag)
        {
            foreach (var variable in BoolVariables)
                if (variable.Tag == tag)
                    return variable;
            return null;
        }

        /// <summary>
        /// Return an FloatVariable with the given tag.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public FloatVariable GetFloat(string tag)
        {
            foreach (var variable in FloatVariables)
                if (variable.Tag == tag)
                    return variable;
            return null;
        }

        /// <summary>
        /// Return an IntVariable with the given tag.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public IntVariable GetInt(string tag)
        {
            foreach (var variable in IntVariables)
                if (variable.Tag == tag)
                    return variable;
            return null;
        }

        /// <summary>
        /// Return an StringVariable with the given tag.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public StringVariable GetString(string tag)
        {
            foreach (var variable in StringVariables)
                if (variable.Tag == tag)
                    return variable;
            return null;
        }

        #endregion get
    }


    [Serializable]
    public class Variable<T>
    {
        #region Editor Parameters


        /// <summary>
        /// A unique tag that represents this variable
        /// </summary>
        public string Tag
        {
            get
            {
                return _tag;
            }
            set
            {
                _tag = value;
            }
        }
        [Tooltip("A unique tag that represents this variable")]
        [SerializeField]
        string _tag;

        /// <summary>
        /// A LocalisableText that holds a name for this variable that can be used for display purposes
        /// </summary>
        public LocalisableText Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        [Tooltip("A LocalisableText that holds a name for this variable that can be used for display purposes")]
        [SerializeField]
        LocalisableText _name;

        /// <summary>
        /// An optional category that this item belongs to
        /// </summary>
        /// Can be used for grouping items
        public bool Category
        {
            get
            {
                return _category;
            }
            set
            {
                _category = value;
            }
        }
        [Tooltip("An optional category that this item belongs to. Can be used for grouping items")]
        [SerializeField]
        bool _category;

        /// <summary>
        /// A default value for this variable
        /// </summary>
        /// This is also reflected through the Value property which can also be updated 
        public T DefaultValue
        {
            get
            {
                return _defaultValue;}
            set
            {
                _defaultValue = value;
            }
        }
        [Tooltip("A default value for this variable")]
        [SerializeField]
        T _defaultValue;

        /// <summary>
        /// Whether changes should be saved across game sessions
        /// </summary>
        public bool PersistChanges
        {
            get
            {
                return _persistChanges;
            }
            set
            {
                _persistChanges = value;
            }
        }
        [Tooltip("Whether changes should be saved across game sessions.")]
        [SerializeField]
        bool _persistChanges;

        #endregion Editor Parameters

        /// <summary>
        /// The current value of this item
        /// </summary>
        public T Value { get; set; }

    }

    [Serializable]
    public class BoolVariable : Variable<bool>
    {
        #region Editor Parameters

        #endregion Editor Parameters
    }

    [Serializable]
    public class IntVariable : Variable<int>
    {
        #region Editor Parameters

        #endregion Editor Parameters
    }

    [Serializable]
    public class FloatVariable : Variable<float>
    {
        #region Editor Parameters

        #endregion Editor Parameters
    }

    [Serializable]
    public class StringVariable : Variable<string>
    {
        #region Editor Parameters

        #endregion Editor Parameters
    }
}