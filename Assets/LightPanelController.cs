using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.Netcode;
using UnityEngine.UIElements;

public class LightPanelController : NetworkBehaviour
{
    public List<GameObject> lightPrefabs;
    public XRBaseInteractor interactor;
    public CharacterController characterController;

    public void CreateLight(int index) {
        Vector3 temp = characterController.transform.position + characterController.transform.TransformDirection(Vector3.forward) + new Vector3(0, 0f, 0);
        if (IsServer || IsHost) {
            var obj = Instantiate(lightPrefabs[index]);
            obj.GetComponent<NetworkObject>().Spawn();
            obj.transform.position = temp;
            //obj.GetComponent<LightController>().SetPosition(temp);
        } else {
            SpwanLightServerRpc(index, temp);
        }
        
        // interactor.interactionManager.SelectEnter(interactor, obj.GetComponent<IXRSelectInteractable>());
    }

    [ServerRpc]
    void SpwanLightServerRpc(int index, Vector3 position) {
        var obj = Instantiate(lightPrefabs[index]);
        obj.GetComponent<NetworkObject>().Spawn();
        obj.transform.position = position;
        //obj.GetComponent<LightController>().SetPosition(position);
    }
}
