using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float accel;
    [SerializeField] private float deccel;
    [SerializeField] private float rotationSpeed;

    [SerializeField] private Vector3VariableSO playerPos;

    private Vector2 moveInput;
    private float speed;
    private float toRotate;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        Vector3 forward = transform.forward;
        forward.y = 0f;
        transform.position = transform.position + forward * speed * Time.fixedDeltaTime;
        if (toRotate != 0)
        {
            transform.Rotate(0f, toRotate, 0f);
            toRotate = 0f;
        }

        playerPos.SetValue(transform.position);
    }

    private void HandleInput()
    {
        // Calculate move speed

        float targetSpeed = moveInput.y != 0 ? Mathf.Sign(moveInput.y) * maxSpeed : 0f;

        float accelRate = Mathf.Abs(targetSpeed) > 0.01f ? accel : deccel;

        float speedDif = targetSpeed - speed;

        speed += Mathf.Lerp(0f, speedDif, accelRate * Time.deltaTime);

        // Calculate rotation speed

        if (moveInput.x != 0)
        {
            float rotationSign = Mathf.Sign(moveInput.x) * ( moveInput.y != 0 ? Mathf.Sign(moveInput.y) : Mathf.Sign(speed) );

            float rotation = rotationSign * rotationSpeed * (Mathf.Abs(speed) / maxSpeed) * Time.deltaTime; // rotation speed is based on current move speed 
             
            toRotate += rotation;
        }
    }

    public void SetMoveInput(Vector2 moveInput) => this.moveInput = moveInput;
}
