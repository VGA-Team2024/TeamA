using Alchemy.Inspector;
using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RotateManager : MonoBehaviour
{
    [DisableInPlayMode][SerializeField] CinemachineVirtualCamera _cam;
    [DisableInPlayMode][SerializeField] List<RotatableObject> _rotateFloorDataList;
    [SerializeField] private float _forwardLineLength = 10;
    [SerializeField] private float _rotateSpeedCoefficient = 10;
    [SerializeField] private bool _isEnableGimmick = false;
    private float _inputHori;
    [SerializeField] private UnityEvent _onCanceledAciton;
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

        _rotateFloorDataList[0].SetParent(_rotateFloorDataList[0].gameObject);
        for(int i = 1; i < _rotateFloorDataList.Count; i++)
        {
            _rotateFloorDataList[i].SetParent(_rotateFloorDataList[i - 1].gameObject);
        }
    }
    private void FixedUpdate()
    {
        if (_isEnableGimmick && PlayerEventHelper.IsExceptionalState())
        {
            _inputHori = InputReader.Instance.MovementInput.x;
            Rotate();
        }
        if (_isEnableGimmick && !PlayerEventHelper.IsExceptionalState())
        {
            _isEnableGimmick = false;
            _cam.Priority = -9999;
            _onCanceledAciton?.Invoke();
        }
    }
    public void EnableRotate()
    {
        _cam.Priority = 9999;
        PlayerEventHelper.SetPlayerStateAsOperatingPlatform(true);
        _isEnableGimmick = true;
    }
    private void Rotate()
    {
        for (int i = 0; i < _rotateFloorDataList.Count && _rotateFloorDataList[i].IsRotatable(_inputHori); i++)
        {
            _rotateFloorDataList[i].transform.RotateAround(transform.position, transform.forward, _inputHori * _rotateSpeedCoefficient);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * _forwardLineLength);
    }
}