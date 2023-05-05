using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaVisualizer : MonoBehaviour
{
    public Light targetLight;
    public LineRenderer centerLine;
    public LineRenderer boundaryLineLeft;
    public LineRenderer boundaryLineRight;
    public LineRenderer endCircleLine;
    public LineRenderer boundaryLineTop;
    public LineRenderer boundaryLineBottom;

    public LineRenderer forwardLine;

    public Transform forwardTransform;
    public TMPro.TextMeshPro text;

    private int numOfPoints = 100;

    private float prevRange = 0;
    private float prevAngleInRadians = 0;

    private void Start()
    {
        prevAngleInRadians = targetLight.spotAngle * Mathf.Deg2Rad;
        prevRange = targetLight.range;
        Draw();
    }

    private void Update()
    {
        // if (prevAngleInRadians != targetLight.spotAngle * Mathf.Deg2Rad || prevRange != targetLight.range)
        // {
        Draw();
        //     prevAngleInRadians = targetLight.spotAngle * Mathf.Deg2Rad;
        //     prevRange = targetLight.range;
        // }
    }

    private void Draw()
    {
        var m = transform.localToWorldMatrix;
        Vector3 position = m.GetColumn(3);
        Quaternion rotation = Quaternion.LookRotation(
            m.GetColumn(2),
            m.GetColumn(1)
        );
        m = Matrix4x4.TRS(position, rotation, Vector3.one);

        forwardLine.positionCount = 2;
        forwardLine.useWorldSpace = true;
        forwardLine.SetPosition(0, m.MultiplyPoint(new Vector3(0, 0, 0)));
        forwardLine.SetPosition(1, m.MultiplyPoint(new Vector3(0, 0, 0)) + forwardTransform.up * targetLight.range);

        centerLine.positionCount = 2;
        centerLine.useWorldSpace = true;
        centerLine.SetPosition(0, m.MultiplyPoint(new Vector3(0, 0, 0)));
        centerLine.SetPosition(1, m.MultiplyPoint(new Vector3(0, 0, targetLight.range)));

        text.text = Vector3.Angle(transform.forward, forwardTransform.up).ToString("0")+"°";

        var angleInRadians = targetLight.spotAngle * Mathf.Deg2Rad;
        var radius = targetLight.range * Mathf.Sin(angleInRadians / 2);
        var height = targetLight.range * Mathf.Cos(angleInRadians / 2);

        boundaryLineLeft.positionCount = 2;
        boundaryLineLeft.useWorldSpace = true;
        boundaryLineLeft.SetPosition(0, m.MultiplyPoint(new Vector3(0, 0, 0)));
        boundaryLineLeft.SetPosition(1, m.MultiplyPoint(new Vector3(radius, 0, height)));

        boundaryLineRight.positionCount = 2;
        boundaryLineRight.useWorldSpace = true;
        boundaryLineRight.SetPosition(0, m.MultiplyPoint(new Vector3(0, 0, 0)));
        boundaryLineRight.SetPosition(1, m.MultiplyPoint(new Vector3(-radius, 0, height)));

        endCircleLine.positionCount = numOfPoints;
        endCircleLine.useWorldSpace = true;
        endCircleLine.loop = true;
        for (int i = 0; i < numOfPoints; i++)
        {
            endCircleLine.SetPosition(i, m.MultiplyPoint(new Vector3(radius * Mathf.Sin(i * 2 * Mathf.PI / numOfPoints), radius * Mathf.Cos(i * 2 * Mathf.PI / numOfPoints), height)));
        }

        var angleInRadiansInner = targetLight.innerSpotAngle * Mathf.Deg2Rad;
        var radiusInner = targetLight.range * Mathf.Sin(angleInRadiansInner / 2);
        var heightInner = targetLight.range * Mathf.Cos(angleInRadiansInner / 2);
        boundaryLineTop.positionCount = 2;
        boundaryLineTop.useWorldSpace = true;
        boundaryLineTop.SetPosition(0, m.MultiplyPoint(new Vector3(0, 0, 0)));
        boundaryLineTop.SetPosition(1, m.MultiplyPoint(new Vector3(0, radiusInner, heightInner)));

        boundaryLineBottom.positionCount = 2;
        boundaryLineBottom.useWorldSpace = true;
        boundaryLineBottom.SetPosition(0, m.MultiplyPoint(new Vector3(0, 0, 0)));
        boundaryLineBottom.SetPosition(1, m.MultiplyPoint(new Vector3(0, -radiusInner, heightInner)));
    }
}
