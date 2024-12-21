using UnityEngine;

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
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            _isEnabled = !_isEnabled;
            Debug.Log($"ObjectRotator is {_isEnabled}");
        }

        if(_isEnabled)
        {
            _inputHori = Input.GetAxis("Horizontal");
            Rotate();
        }

        //Vector3 currentEuler = RotatableObject.transform.eulerAngles;
        //float currentY = currentEuler.y;
        //if (currentY > 180f)
        //{
        //    currentY -= 360f;
        //}
        //float targetY = currentY + input * rotationSpeed * Time.deltaTime;
        //targetY = Mathf.Clamp(targetY, 0, angle); //êßå¿Ç©ÇØÇÈ
        //if (targetY < 0f)
        //{
        //    targetY += 360f;
        //}
        //RotatableObject.transform.eulerAngles = new Vector3(currentEuler.x, targetY, currentEuler.z);

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
