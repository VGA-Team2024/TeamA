using Alchemy.Inspector;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public class RotatableObject : MonoBehaviour, IResetable
{
    private float _obstacledInput = 0;
    private GameObject _parent;
    [SerializeField] private float _adjustOverlapBoxRange = 0.1f;
    private int _obstacleOrRotateLayer;
    private int _floorLayer;
    private Vector3 _initialPosition;
    private Quaternion _initialRotation;

    bool _isContainParent = false;
    bool _isObstacled = false;
    private void Start()
    {
        _initialPosition = transform.position;
        _initialRotation = transform.rotation;
        _obstacleOrRotateLayer = 1 << 10;
        _floorLayer = 1 << 11;
    }
    public void SetParent(GameObject gameObject)
    {
        _parent = gameObject;
    }
    private void CheckObstacles(float rotateInput)
    {
        if (Physics.OverlapBox(this.transform.position,
            this.transform.localScale * (0.5f + _adjustOverlapBoxRange),
            transform.rotation, _obstacleOrRotateLayer, QueryTriggerInteraction.Collide).Length > 0)
        {
            //後天的なら入力値保存
            if (rotateInput != 0 && !_isObstacled)
            {
                _obstacledInput = rotateInput > 0 ? 1 : -1;
                Debug.Log($"{this.gameObject.name} : {_obstacledInput}");
            }
            _isObstacled = true;
        }
        else
        {
            _isObstacled = false;
        }
    }
    private void CheckRotateFloor()
    {
        if (Physics.OverlapBox(this.transform.position,
            this.transform.localScale * (0.5f + _adjustOverlapBoxRange),
            transform.rotation, _floorLayer, QueryTriggerInteraction.Collide).Any(x => x.gameObject == _parent))
        {
            _isContainParent = true;
        }
        else
        {
            _isContainParent = false;
        }
    }
    /// <summary>
    /// 回せない状態かつ入力が前回止まっていた入力であるならfalse、それ以外ならtrueを返す
    /// </summary>
    /// <param name="rotateInput">
    /// 回転用の入力
    /// </param>
    public bool IsRotatable(float rotateInput)
    {
        CheckObstacles(rotateInput);
        CheckRotateFloor();
        
        if(!_isContainParent)
        {
            return false;
        }
        if(_isObstacled && (rotateInput - _obstacledInput) * (rotateInput - _obstacledInput) < 2)
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

    public void RegisterReset()
    {
        try
        {
            GimmickResetManager[] objects = FindObjectsByType<GimmickResetManager>(FindObjectsSortMode.None);
            foreach (var resetManager in objects)
            {
                resetManager._resetAction += ResetGimmick;
            }
        }
        catch
        {
            Debug.Log($"{this.gameObject.name} can't register ResetGimmick ");
        }
    }

    public void ResetGimmick()
    {
        transform.position = _initialPosition;
        transform.rotation = _initialRotation;
    }

    public void CancelletionReset()
    {
        try
        {
            GimmickResetManager[] objects = FindObjectsByType<GimmickResetManager>(FindObjectsSortMode.None);
            foreach (var resetManager in objects)
            {
                resetManager._resetAction -= ResetGimmick;
            }
        }
        catch
        {
            Debug.Log($"{this.gameObject.name} can't remove ResetGimmick ");
        }
    }
}
