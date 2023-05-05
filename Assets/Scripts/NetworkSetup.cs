using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class NetworkSetup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        if (Manager.Instance.networkMode == NetworkMode.Client) StartClient();
        else if (Manager.Instance.networkMode == NetworkMode.Server) StartServer();
        else StartHost();
    }

    IEnumerator Example_ConfigureTransportAndStartNgoAsHost() {
        // yield return new WaitForSeconds(5f); // wait 1s for the game to startup first, otherwise the relay will not setup well and causing the client to have 404

        var serverRelayUtilityTask = RelayManager.Instance.SetupRelay();
        while (!serverRelayUtilityTask.IsCompleted) {
            yield return null;
        }
        if (serverRelayUtilityTask.IsFaulted) {
            Debug.LogError("Exception thrown when attempting to start Relay Server. Server not started. Exception: " + serverRelayUtilityTask.Exception.Message);
            yield break;
        }

        // display the join code
        var data = serverRelayUtilityTask.Result;
        Manager.Instance.joinCodeText.text = data.joinCode;

        NetworkManager.Singleton.StartHost();
        yield return null;
    }

     IEnumerator Example_ConfigureTransportAndStartNgoAsServer() {
        yield return new WaitForSeconds(0.5f); // wait 1s for the game to startup first, otherwise the relay will not setup well and causing the client to have 404

        var serverRelayUtilityTask = RelayManager.Instance.SetupRelay();
        while (!serverRelayUtilityTask.IsCompleted) {
            yield return null;
        }
        if (serverRelayUtilityTask.IsFaulted) {
            Debug.LogError("Exception thrown when attempting to start Relay Server. Server not started. Exception: " + serverRelayUtilityTask.Exception.Message);
            yield break;
        }

        // display the join code
        var data = serverRelayUtilityTask.Result;
        Manager.Instance.joinCodeText.text = data.joinCode;

        NetworkManager.Singleton.StartServer();
        yield return null;
    }

    void StartHost() {
        StartCoroutine(Example_ConfigureTransportAndStartNgoAsHost());

        //if (RelayManager.Instance.IsRelayEnabled) {
        //    var data = await RelayManager.Instance.SetupRelay();
        //    Manager.Instance.joinCodeText.text = data.joinCode;
        //}

        //NetworkManager.Singleton.StartHost();
    }

    async void StartClient() {
        if (RelayManager.Instance.IsRelayEnabled)
            await RelayManager.Instance.JoinRelay(Manager.Instance.joinCode);
        NetworkManager.Singleton.StartClient();
    }

    void StartServer() {
        StartCoroutine(Example_ConfigureTransportAndStartNgoAsServer());
    }
}
