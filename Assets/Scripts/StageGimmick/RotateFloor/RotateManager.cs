using Alchemy.Inspector;
using Cinemachine;
using System.Collections.Generic;
using UnityEngine;

public class RotateManager : MonoBehaviour
{
    [DisableInPlayMode][SerializeField] CinemachineVirtualCamera _cam;
    [DisableInPlayMode][SerializeField] List<RotatableObject> _rotateFloorDataList;
    [SerializeField] private float _forwardLineLength = 10;
    [SerializeField] private float _rotateSpeedCoefficient = 10;
    [SerializeField] private bool _isEnableGimmick = false;
    private float _inputHori;
    void Start()
    {
        if (!_cam)
        {
            this.gameObject.SetActive(false);
        }
        _cam.Priority = -999;
        //NullCheck
        if (_rotateFloorDataList.Count == 0)
        {
            this.gameObject.SetActive(false);
        }
        for (int i = 0; i < _rotateFloorDataList.Count; i++)
        {
            if (!_rotateFloorDataList[i])
            {
                this.gameObject.SetActive(false);
            }
        }

        //ªŒ³‚Ì‚â‚Â‚¾‚¯‹N“®
        //_rotateFloorDataList[0]._isRotatable = true;
    }
    public void OnInputVec2()
    {
        _inputHori = Input.GetAxisRaw("Horizontal");
    }
    private void FixedUpdate()
    {
        if (!_isEnableGimmick)
        {
            return;
        }
        CheckSituation(_inputHori);
        Rotate();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            _isEnableGimmick = !_isEnableGimmick;
            Debug.Log($"RotateGimmick : {_isEnableGimmick}");
        }
        if (!_isEnableGimmick)
        {
            return;
        }

        OnInputVec2();
    }
    private void CheckSituation(float inputAxis)
    {
        for (int i = 0; i < _rotateFloorDataList.Count; i++)
        {
            _rotateFloorDataList[i].CheckRotateFloor();
            _rotateFloorDataList[i].CheckObstacle(inputAxis);
        }
    }
    private void Rotate()
    {
        for (int i = 0; i < _rotateFloorDataList.Count && _rotateFloorDataList[i].IsRotatable(_inputHori); i++)
        {/*
            if (_rotateFloorDataList[i]._isRotatable == true)
            {
                _rotateFloorDataList[i].transform.RotateAround(transform.position, transform.forward, _inputHori * _rotateSpeedCoefficient);
            }*/
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * _forwardLineLength);
    }
}