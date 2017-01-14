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

using GameFramework.GameObjects.Components;

namespace GameFramework.Networking
{
    /// <summary>
    /// 
    /// NOTE: This class is beta and subject to changebreaking change without warning.
    /// </summary>
    public class PlayerNetworkPlayManager : Singleton<PlayerNetworkPlayManager>
    {
        //public NetworkDiscovery NetworkDiscovery;

        //public Action<string, string> OnReceivedBroadcast;
        //public Action OnClientConnect;
        //public Action OnServerConnect;
        //public Action OnClientDisconnect;
        //public Action OnServerDisconnect;

        //DialogInstance Dialog;

        //public Dictionary<string, string> NetworkDiscoveryServers;

        //protected override void GameSetup()
        //{
        //    NetworkDiscoveryServers = new Dictionary<string, string>(5);

        //    OnReceivedBroadcast += _OnReceivedBroadcast;
        //    OnClientConnect += _OnClientConnect;
        //    OnServerConnect += _OnServerConnect;
        //}


        //protected override void GameDestroy()
        //{
        //    OnReceivedBroadcast -= _OnReceivedBroadcast;
        //    OnClientConnect -= _OnClientConnect;
        //    OnServerConnect -= _OnServerConnect;

        //}

        ////
        //// GENERIC FUNCTIONS
        ////
        //public void StopNetworking()
        //{
        //    HostGameStop();
        //    JoinGameStop();
        //}

        //private void StopGameDiscovery()
        //{
        //    if (NetworkDiscovery.running)
        //    {
        //        NetworkDiscovery.StopBroadcast();
        //    }
        //}

        ////
        //// HOSTING OF A GAME
        ////
        //public void HostGameUI()
        //{
        //    // setup hosting of a game.
        //    if (HostGame())
        //    {
        //        ServerDialog = DialogManager.Instance.Show(
        //            title: LocaliseText.Get("NetworkPlay.DialogTitle"),
        //            text: LocaliseText.Get("NetworkPlay.WaitingForOthers"),
        //            doneCallback: HostGameUICallback,
        //            dialogButtons: DialogInstance.DialogButtonsType.Cancel);
        //    }
        //    else
        //    {
        //        DialogManager.Instance.Show(
        //            title: LocaliseText.Get("NetworkPlay.DialogTitle"),
        //            text: LocaliseText.Get("NetworkPlay.GenericError"),
        //            dialogButtons: DialogInstance.DialogButtonsType.OK);
        //    }
        //}

        //void HostGameUICallback(DialogInstance dialogInstance)
        //{
        //    MyDebug.Log("HostGameUICallback: " + dialogInstance.DialogResult.ToString());
        //    ServerDialog = null;
        //    if (dialogInstance.DialogResult == DialogInstance.DialogResultType.Cancel)
        //        HostGameStop();
        //}

        //public bool HostGame()
        //{
        //    //UnityEngine.Networking.NetworkManager.singleton.StartHost();
        //    UnityEngine.Networking.NetworkManager.singleton.StartServer();
        //    if (NetworkDiscovery.Initialize())
        //    {
        //        if (NetworkDiscovery.StartAsServer())
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        //public void HostGameStop()
        //{
        //    StopGameDiscovery();
        //    UnityEngine.Networking.NetworkManager.singleton.StopHost();
        //}

        //private void _OnServerConnect()
        //{
        //    if (ServerDialog != null)
        //    {
        //        StopGameDiscovery();
        //        ServerDialog.DoneCallback = null;
        //        ServerDialog.Done();
        //    }

        //}

        ////
        //// JOINING A GAME
        ////
        //public void JoinGameUI()
        //{
        //    Dialog = DialogManager.Instance.Show(
        //        title: LocaliseText.Get("NetworkPlay.DialogTitle"),
        //        text: LocaliseText.Get("NetworkPlay.WaitingToJoin"),
        //        doneCallback: JoinGameDialogCallback,
        //        dialogButtons: DialogInstance.DialogButtonsType.Cancel);

        //    JoinGame();
        //}

        //void JoinGameDialogCallback(DialogInstance dialogInstance)
        //{
        //    Debug.Log("JoinGameDialogCallback: " + dialogInstance.DialogResult.ToString());
        //    Dialog = null;
        //    if (dialogInstance.DialogResult == DialogInstance.DialogResultType.Cancel)
        //        JoinGameStop();
        //}

        //public void JoinGame()
        //{
        //    NetworkDiscoveryServers.Clear();

        //    NetworkDiscovery.Initialize();
        //    NetworkDiscovery.StartAsClient();
        //}

        //public void JoinGameStop()
        //{
        //    StopGameDiscovery();
        //    UnityEngine.Networking.NetworkManager.singleton.StopClient();
        //}

        //private void _OnReceivedBroadcast(string fromAddress, string data)
        //{
        //    //       base.OnReceivedBroadcast(fromAddress, data);
        //    MyDebug.LogF("Received Join Game From: {0} ({1}). ClientDialog is ", fromAddress, data, Dialog == null ? "null" : "not null");

        //    // add to the list of possible servers.
        //    NetworkDiscoveryServers.Add(fromAddress, data);

        //    // if client dialog then auto start client.
        //    if (Dialog != null)
        //    {
        //        //var items = data.Split(':');
        //        //if (items.Length == 3 && items[0] == "NetworkManager")
        //        //{
        //        if (UnityEngine.Networking.NetworkManager.singleton != null && UnityEngine.Networking.NetworkManager.singleton.client == null)
        //        {
        //            UnityEngine.Networking.NetworkManager.singleton.networkAddress = fromAddress;
        //            //UnityEngine.Networking.NetworkManager.singleton.networkPort = Convert.ToInt32(items[2]);
        //            UnityEngine.Networking.NetworkManager.singleton.StartClient();
        //        }
        //        //}
        //    }
        //}

        //private void _OnClientConnect()
        //{
        //    if (Dialog != null)
        //    {
        //        StopGameDiscovery();
        //        Dialog.DoneCallback = null;
        //        Dialog.Done();
        //    }
        //}
    }
}