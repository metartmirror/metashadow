using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class StudioLightController : NetworkBehaviour
{
    public NetworkPlayer player;

    public void ToggleStudioLight(bool on) {
        player.UpdateStudioLight(on);
    }
}
