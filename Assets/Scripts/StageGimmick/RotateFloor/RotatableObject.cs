using Alchemy.Inspector;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class RotatableObject : MonoBehaviour
{
    private bool _isRotatable = false;
    private float _obstacledInput = 0;
    private GameObject _parent;
    [SerializeField] private float _adjustOverlapBoxRange = 0.1f;
    private int _obstacleOrRotateLayer;
    private int _floorLayer;
    private void Start()
    {
        _obstacleOrRotateLayer = 1 << 10;
        _floorLayer = 1 << 11;
    }
    public void SetParent(GameObject gameObject)
    {
        _parent = gameObject;
    }
    public void CheckObstacle(float rotateInput)
    {
        if (CheckRotateFloor())
        {
            if (_obstacledInput != 0 && rotateInput != 0 && Mathf.Sign(_obstacledInput) != Mathf.Sign(rotateInput))
            {
                _isRotatable = true;
                _obstacledInput = 0;
                return;
            }
        }
        if (Physics.OverlapBox(this.transform.position,
            this.transform.localScale * (0.5f + _adjustOverlapBoxRange),
            transform.rotation, _obstacleOrRotateLayer, QueryTriggerInteraction.Collide).Length > 0)
        {
            _isRotatable = false;
            if (rotateInput != 0 && _obstacledInput == 0)
            {
                _obstacledInput = rotateInput > 0 ? 1 : -1;
                Debug.Log($"{this.gameObject.name} : {_obstacledInput}");
            }
        }
    }
    public bool CheckRotateFloor()
    {
        if (Physics.OverlapBox(this.transform.position,
            this.transform.localScale * (0.5f + _adjustOverlapBoxRange),
            transform.rotation, _floorLayer, QueryTriggerInteraction.Collide).Length > 1)
        {
            _isRotatable = true;
            return true;
        }
        return false;
    }
    /// <summary>
    /// 回せない状態かつ入力が前回止まっていた入力であるならfalse、それ以外ならtrueを返す
    /// </summary>
    /// <param name="rotateInput">
    /// 回転用の入力
    /// </param>
    public bool IsRotatable(float rotateInput)
    {
        if (!_isRotatable && (rotateInput - _obstacledInput) * (rotateInput - _obstacledInput) < 2)
        {
            return false;
        }
        return true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        var normalMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, transform.localScale * (1 + _adjustOverlapBoxRange));
        Gizmos.matrix = normalMatrix;
    }
}
