using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Animations;

public class RegionController : MonoBehaviour
{
    public GameObject objectPanel;

    CharacterController controller = null;

    private void OnTriggerEnter(Collider other) {

        if (other.TryGetComponent(out controller)) {
            objectPanel.SetActive(true);
        }

    }

    private void Update() {
        if (controller != null) {
            XROrigin origin;
            if (controller.TryGetComponent(out origin)) {
                objectPanel.transform.position = new Vector3(objectPanel.transform.position.x, origin.Camera.transform.position.y, objectPanel.transform.position.z);
                var constraintSource = new ConstraintSource();
                constraintSource.weight = 1.0f;
                constraintSource.sourceTransform = origin.Camera.transform;
                objectPanel.GetComponent<LookAtConstraint>().SetSource(0, constraintSource);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.TryGetComponent(out controller)) {
            objectPanel.SetActive(false);
            this.controller = null;
        }
    }
}
