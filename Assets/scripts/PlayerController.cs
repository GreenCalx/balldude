using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public CharacterController controller;
    public float runSpeed = 40;
    private float __x_move = 0f;
    private bool __doJump = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        __x_move = Input.GetAxisRaw("Horizontal") * runSpeed ;
        __doJump = Input.GetAxisRaw("Vertical") > 0 ;
    }

    void FixedUpdate()
    {
        controller.move( __x_move * Time.fixedDeltaTime, __doJump );
    }


}
