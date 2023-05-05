using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;
// #if(UNITY_2018_3_OR_NEWER)
// using UnityEngine.Android;
// #endif
// using agora_gaming_rtc;

public class NetworkPlayer : NetworkBehaviour
{
    public TMPro.TextMeshPro textObject;
    [SerializeField] private Vector2 placementArea = new Vector2(-10, 10);

    [SerializeField] private string channelName;
    // private IRtcEngine mRtcEngine = null;
    [SerializeField] private string AppID = "app_id";

    private NetworkVariable<NetworkString> playerName = new NetworkVariable<NetworkString>();

    GameObject studioLight;
    Light[] lights;
    Renderer _renderer;
    NetworkVariable<bool> _enabled = new NetworkVariable<bool>(true);

    bool _prevEnabled = true;

    public override void OnNetworkSpawn()
    {
        if (Manager.Instance.vrMode == false)
        {
            var xrDeviceSimulator = GetComponent<XRDeviceSimulator>();
            xrDeviceSimulator.enabled = true;
        }

        if (IsOwner)
        {
            if (IsClient)
            {
                UpdatePlayerNameServerRpc(Manager.Instance.playerName);
            }
            else
            {
                playerName.Value = Manager.Instance.playerName;
                Debug.Log("Player name: " + Manager.Instance.playerName);
            }
        }

        DisableClientInput();

        studioLight = GameObject.Find("ceiling_lamp");
        lights = studioLight.GetComponentsInChildren<Light>();
        _renderer = studioLight.GetComponent<Renderer>();
    }

    [ServerRpc]
    private void UpdatePlayerNameServerRpc(string clientPlayerName)
    {
        playerName.Value = clientPlayerName;
        Debug.Log("Player name from client: " + clientPlayerName);
    }

    public void UpdateStudioLight(bool on)
    {
        if (IsClient) {
            ToggleStudioLightServerRpc(on);
        } else {
            _enabled.Value = on;
        }
    }

    void UpdateStudioLight() {
        if (_enabled.Value)
        {
            foreach (Light light in lights)
            {
                light.enabled = true;
            }
            _renderer.materials[1].SetColor("_EmissionColor", Color.white);
            _renderer.materials[3].SetColor("_EmissionColor", Color.white);
        }
        else
        {
            foreach (Light light in lights)
            {
                light.enabled = false;
            }
            _renderer.materials[1].SetColor("_EmissionColor", Color.black);
            _renderer.materials[3].SetColor("_EmissionColor", Color.black);
        }
    }

    private void Update()
    {
        textObject.text = playerName.Value;

        if (_enabled.Value != _prevEnabled) {
            UpdateStudioLight();
            _prevEnabled = _enabled.Value;
        }
    }

    [ServerRpc]
    void ToggleStudioLightServerRpc(bool on) {
        _enabled.Value = on;
    }

    private void DisableClientInput()
    {
        if (!IsOwner)
        {
            var clientMoveProvider = GetComponent<NetworkMoveProvider>();
            var clientControllers = GetComponentsInChildren<ActionBasedController>();
            var clientTurnProvider = GetComponent<ActionBasedSnapTurnProvider>();
            var clientHead = GetComponentInChildren<TrackedPoseDriver>();
            var clientCamera = GetComponentInChildren<Camera>();
            var clientAudioListener = clientCamera.GetComponent<AudioListener>();
            var xrDeviceSimulator = GetComponent<XRDeviceSimulator>();


            var _cam = transform.Find("Camera Offset/Main Camera");
            // _cam.gameObject.SetActive(false);
            // _cam.gameObject.RemoveComponent<Camera>();
            // Destroy(_cam.GetComponent<Camera>());
            Destroy(_cam.GetComponent<AudioListener>());
            clientCamera.enabled = false;
            // clientAudioListener.enabled = false;
            clientHead.enabled = false;
            clientMoveProvider.enableInputActions = false;
            if (clientTurnProvider != null)
            {
                clientTurnProvider.enableTurnLeftRight = false;
                clientTurnProvider.enableTurnAround = false;
            }

            foreach (var controller in clientControllers)
            {
                controller.enableInputActions = false;
                controller.enableInputTracking = false;
            }

            xrDeviceSimulator.enabled = false;
        }
    }

    private void Start()
    {
        if (IsClient && IsOwner)
        {
            transform.position = new Vector3(Random.Range(placementArea.x, placementArea.y), 0f, Random.Range(placementArea.x, placementArea.y));

            //CheckPermission();

            // LanuchAgoreEngine();

            SceneSetup.Instance.InitScene();
        }
    }

    // private void LanuchAgoreEngine()
    // {
    //     mRtcEngine = IRtcEngine.GetEngine(AppID);
    //     mRtcEngine.JoinChannel(channelName, "extra", 0);
    // }

//    private void CheckPermission()
//    {
//#if (UNITY_2018_3_OR_NEWER)
//        if (Permission.HasUserAuthorizedPermission(Permission.Microphone))
//        {

//        }
//        else
//        {
//            Permission.RequestUserPermission(Permission.Microphone);
//        }
//#endif
//    }

    public void OnSelectGrabbable(SelectEnterEventArgs eventArgs)
    {
        if (IsClient && IsOwner)
        {
            NetworkObject networkObjectSelected = eventArgs.interactableObject.transform.GetComponent<NetworkObject>();

            if (networkObjectSelected != null)
            {
                // request ownership from the server
                RequestGrabbableOwnershipServerRpc(OwnerClientId, networkObjectSelected);
            }
        }
    }

    [ServerRpc]
    public void RequestGrabbableOwnershipServerRpc(ulong newOwnerClientId, NetworkObjectReference networkObjectReference)
    {
        if (networkObjectReference.TryGet(out NetworkObject networkObject))
        {
            networkObject.ChangeOwnership(newOwnerClientId);
        }
        else
        {
            Debug.Log("Unable change the ownership");
        }
    }

    void OnApplicationQuit()
    {
        // if (mRtcEngine != null)
        // {
        //     IRtcEngine.Destroy();
        // }
    }
}
