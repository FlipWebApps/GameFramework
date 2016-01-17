//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using FlipWebApps.GameFramework.Scripts.Debugging;
using UnityEngine.Networking;

namespace FlipWebApps.GameFramework.Scripts.Networking
{
    /// <summary>
    /// 
    /// NOTE: This class is beta and subject to changebreaking change without warning.
    /// </summary>
    public class NetworkManagerCallbacks : UnityEngine.Networking.NetworkManager
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