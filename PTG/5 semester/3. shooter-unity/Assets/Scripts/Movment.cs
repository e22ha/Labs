using UnityEngine;

public class Movment : MonoBehaviour
{
    public Camera cam;
    private float _baseFOV = 60f;
    public float sprintFOV = 1.25f;

    [Range(1000f, 20000f)] public float jumpForce = 5000f;

    public LayerMask ground;
    public Transform groundDetector;

    [Range(50f, 200f)] public float walkSpeed = 100f;
    [Range(50f, 200f)] public float runSpeed = 200f;

    private float _mSpeed;

    private Rigidbody _rb;

    // Start is called before the first frame update
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _mSpeed = walkSpeed;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        var groundCheck = Physics.Raycast(groundDetector.position, Vector3.down, 0.1f, ground);
        var jump = Input.GetKey(KeyCode.Space) && groundCheck;

        if (jump) _rb.AddForce(Vector3.up * jumpForce);

        var sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        var xMove = Input.GetAxisRaw("Horizontal");
        var zMove = Input.GetAxisRaw("Vertical");

        if (sprint && zMove > 0)
        {
            _mSpeed = runSpeed;
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, _baseFOV * sprintFOV, Time.fixedDeltaTime * 8f);
        }
        else
        {
            _mSpeed = walkSpeed;
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, _baseFOV, Time.fixedDeltaTime * 8f);
        }

        var dir = new Vector3(xMove, 0, zMove);

        dir.Normalize();

        var v = transform.TransformDirection(dir) * _mSpeed * Time.fixedDeltaTime;

        v.y = _rb.velocity.y;

        _rb.velocity = v;
    }
}