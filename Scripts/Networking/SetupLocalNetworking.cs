//----------------------------------------------
// Flip Web Apps: Game Framework
// Copyright © 2016 Flip Web Apps / Mark Hewitt
//----------------------------------------------

using UnityEngine.Networking;

namespace FlipWebApps.GameFramework.Scripts.Networking
{
    /// <summary>
    /// 
    /// NOTE: This class is beta and subject to changebreaking change without warning.
    /// </summary>
    public class SetupLocalNetworking : NetworkDiscovery
    {
        public override void OnReceivedBroadcast(string fromAddress, string data)
        {
            //MyDebug.LogF("SetupLocalNetworking,OnReceivedBroadcast: {0} ({1})", fromAddress, data);

            NetworkPlayManager.Instance.OnReceivedBroadcast(fromAddress, data);
        }

    }
}