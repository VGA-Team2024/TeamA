using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    [SerializeField]
    private float angle = 340;
    public float rotationSpeed = 50f;

    [SerializeField] private GameObject RotatableObject;

    void Update()
    {
        float input = Input.GetAxis("Horizontal");

        Vector3 currentEuler = RotatableObject.transform.eulerAngles;
        float currentY = currentEuler.y;
        if (currentY > 180f)
        {
            currentY -= 360f;
        }
        float targetY = currentY + input * rotationSpeed * Time.deltaTime;
        targetY = Mathf.Clamp(targetY, 0, angle); //êßå¿Ç©ÇØÇÈ
        if (targetY < 0f)
        {
            targetY += 360f;
        }
        RotatableObject.transform.eulerAngles = new Vector3(currentEuler.x, targetY, currentEuler.z);
    }
}
