using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour
{
    public Transform player;
    public float triggerDistance = 2;
    private Animator _animator;
    private bool isOn = false;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //if e key is pressed and player is close enough
        if(Input.GetKeyDown(KeyCode.E) && Vector3.Distance( player.position,this.transform.position)<triggerDistance)
        {
            Debug.Log("Clicked!");
            isOn=!isOn;
            _animator.SetBool("IsOn",isOn);
        }
        
    }
}
