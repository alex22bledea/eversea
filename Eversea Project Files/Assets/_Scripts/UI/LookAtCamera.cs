using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private bool isInverted = false;

    public void LateUpdate()
    {
        if (isInverted)
            transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        else
            transform.LookAt(Camera.main.transform);

    }
}
