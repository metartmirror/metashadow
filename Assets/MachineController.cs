using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineController : MonoBehaviour
{
    public List<RenderTexture> textures;
    public RawImage display;

    [SerializeField] int currIndex = 0;

    // Start is called before the first frame update
    void Start() {
    }

    void UpdateMachine() {
        display.texture = textures[currIndex];
        // RenderSettings.skybox = data[currIndex].material;
    }

    public void NextBackground() {
        currIndex = (currIndex + 1) % textures.Count;
        UpdateMachine();
    }

    public void PreviousBackground() {
        currIndex = (currIndex - 1) % textures.Count;
        UpdateMachine();
    }
}
