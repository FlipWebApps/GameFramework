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

using UnityEditor;

namespace GameFramework.GameObjects.Editor.AbstractClasses
{
    public abstract class RunOnStateEditor : UnityEditor.Editor
    {
        //GameItem _gameItem;
        SerializedProperty _runProperty;
        SerializedProperty _runFrequencyProperty;

        public virtual void OnEnable()
        {
            //_gameItem = (GameItem)target;
            // get serialized objects so we can use attached property drawers (e.g. tooltips, ...)
            _runProperty = serializedObject.FindProperty("Run");
            _runFrequencyProperty = serializedObject.FindProperty("RunFrequency");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_runProperty);
            if (_runProperty.enumValueIndex == 4)
            EditorGUILayout.PropertyField(_runFrequencyProperty);
        }
    }
}