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

using GameFramework.Debugging;
using UnityEngine.Networking;

namespace GameFramework.Networking
{
    /// <summary>
    /// 
    /// NOTE: This class is beta and subject to changebreaking change without warning.
    /// </summary>
    public class NetworkManagerCallbacks : NetworkManager
    {
        public override void OnClientConnect(NetworkConnection conn)
        {
            MyDebug.LogF("NetworkManagerCallbacks,OnClientConnect: {0})", conn.address);
            base.OnClientConnect(conn);

            if (NetworkPlayManager.Instance.OnClientConnect != null)
                NetworkPlayManager.Instance.OnClientConnect();
        }

        public override void OnServerConnect(NetworkConnection conn)
        {
            MyDebug.LogF("NetworkManagerCallbacks,OnServerConnect: {0})", conn.address);
            base.OnServerConnect(conn);

            if (NetworkPlayManager.Instance.OnServerConnect != null)
                NetworkPlayManager.Instance.OnServerConnect();
        }

        public override void OnClientDisconnect(NetworkConnection conn)
        {
            MyDebug.LogF("NetworkManagerCallbacks,OnClientDisconnect: {0})", conn.address);
            base.OnClientDisconnect(conn);

            if (NetworkPlayManager.Instance.OnClientDisconnect != null)
                NetworkPlayManager.Instance.OnClientDisconnect();
        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            MyDebug.LogF("NetworkManagerCallbacks,OnServerDisconnect: {0})", conn.address);
            base.OnServerDisconnect(conn);

            if (NetworkPlayManager.Instance.OnServerDisconnect != null)
                NetworkPlayManager.Instance.OnServerDisconnect();
        }

    }
}