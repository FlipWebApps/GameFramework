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
using System.Collections;
using System.Collections.Generic;
using GameFramework.GameObjects.Components;
using UnityEngine;
using UnityEngine.Networking;

namespace GameFramework.Networking
{
    /// <summary>
    /// 
    /// NOTE: This class is beta and subject to changebreaking change without warning.
    /// </summary>
    public class NetworkPlayManager : Singleton<NetworkPlayManager>
    {
        public NetworkDiscovery NetworkDiscovery;
        public Dictionary<string, NetworkGame> NetworkDiscoveryServers;

        public Action<string, string> OnReceivedBroadcast;
        public Action OnServerListChanged;
        public Action OnClientConnect;
        public Action OnServerConnect;
        public Action OnClientDisconnect;
        public Action OnServerDisconnect;

        public NetworkClient Client;

        protected override void GameSetup()
        {
            NetworkDiscoveryServers = new Dictionary<string, NetworkGame>(5);

            OnReceivedBroadcast += _OnReceivedBroadcast;
        }


        protected override void GameDestroy()
        {
            OnReceivedBroadcast -= _OnReceivedBroadcast;
        }

        //
        // GENERIC FUNCTIONS
        //
        public void StopNetworking()
        {
            HostGameStop();
            JoinGameStop();
            StopGameDiscovery();
        }

        void StopGameDiscovery()
        {
            if (NetworkDiscovery.running)
            {
                NetworkDiscovery.StopBroadcast();
            }
        }

        //
        // HOSTING OF A GAME
        //
        public bool HostGame()
        {
            StartHost();
            //UnityEngine.Networking.NetworkManager.singleton.StartServer();

            if (NetworkDiscovery.Initialize())
            {
                if (NetworkDiscovery.StartAsServer())
                {
                    return true;
                }
            }
            return false;
        }

        public NetworkClient StartHost()
        {
            Client = NetworkManager.singleton.StartHost();
            return Client;
        }

        public void HostGameStop()
        {
            StopGameDiscovery();
            NetworkManager.singleton.StopHost();
        }


        //
        // JOINING A GAME
        //
        public bool JoinGame()
        {
            NetworkDiscoveryServers.Clear();

            try
            {
                if (NetworkDiscovery.Initialize())
                {
                    if (NetworkDiscovery.StartAsClient())
                    {
                        StartCoroutine(RefreshServers());
                        return true;
                    }
                }
            }
            catch (Exception) { }
            return false;
        }

        public void JoinGameStop()
        {
            StopGameDiscovery();
            NetworkManager.singleton.StopClient();
        }

        public void StartClient(string address)
        {
            if (NetworkManager.singleton != null && NetworkManager.singleton.client == null)
            {
                NetworkManager.singleton.networkAddress = address;
                //UnityEngine.Networking.NetworkManager.singleton.networkPort = Convert.ToInt32(items[2]);
                Client = NetworkManager.singleton.StartClient();
            }
        }

        IEnumerator RefreshServers()
        {
            do
            {
                bool itemsRemoved = false;
                Dictionary<string, NetworkGame> updatedServers = new Dictionary<string, NetworkGame>(5);
                foreach (KeyValuePair<string, NetworkGame> networkGame in Instance.NetworkDiscoveryServers)
                {
                    if (networkGame.Value.LastDetected.AddSeconds(2) > DateTime.Now)
                        updatedServers.Add(networkGame.Key, networkGame.Value);
                    else
                        itemsRemoved = true;
                }
                NetworkDiscoveryServers = updatedServers;
                if (itemsRemoved && OnServerListChanged != null)
                    OnServerListChanged();

                yield return new WaitForSeconds(1);
            } while (NetworkDiscovery.running);
        }

        void _OnReceivedBroadcast(string fromAddress, string data)
        {
            //       base.OnReceivedBroadcast(fromAddress, data);
            //MyDebug.LogF("Received Join Game From: {0} ({1}).", fromAddress, data);

            // add to the list of possible servers.
            NetworkGame networkGame;
            if (NetworkDiscoveryServers.TryGetValue(fromAddress, out networkGame))
            {
                networkGame.LastDetected = DateTime.Now;
            }
            else
            {
                NetworkDiscoveryServers.Add(fromAddress, new NetworkGame { Address = fromAddress, Name = data, LastDetected = DateTime.Now });

                if (OnServerListChanged != null)
                    OnServerListChanged();
            }
        }

        public class NetworkGame
        {
            public string Address;
            public string Name;
            public DateTime LastDetected;
        }
    }
}