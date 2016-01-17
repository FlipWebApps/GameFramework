//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEngine;

namespace FlipWebApps.GameFramework.Scripts.Networking
{
    /// <summary>
    /// 
    /// NOTE: This class is beta and subject to changebreaking change without warning.
    /// </summary>
    public class NetworkGameSetupJoinButton : MonoBehaviour
    {
        public NetworkPlayManager.NetworkGame NetworkGame;

        public void Awake()
        {
        }

        public void OnClick()
        {
            NetworkGameSetupInterfaceManager.Instance.JoinGame(NetworkGame.Address);
        }
    }
}