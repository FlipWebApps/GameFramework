//----------------------------------------------
// copypasta
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.Preferences;
using FlipWebApps.GameFramework.Scripts.Debugging;
using FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.Messages;
using FlipWebApps.GameFramework.Scripts.GameStructure.Players.ObjectModel;
using FlipWebApps.GameFramework.Scripts.Helper;
using FlipWebApps.GameFramework.Scripts.Localisation;
using UnityEngine;
using UnityEngine.Assertions;

namespace FlipWebApps.GameFramework.Scripts.GameStructure.GameItems.ObjectModel //TODO: Place it where you need
{

    [CreateAssetMenu(fileName = "gameItemExtensionData")]
    public class GameItemExtension : ScriptableObject
    {

        /// <summary>
        /// The name of this gameitem. Through the constructor you can specify whether this is part of a localisation key, or a fixed value
        /// </summary>
        public string Name
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
        [Tooltip("Game Item name")]
        [SerializeField]
        string _name;

        /// <summary>
        /// A description of this gameitem. Through the constructor you can specify whether this is part of a localisation key, or a fixed value
        /// </summary>
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }
        [Tooltip("Game Item description")]
        [SerializeField]
        string _description;

        /// <summary>
        /// A value that is needed to unlock this item.
        /// </summary>
        /// Typically this will be the number of coins that you need to collect before being able to unlock this item. A value of
        /// -1 means that you can not unlock this item in this way.
        public int ValueToUnlock {
            get
            {
                return _valueToUnlock;
            }
            set
            {
                _valueToUnlock = value;
            }
        }
        [Tooltip("Value needed to unlock")]
        [SerializeField]
        int _valueToUnlock;

    }

}