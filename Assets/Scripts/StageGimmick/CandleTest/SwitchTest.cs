using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwitchTest : MonoBehaviour
{
    [SerializeField] private UnityEvent OnActivated;
    private bool _hasProcessed = false;
    [SerializeField] private Vector3 _halfExtents; //スイッチの大きさの半径
    [SerializeField] private LayerMask playerLayer;
    private Vector3 _switchCenterOffset = new Vector3(0, 0.5f, 0);
   // public event Action OnSwitchPressed; //ドア開閉通知
    private void FixedUpdate()
    {
        if (_hasProcessed || !gameObject) return;

        if (IsPlayerOnSwitch())
        {
            Debug.Log("PlayerOnSwitchGoal");
            OnActivated?.Invoke();
            _hasProcessed = true;
        }
    }
    private bool IsPlayerOnSwitch()
    {
        RaycastHit hit;
        return Physics.BoxCast(transform.position
            , new Vector3(_halfExtents.x, 0.01f, _halfExtents.z)
            , Vector3.up
            , out hit
            , Quaternion.identity
            , _halfExtents.y * 2, playerLayer.value);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + new Vector3(0, _halfExtents.y, 0), _halfExtents * 2);
    }
}
