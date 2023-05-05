using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HeightConstriant : MonoBehaviour
{
    public float initHeight = 0;
    public float maxHeight = 0;
    public float minHeight = 0;

    // Start is called before the first frame update
    void Start()
    {
        transform.localPosition = new Vector3(0, 0, initHeight);
    }

    // Update is called once per frame
    void Update()
    {
        // only allow to change the y position
        if (transform.localPosition.z < minHeight)
        {
            transform.localPosition = new Vector3(0, 0, minHeight);
        }
        if (transform.localPosition.z > maxHeight)
        {
            transform.localPosition = new Vector3(0, 0, maxHeight);
        } else {
            transform.localPosition = new Vector3(0, 0, transform.localPosition.z);
        }
    }
}
