using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WristMenuController : MonoBehaviour
{
    [Header("UI")]
    public GameObject wristUI;
    public TMPro.TextMeshProUGUI joinCodeText;
    public bool activeWristUI = true;

    [Header("Machine Control")]
    public GameObject machineDisplay;
    public GameObject machine;
    public Camera machineCam;
    AudioListener machineAudioListener;
    public bool activeMachine = true;
    public Camera userCam;
    public AudioListener userAudioListener;
    public GameObject cameraPrefab;

    private GameObject _cameraObject;

    [Header("Prefabs")]
    public GameObject lightPrefab;

    // Start is called before the first frame update
    void Start()
    {
        DisplayWristUI();

        if (joinCodeText != null) {
            joinCodeText.text = Manager.Instance.joinCodeText.text;
        }

        machineAudioListener = machineCam.GetComponent<AudioListener>();



        // Default: no camera on hand
        machineCam.enabled = false;
        machineAudioListener.enabled = false;
        activeMachine = false;
        machineDisplay.SetActive(false);
        foreach (var renderer in machine.GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = false;
        }

        //userCam.enabled = true;
        userAudioListener.enabled = true;
    }

    public void CreateLight()
    {
        var obj = Instantiate(lightPrefab);
        obj.transform.position = transform.position;
    }

    void DisplayWristUI()
    {
        if (activeWristUI)
        {
            wristUI.SetActive(false);
            activeWristUI = false;
        }
        else
        {
            wristUI.SetActive(true);
            activeWristUI = true;
        }
    }

    public void SwitchCamera(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (activeMachine)
            {
                machineCam.enabled = false;
                machineAudioListener.enabled = false;
                activeMachine = false;
                machineDisplay.SetActive(false);
                foreach (var renderer in machine.GetComponentsInChildren<Renderer>())
                {
                    renderer.enabled = false;
                }

                //userCam.enabled = true;
                userAudioListener.enabled = true;
            }
            else
            {
                machineCam.enabled = true;
                machineAudioListener.enabled = true;
                activeMachine = true;
                machineDisplay.SetActive(true);
                foreach (var renderer in machine.GetComponentsInChildren<Renderer>())
                {
                    renderer.enabled = true;
                }

                //userCam.enabled = false;
                userAudioListener.enabled = false;
            }
        }
    }

    public void PutCamera(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (!_cameraObject)
            {
                _cameraObject = Instantiate(cameraPrefab);
            }
            _cameraObject.transform.position = transform.position;
            _cameraObject.transform.rotation = transform.rotation;
        }
    }
}
