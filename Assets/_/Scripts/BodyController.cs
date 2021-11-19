using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    [SerializeField] private float scaleBoundaryUp;
    [SerializeField] private float scaleBoundaryDown;
    [SerializeField] private float scaleSpeed;
    private bool doGrow;
    
    private GameObject target;

    private void Update()
    {
        UpdateScaling();
    }

    private void UpdateScaling()
    {
        var tf = transform;
        var scale = tf.localScale.x;

        if (scale > scaleBoundaryUp)
        {
            scale = scaleBoundaryUp;
            doGrow = false;
        }
        else if (scale < scaleBoundaryDown)
        {
            scale = scaleBoundaryDown;
            doGrow = true;
        }

        if (doGrow)
        {
            tf.localScale = Vector3.one * (scale + scaleSpeed * Time.deltaTime);
        }
        else
        {
            tf.localScale = Vector3.one * (scale - scaleSpeed * Time.deltaTime);
        }
    }
}
