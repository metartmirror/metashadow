using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class LightAngleController : NetworkBehaviour
{
    // public TMPro.TextMeshPro angleText;

    InputDevice _device_leftController;
    InputDevice _device_rightController;

    private Vector2 _inputAxis_leftController;

    private Vector2 _inputAxis_rightController;


    Quaternion prevRot;
    bool isSelecting = false;
    bool isFirstSelected = false;

    NetworkVariable<Quaternion> Rotation = new NetworkVariable<Quaternion>(Quaternion.identity);

    private void Start()
    {
        // angleText.gameObject.SetActive(false);


        _device_leftController = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        _device_rightController = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelecting)
        {
            // angleText.text = Quaternion.Angle(prevRot, transform.rotation).ToString("0") + "°";
            _device_leftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out _inputAxis_leftController); // left stick
            _device_rightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out _inputAxis_rightController); //right stick

            // right hand only
            if (_inputAxis_rightController.magnitude > 0.1f)
            {
                if (IsClient) {
                    RequestRotationServerRpc(_inputAxis_rightController);
                }
                else {
                    transform.Rotate(Vector3.right, _inputAxis_rightController.x * Time.deltaTime * 100);
                }
                //transform.Rotate(Vector3.right, _inputAxis_rightController.x * Time.deltaTime * 100);
                // transform.position += transform.forward * _inputAxis_rightController.y * Time.deltaTime * 10;
            }
        }
        if (IsClient) {
            transform.localRotation = Rotation.Value;
        }
        //Debug.Log(Rotation.Value);
    }

    [ServerRpc(RequireOwnership = false)]
    void RequestRotationServerRpc(Vector2 v) {
        transform.Rotate(Vector3.right, v.x * Time.deltaTime * 100);
        Rotation.Value = transform.localRotation;
    }

    public void onSelect()
    {
        isSelecting = true;
        //prevRot = transform.rotation;


        // angleText.gameObject.SetActive(true);
    }

    public void onDeselect()
    {
        isSelecting = false;

        // angleText.gameObject.SetActive(false);
    }
}
