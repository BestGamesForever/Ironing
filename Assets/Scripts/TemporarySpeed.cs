using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Deform;

public class TemporarySpeed : MonoBehaviour
{
    SimplexNoiseDeformer deformable;
    public static float _offsetvector;
    void Start()
    {
        deformable = GetComponent<SimplexNoiseDeformer>();
        _offsetvector = 0;
    }

    
    void Update()
    {
        deformable.OffsetSpeedVector = new Vector4(0, 0, 0, _offsetvector);

    }
}
