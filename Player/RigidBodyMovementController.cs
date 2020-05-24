using UnityEngine;

public class RigidBodyMovementController : MonoBehaviour
{
    public Rigidbody rb;

    public float moveSpeed = 35f;

    public float sprintSpeedMultiplier = 1.6f;

    public float jumpForce = 35f;

    public Transform groundCheckTransform;

    public LayerMask groundCheckLayerMask;
    
    private Vector3 _inputVector;

    private bool _isGrounded = true;

    private void Update()
    {
        _inputVector = new Vector3(Input.GetAxis("Horizontal") , 0f , Input.GetAxis("Vertical"));

        if (Input.GetKey(KeyCode.LeftShift))
            _inputVector.z *= sprintSpeedMultiplier;
        
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            rb.AddForce(jumpForce * 10 * transform.up, ForceMode.Acceleration);
            _isGrounded = false;
        }
    }

    private void FixedUpdate()
    {
        _isGrounded = Physics.CheckSphere(groundCheckTransform.position, 0.3f, groundCheckLayerMask);
        
        Vector3 movement = moveSpeed * 10f * _inputVector.z * Time.fixedDeltaTime * transform.forward +
                        moveSpeed * 10f * _inputVector.x * Time.fixedDeltaTime * transform.right;
        rb.MovePosition(rb.position + movement);
    }
}