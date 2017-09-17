using System;
using UnityEngine;

//Original version of the ConditionalHideAttribute created by Brecht Lecluyse (www.brechtos.com)
//Modified by: -

namespace GameFramework.EditorExtras
{
    /// <summary>
    /// Attribute for causing a field to be conditionally hidden based upon the status of a seperate boolean value
    /// </summary>
    /// Add this to the field that you want to conditionally hide when the value of the target is true (can be inverted). 
    /// See also ConditionalHidePropertyDrawer
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct)]
    public class ConditionalHideAttribute : PropertyAttribute
    {
        /// <summary>
        /// The name of the bool field that will determine whether to hide this property
        /// </summary>
        public string ConditionalSourceField;
        public string ConditionalSourceField2 = "";
        /// <summary>
        /// Whether to hide or just disable the property.
        /// </summary>
        public bool HideInInspector = false;
        /// <summary>
        /// Whether it invert the hiding operation (target of true means hide)
        /// </summary>
        public bool Inverse = false;

        // Use this for initialization
        public ConditionalHideAttribute(string conditionalSourceField)
        {
            ConditionalSourceField = conditionalSourceField;
            HideInInspector = false;
            Inverse = false;
        }

        public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector)
        {
            ConditionalSourceField = conditionalSourceField;
            HideInInspector = hideInInspector;
            Inverse = false;
        }

        public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector, bool inverse)
        {
            ConditionalSourceField = conditionalSourceField;
            HideInInspector = hideInInspector;
            Inverse = inverse;
        }
   
    }
}



