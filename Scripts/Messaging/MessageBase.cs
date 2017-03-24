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

using System.Diagnostics;

namespace GameFramework.Messaging
{
    /// <summary>
    /// A base class that should be used and inherited for all messages that we send through the 
    /// messaging system.
    /// </summary>
    public class BaseMessage
    {
        public enum SendModeType { SendToAll, SendToFirst }

        public string Name;
        public SendModeType SendMode;

#if UNITY_EDITOR
        public bool DontShowInEditorLogInternal;         // only needed for editor mode.
#endif

        public BaseMessage() {
            Name = GetType().Name;
            SendMode = SendModeType.SendToAll;
        }


        /// <summary>
        /// Hide this message so that it isn't displayed in the editor logs. (Editor only method)
        /// </summary>
        /// Use this method rather than setting the property so that it will be easily compiled out in non editor mode.
        [Conditional("UNITY_EDITOR")]
        public void DontShowInEditorLog()
        {
#if UNITY_EDITOR
            DontShowInEditorLogInternal = true;
#endif
        }
    }
}