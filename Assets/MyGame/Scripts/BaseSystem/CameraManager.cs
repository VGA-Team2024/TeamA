using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カメラを制御するクラス
/// </summary>
public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private float _camMoveSpeed = 1;
    private Vector3 _right;
    private Vector3 _forward = Vector3.forward;
    
    // Start is called before the first frame update
    void Start()
    {
        _right = transform.right.normalized;
        var forwardVector = transform.forward;
        forwardVector.y = 0;
        _forward = forwardVector.normalized;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Time.deltaTime * _forward * _camMoveSpeed;
        }
        
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= Time.deltaTime * _forward * _camMoveSpeed;
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            transform.position -= Time.deltaTime * _right * _camMoveSpeed;
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Time.deltaTime * _right * _camMoveSpeed;
        }
    }
}
