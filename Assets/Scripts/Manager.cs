using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;
using UnityEngine.XR;
using System.Linq;

public enum NetworkMode
{
    Client,
    Host,
    Server
}

public class Manager : Singleton<Manager>
{
    [Header("UI")]
    public Button hostBtn;
    public Button serverBtn;
    public Button clientBtn;
    public TextMeshProUGUI welcomeText;
    public TextMeshProUGUI joinCodeText;
    public TMP_InputField inputField;
    public GameObject joinPanel;
    public ToggleGroup toggleGroup;

    [Header("Network")]
    public NetworkMode networkMode;
    public string joinCode;
    public string playerName = "Teacher";

    [Header("VR")]
    public bool vrMode = true;

    private void Start()
    {
        if (hostBtn != null) hostBtn.onClick.AddListener(() => StartHost());
        if (clientBtn != null) clientBtn.onClick.AddListener(() => StartClient());
        if (serverBtn != null) serverBtn.onClick.AddListener(() => StartServer());

        DetectDevice();

        foreach (var toggle in toggleGroup.GetComponentsInChildren<Toggle>())
        {
            toggle.onValueChanged.AddListener((value) =>
            {
                // Debug.Log("Toggle value: " + value);
                // Debug.Log("Toggle value: " + toggleGroup.ActiveToggles().FirstOrDefault().GetComponentInChildren<TMPro.TextMeshProUGUI>().text);
                playerName = toggleGroup.ActiveToggles().FirstOrDefault().GetComponentInChildren<TMPro.TextMeshProUGUI>().text;
            });
        }
    }

    private void DetectDevice() {
        //Check if initialization was successfull.
        var xrDisplaySubsystems = new List<XRDisplaySubsystem>();
        SubsystemManager.GetInstances(xrDisplaySubsystems);

        if (xrDisplaySubsystems.Count == 0) {
            //Initialization was not successfull, load mock instead.
            print("loading mock");
            vrMode = false;
        }
    }

    void StartHost()
    {
        networkMode = NetworkMode.Host;
        SceneManager.LoadScene(1);
        joinPanel.SetActive(false);
    }

    void StartClient()
    {
        networkMode = NetworkMode.Client;
        joinCode = inputField.text;
        if (!string.IsNullOrEmpty(joinCode)) {
            SceneManager.LoadScene(1);
            joinPanel.SetActive(false);
        }
    }

    void StartServer()
    {
        networkMode = NetworkMode.Server;
        SceneManager.LoadScene(1);
        joinPanel.SetActive(false);
    }
}
