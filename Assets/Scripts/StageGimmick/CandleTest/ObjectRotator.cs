using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    [SerializeField]
    private float angle = 340;
    [SerializeField]
    private float rotationSpeed = 50f;

    [SerializeField] private GameObject RotatableObject;
    private float _currentAngle = 0;

    void Update()
    {
        float input = Input.GetAxis("Horizontal");

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

        _currentAngle += input * rotationSpeed * Time.deltaTime;
        _currentAngle = Mathf.Clamp(_currentAngle, 0, angle);
        RotatableObject.transform.eulerAngles = new Vector3(
            RotatableObject.transform.eulerAngles.x,
            _currentAngle,
            RotatableObject.transform.eulerAngles.z);
    }
}
