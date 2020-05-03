using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    [SerializeField] private Transform playerTransform;
    public float rotSpeed = 1.5f;

    public Vector3 lookOffset = new Vector3(0,0,0);
    private float _rotY;
    private Vector3 _offset;
    
	void Start ()
    {
        _rotY = transform.rotation.y;
        _offset = playerTransform.position - transform.position;
	}
	
	void LateUpdate ()
    {
        float hInput = Input.GetAxis("Horizontal");
        //If we have keyboard input base rotation on rotSpeed
        if(hInput != 0)
        {
            _rotY += hInput * rotSpeed;
        }
        else //Use mouse and move faster
        {
            _rotY += Input.GetAxis("Mouse X") * 3.0f;
        }
        //Figure out direction to point camera
        Quaternion rotation = Quaternion.Euler(0, _rotY, 0);
        //Change camera's position by rotating offset 
        transform.position = playerTransform.position - (rotation * _offset);
        //Make camera look at player
        transform.LookAt(playerTransform.position+lookOffset);
	}
}
