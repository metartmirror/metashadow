using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SceneSetup : Singleton<SceneSetup>
{
    public GameObject floor;
    public GameObject roomFloor;
    public GameObject stairs;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitScene() {
        Debug.Log("set up scene -- init scene");

        if (floor != null) floor.AddComponent<TeleportationArea>();
        if (roomFloor != null) roomFloor.AddComponent<TeleportationArea>();
        if (stairs != null) stairs.AddComponent<TeleportationArea>();
    }
}
