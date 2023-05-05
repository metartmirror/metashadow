using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public List<BackgroundData> data;

    [SerializeField] GameObject backgroundPrefab;
    [SerializeField] int currIndex = 0;

    // Start is called before the first frame update
    void Start() {
        backgroundPrefab = GameObject.Find("Outside_LED");

        UpdateCurrentBackground();
    }

    void UpdateCurrentBackground() {
        backgroundPrefab.GetComponent<MeshRenderer>().material = data[currIndex].material;
        // RenderSettings.skybox = data[currIndex].material;
    }

    public void NextBackground() {
        currIndex = (currIndex + 1) % data.Count;
        UpdateCurrentBackground();
    }

    public void PreviousBackground() {
        currIndex = (currIndex - 1) % data.Count;
        UpdateCurrentBackground();
    }
}
