using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class NetworkMoveProvider : ActionBasedContinuousMoveProvider
{
    public bool enableInputActions;

    protected override Vector2 ReadInput() {
        if (enableInputActions) {
            return base.ReadInput();
        } else {
            return Vector2.zero;
        }
    }
}
