using UnityEngine;

public class ObjectSpinner : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float rotationAmount;

    private bool spinLeft = true;

    void Update()
    {
        if (spinLeft)
        {
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
            if (transform.rotation.eulerAngles.y <= 360-rotationAmount)
                spinLeft = false;
        }
        else
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            if (transform.rotation.eulerAngles.y >= rotationAmount)
                spinLeft = true;
        }
    }
}
