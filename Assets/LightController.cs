using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class LightController : NetworkBehaviour
{
    public TMPro.TextMeshPro angleText;

    InputDevice _device_leftController;
    InputDevice _device_rightController;

    private Vector2 _inputAxis_leftController;

    private Vector2 _inputAxis_rightController;



    //public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
    //public NetworkVariable<Quaternion> Rotation = new NetworkVariable<Quaternion>();

    //public override void OnNetworkSpawn() {
    //    Position.Value = transform.position;
    //    Rotation.Value = transform.rotation;
    //}

    //public void SetPosition(Vector3 position) {
    //    NetworkLog.LogInfoServer(position.ToString());
    //    if (IsServer) {
    //        transform.position = position;
    //        Position.Value = position;
    //    } else if (IsClient) {
    //        SetPositionServerRpc(position);
    //    }
    //}

    //[ServerRpc(RequireOwnership = false)]
    //public void SetPositionServerRpc(Vector3 position, ServerRpcParams rpcParams = default) {
    //    Position.Value = position;
    //    NetworkLog.LogInfoServer("SetPositionServerRpc");
    //    NetworkLog.LogInfoServer(Position.Value.ToString());
    //}


    Quaternion prevRot;
    bool isSelecting = false;

    private void Start()
    {
        angleText.gameObject.SetActive(false);


        _device_leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        _device_rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }

    //[ServerRpc(RequireOwnership = false)]
    //void SubmitPositionRotationRequestServerRpc(Vector2 movement, ServerRpcParams rpcParams = default) {
    //    transform.Rotate(Vector3.up, movement.x * Time.deltaTime * 100);
    //    transform.position += transform.forward * movement.y * Time.deltaTime * 1;
    //    //Position.Value = transform.position;
    //    //Rotation.Value = transform.rotation;
    //}

    // Update is called once per frame
    void Update()
    {
        if (isSelecting)
        {
            angleText.text = Quaternion.Angle(prevRot, transform.rotation).ToString("0") + "°";
            _device_leftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out _inputAxis_leftController); // left stick
            _device_rightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out _inputAxis_rightController); //right stick

            // right hand only
            if (_inputAxis_rightController.magnitude > 0.1f)
            {
                transform.Rotate(Vector3.up, _inputAxis_rightController.x * Time.deltaTime * 100);
                transform.position += transform.forward * _inputAxis_rightController.y * Time.deltaTime * 1;
                //if (IsClient) {
                //    SubmitPositionRotationRequestServerRpc(_inputAxis_rightController);
                //}
            }
        }

        //if (IsClient) {
        //    transform.SetPositionAndRotation(Position.Value, Rotation.Value);
        //}

    }

    public void onSelect()
    {
        isSelecting = true;
        prevRot = transform.rotation;

        angleText.gameObject.SetActive(true);
    }

    public void onDeselect()
    {
        isSelecting = false;

        angleText.gameObject.SetActive(false);
    }
}
