using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraVisualizer : MonoBehaviour
{
    public Camera targetCamera;
    public LineRenderer Plane1;
    public LineRenderer Plane2;
    public LineRenderer Plane3;
    public LineRenderer Plane4;


    // Start is called before the first frame update
    void Start()
    {
        Draw();
    }

    void Update()
    {
        Draw();
    }

    void Draw()
    {
        Vector3[] frustumCornersFar = new Vector3[4];
        Vector3[] frustumCornersNear = new Vector3[4];
        targetCamera.CalculateFrustumCorners(new Rect(0, 0, 1, 1), targetCamera.farClipPlane, Camera.MonoOrStereoscopicEye.Mono, frustumCornersFar);
        targetCamera.CalculateFrustumCorners(new Rect(0, 0, 1, 1), targetCamera.nearClipPlane, Camera.MonoOrStereoscopicEye.Mono, frustumCornersNear);

        Plane1.positionCount = 4;
        Plane1.useWorldSpace = false;
        Plane1.SetPositions(new Vector3[] { frustumCornersNear[0], frustumCornersNear[1], frustumCornersFar[1], frustumCornersFar[0], });

        Plane2.positionCount = 4;
        Plane2.useWorldSpace = false;
        Plane2.SetPositions(new Vector3[] { frustumCornersNear[1], frustumCornersNear[2], frustumCornersFar[2], frustumCornersFar[1], });

        Plane3.positionCount = 4;
        Plane3.useWorldSpace = false;
        Plane3.SetPositions(new Vector3[] { frustumCornersNear[2], frustumCornersNear[3], frustumCornersFar[3], frustumCornersFar[2], });
        
        Plane4.positionCount = 4;
        Plane4.useWorldSpace = false;
        Plane4.SetPositions(new Vector3[] { frustumCornersNear[3], frustumCornersNear[0], frustumCornersFar[0], frustumCornersFar[3], });
    }
}
