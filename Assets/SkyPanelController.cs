using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class SkyPanelController : NetworkBehaviour
{
    public GameObject obj;

    private NetworkVariable<float> red = new NetworkVariable<float>(1f);
    private NetworkVariable<float> green = new NetworkVariable<float>(1f);
    private NetworkVariable<float> blue = new NetworkVariable<float>(1f);

    private NetworkVariable<float> intensity = new NetworkVariable<float>(10f);

    float _prevRed = 1f;
    float _prevGreen = 1f;
    float _prevBlue = 1f;
    float _prevIntensity = 10f;

    public Slider m_rSlider;
    public Slider m_gSlider;
    public Slider m_bSlider;

    Light _light;
    Renderer _renderer;

    void Start()
    {
        _light = obj.GetComponent<Light>();
        _renderer = obj.GetComponent<Renderer>();
    }
    public void ChangeRed(float value)
    {
        if (IsClient)
        {
            ChangeRedServerRpc(value);
        }
        else
        {
            red.Value = value;
            UpdateColor();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void ChangeRedServerRpc(float value)
    {
        red.Value = value;
    }

    public void ChangeBlue(float value)
    {
        if (IsClient)
        {
            ChangeBlueServerRpc(value);
        }
        else
        {
            blue.Value = value;
            UpdateColor();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void ChangeBlueServerRpc(float value)
    {
        blue.Value = value;
    }

    public void ChangeGreen(float value)
    {
        if (IsClient)
        {
            ChangeGreenServerRpc(value);
        }
        else
        {
            green.Value = value;
            UpdateColor();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void ChangeGreenServerRpc(float value)
    {
        green.Value = value;
    }

    public void ChangeIntensity(float value)
    {
        if (IsClient)
        {
            ChangeIntensityServerRpc(value);
        }
        else
        {
            intensity.Value = value;
            UpdateColor();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    void ChangeIntensityServerRpc(float value)
    {
        intensity.Value = value;
    }

    private void UpdateColor()
    {
        _light.color = new Color(red.Value, green.Value, blue.Value);
        _renderer.material.SetVector("_EmissionColor", new Vector4(1f, 1f, 1f, 1f) * Mathf.Lerp(0, 10, intensity.Value / 10f));
    }

    private void Update()
    {
        if (_prevBlue != blue.Value)
        {
            _prevBlue = blue.Value;
            UpdateColor();
            m_bSlider.onValueChanged.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.Off);
            m_bSlider.value = blue.Value;
            m_bSlider.onValueChanged.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.RuntimeOnly);
        }

        if (_prevGreen != green.Value)
        {
            _prevGreen = green.Value;
            UpdateColor();
            m_gSlider.onValueChanged.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.Off);
            m_gSlider.value = green.Value;
            m_gSlider.onValueChanged.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.RuntimeOnly);
        }

        if (_prevRed != red.Value)
        {
            _prevRed = red.Value;
            UpdateColor();
            m_rSlider.onValueChanged.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.Off);
            m_rSlider.value = red.Value;
            m_rSlider.onValueChanged.SetPersistentListenerState(0, UnityEngine.Events.UnityEventCallState.RuntimeOnly);
        }

        if (_prevIntensity != intensity.Value)
        {
            _prevIntensity = intensity.Value;
            UpdateColor();
        }
    }
}
