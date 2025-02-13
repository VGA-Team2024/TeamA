using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class Capturaing : OverlapDetectionSystem<Capturable>
{
#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        Gizmos.matrix = Matrix4x4.TRS(transform.position + _offset, transform.rotation, transform.lossyScale);
        Gizmos.DrawWireSphere(Vector3.zero, _radius);
    }
#endif
}