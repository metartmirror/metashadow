using Cinemachine;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
    public GameObject PlayerCameraRoot;

    public override void OnNetworkSpawn() {
        if (IsOwner) {
            Move();
            GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>().Follow = PlayerCameraRoot.transform;
        }
    }

    public void Move() {
        if (NetworkManager.Singleton.IsServer) {
            var randomPosition = GetRandomPositionOnPlane();
            transform.position = randomPosition;
            Position.Value = randomPosition;
        }
        else {
            SubmitPositionRequestServerRpc();
        }
    }

    [ServerRpc]
    void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default) {
        Position.Value = GetRandomPositionOnPlane();
    }

    static Vector3 GetRandomPositionOnPlane() {
        return new Vector3(Random.Range(-3f, 3f), 3f, Random.Range(-3f, 3f));
    }

    //void Update() {
    //    transform.position = Position.Value;
    //}
}
