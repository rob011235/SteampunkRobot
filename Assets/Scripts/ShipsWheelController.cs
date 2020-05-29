using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Animator))]
public class ShipsWheelController : MonoBehaviour
{
    
    private GameObject _player;
    public float triggerDistance = 2;
    private Animator _animator;
    const int STEADYAHEAD = 0;
    const int SPINLEFT = 1;
    const int SPINRIGHT = 2;
    private int wheelState = STEADYAHEAD;
    private bool _lastDirectionRight;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _lastDirectionRight=true;
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && Vector3.Distance( _player.transform.position,this.transform.position)<triggerDistance)
        {
            ToggleWheelState();
            _animator.SetInteger("WheelState",wheelState);
        }
    }

    private void ToggleWheelState()
    {
        if(wheelState==STEADYAHEAD)
        {
            if(_lastDirectionRight)
            {
                wheelState = SPINLEFT;
            }
            else
            {
                wheelState = SPINRIGHT;
            }
            _lastDirectionRight=!_lastDirectionRight;
        }
        else 
        {
            wheelState = STEADYAHEAD;
        }
    }
}
