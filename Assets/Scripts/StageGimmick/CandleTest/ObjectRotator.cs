using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class ObjectRotator : MonoBehaviour
{
    [SerializeField]
    private float angle = 340;
    [SerializeField]
    private float rotationSpeed = 50f;

    [SerializeField] private GameObject RotatableObject;
    [SerializeField] private GameObject RotatableObject2nd;
    private float _currentAngle = 0;
    private float _inputHori = 0;

    private bool _isEnabled = false;

    [SerializeField] private UnityEvent _onCanceledAciton;

    [SerializeField] CinemachineVirtualCamera _cam;
    public void ChangeRotateEnable()
    {
        _cam.Priority = 9999;
        PlayerEventHelper.SetPlayerStateAsOperatingPlatform(true);
        _isEnabled = !_isEnabled;
    }
    void FixedUpdate()
    {
        if(_isEnabled && PlayerEventHelper.IsExceptionalState())
        {
            _inputHori = InputReader.Instance.MovementInput.x;
            Rotate();
        }
        if(_isEnabled && !PlayerEventHelper.IsExceptionalState())
        {
            _isEnabled = false;
            _cam.Priority = -9999;
            _onCanceledAciton?.Invoke();
        }
    }
    void Rotate()
    {
        _currentAngle += _inputHori * rotationSpeed * Time.deltaTime;

        _currentAngle = Mathf.Clamp(_currentAngle, 0, angle);
        RotatableObject.transform.eulerAngles = new Vector3(
            RotatableObject.transform.eulerAngles.x,
            _currentAngle,
            RotatableObject.transform.eulerAngles.z);

        RotatableObject2nd.transform.eulerAngles = new Vector3(
            RotatableObject2nd.transform.eulerAngles.x,
            _currentAngle,
            RotatableObject2nd.transform.eulerAngles.z);
    }
}
