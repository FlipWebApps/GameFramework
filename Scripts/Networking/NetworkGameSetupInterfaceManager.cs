//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using System.Collections.Generic;
using FlipWebApps.GameFramework.Scripts.GameObjects;
using FlipWebApps.GameFramework.Scripts.GameObjects.Components;
using FlipWebApps.GameFramework.Scripts.UI.Dialogs.Components;
using FlipWebApps.GameFramework.Scripts.UI.Other;
using FlipWebApps.GameFramework.Scripts.UI.Other.Components;
using UnityEngine;
using UnityEngine.UI;

namespace FlipWebApps.GameFramework.Scripts.Networking
{
    /// <summary>
    /// 
    /// NOTE: This class is beta and subject to changebreaking change without warning.
    /// </summary>
    public class NetworkGameSetupInterfaceManager : Singleton<NetworkGameSetupInterfaceManager>
    {
        public DialogInstance DialogInstance;

        [Header("Interface Elements")]
        public GameObject JoinGameDisplay;
        public GameObject HostGameDisplay;
        public Button NewGameButton;
        public Text Message;
        public GameObject JoinGameEntry;

        System.Action<bool> _doneCallback;

        /// <summary>
        /// Show gui and default to listening for available network games
        /// </summary>
        public void Show(System.Action<bool> doneCallback = null)
        {
            _doneCallback = doneCallback;

            NetworkPlayManager.Instance.OnServerListChanged += UpdateServerList;
            NetworkPlayManager.Instance.OnServerConnect += StartHostedGame;
            NetworkPlayManager.Instance.OnClientConnect += StartHostedGame;
            DialogInstance.Show(destroyOnClose: false);

            JoinGame();
        }

        public void JoinGame()
        {
            NetworkPlayManager.Instance.StopNetworking();

            if (NetworkPlayManager.Instance.JoinGame())
            {
                NewGameButton.interactable = true;
                HostGameDisplay.SetActive(false);
                JoinGameDisplay.SetActive(true);
            }
            else
            {
                Message.text = "Error starting client. Try again later.";
            }
        }

        public void JoinGame(string address)
        {
            Debug.Log("JOIN GAME" + address);
            //var items = data.Split(':');
            //if (items.Length == 3 && items[0] == "NetworkManager")
            //{
            NetworkPlayManager.Instance.StopNetworking();
            NetworkPlayManager.Instance.StartClient(address);
            //}
        }



        public void HostGame()
        {
            NetworkPlayManager.Instance.StopNetworking();

            if (NetworkPlayManager.Instance.HostGame())
            {
                NewGameButton.interactable = false;
                HostGameDisplay.SetActive(true);
                JoinGameDisplay.SetActive(false);
            }
            else
            {
                Message.text = "Error starting server. Try again later.";
            }
        }

        public void CancelHostedGame()
        {
            JoinGame();
        }

        public void StartHostedGame()
        {
            Done(true);
        }

        public void Cancel()
        {
            NetworkPlayManager.Instance.StopNetworking();

            Done(false);
        }

        void Done(bool isSetUp)
        {
            DialogInstance.DoneOk();

            NetworkPlayManager.Instance.OnClientConnect -= StartHostedGame;
            NetworkPlayManager.Instance.OnServerConnect -= StartHostedGame;
            NetworkPlayManager.Instance.OnServerListChanged -= UpdateServerList;

            if (_doneCallback != null)
                _doneCallback(isSetUp);
        }

        void UpdateServerList()
        {
            Debug.Log("ServerListUpdate");
            GameObject go = GameObjectHelper.GetChildNamedGameObject(JoinGameDisplay, "Content", true);
            GameObjectHelper.DestroyChildren(go.transform);
            foreach (KeyValuePair<string, NetworkPlayManager.NetworkGame> networkGame in NetworkPlayManager.Instance.NetworkDiscoveryServers)
            {
                Debug.Log(networkGame.Key + ", " + networkGame.Value.Name);
                //NetworkGameSetupJoinButton networkGameSetupJoinButton = JoinGameEntry.GetComponent<NetworkGameSetupJoinButton>();
                //networkGameSetupJoinButton.NetworkGame = networkGame.Value;

                GameObject go2 = Instantiate(JoinGameEntry);
                NetworkGameSetupJoinButton networkGameSetupJoinButton = go2.GetComponent<NetworkGameSetupJoinButton>();
                networkGameSetupJoinButton.NetworkGame = networkGame.Value;
                UIHelper.SetTextOnChildGameObject(go2, "Name", networkGame.Key + ", " + networkGame.Value.Name);
                go2.transform.SetParent(go.transform);
                go2.transform.localScale = Vector3.one;
            }
        }
    }

}