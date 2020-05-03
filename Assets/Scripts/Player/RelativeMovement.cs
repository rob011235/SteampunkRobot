using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class RelativeMovement : MonoBehaviour
{    
    [SerializeField]
    private Transform _cameraTransform;
    private float _vertSpeed;
    private CharacterController _charController;
    private ControllerColliderHit _contact;
    private Animator _animator;

    public float rotSpeed = 12.0f;
    public float moveSpeed = 5.0f;
    public float jumpSpeed = 15.0f;
    public float gravity = -9.8f;
    public float terminalVelocity = -10.0f;
    public float minFall = -1.5f;
    public float heightCheck = 2;
    void Start()
    {
        _charController = GetComponent<CharacterController>();
        _vertSpeed = minFall;
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = Vector3.zero;
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");
        if (hInput != 0 || vInput != 0)
        {
            //Set inputs into player movement vector. Note vector is in local space.
            movement.x = hInput * moveSpeed;
            movement.z = vInput * moveSpeed;
            movement = Vector3.ClampMagnitude(movement, moveSpeed);

            //Preserve player rotation for later
            Quaternion temp = _cameraTransform.rotation;

            //Make the player vertical before changing movement vector to world space.
            _cameraTransform.eulerAngles = new Vector3(0, _cameraTransform.eulerAngles.y, 0);

            //Change the movement vector to World Space
            movement = _cameraTransform.TransformDirection(movement);

            //Make player face original direction
            _cameraTransform.rotation = temp;

            //Figure out which direction the camera will be looking at
            Quaternion direction = Quaternion.LookRotation(movement);

            //Apply the direction to the camera using the Slerp method so that it moves smoothly
            transform.rotation = Quaternion.Slerp(transform.rotation, direction, rotSpeed * Time.deltaTime);

            
        }

        
        bool hitGround = false;
        RaycastHit hit;
        if(_vertSpeed < 0 && 
            Physics.Raycast(transform.position,Vector3.down,out hit))
        {
            float check = heightCheck;
            hitGround = hit.distance <= check;
        }

        _animator.SetFloat("Speed", movement.sqrMagnitude);

        if (hitGround)
        {
            if (Input.GetButtonDown("Jump"))
            {
                _vertSpeed = jumpSpeed;
            }
            else
            {
                _vertSpeed = -0.1f;
                _animator.SetBool("Jumping", false);
            }
        }
        else
        {
            _vertSpeed += gravity * 5 * Time.deltaTime;
            if (_vertSpeed < terminalVelocity)
            {
                _vertSpeed = terminalVelocity;
            }

            if(_contact != null)
            {
                _animator.SetBool("Jumping", true);
            }


            if(_charController.isGrounded)
            {
                if(Vector3.Dot(movement,_contact.normal)<0)
                {
                    movement = _contact.normal * moveSpeed;
                }
                else
                {
                    movement += _contact.normal * moveSpeed;
                }
            }
        }
        movement.y = _vertSpeed;
        //Adjust for frame rate.
        movement *= Time.deltaTime;
        //Position the camera where it needs to be
        _charController.Move(movement);
        
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        _contact = hit;
    }
}
