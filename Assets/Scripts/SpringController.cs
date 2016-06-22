using UnityEngine;
using System.Collections;
using Prime31;
using System.Diagnostics;

public class SpringController : MonoBehaviour
{
    public float gravity = -35f;
    public float walkSpeed = 3;
    public float jumpHeight = 2;

    private CharacterController2D _controller;
    private Stopwatch timer;
    private bool chargingUp;
    private bool enabled;

    void Start()
    {
        _controller = gameObject.GetComponent<CharacterController2D>();
        timer = new Stopwatch();
        chargingUp = false;
        enabled = true;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            switch (enabled)
            {
                case true: enabled = false;
                    break;
                case false: enabled = true;
                    break;
                default:
                    break;
            }
        }

        Vector3 velocity = _controller.velocity;
        velocity.x = 0;

        if (Input.GetAxis("Horizontal") < 0 && !chargingUp && enabled){
            velocity.x = walkSpeed * -1;
        }
        else if(Input.GetAxis("Horizontal") > 0 && !chargingUp && enabled){
            velocity.x = walkSpeed;
        }

        if(Input.GetKeyDown(KeyCode.Space) && enabled){
            timer.Reset();
            timer.Start();
            chargingUp = true;
        }
        if (Input.GetKeyUp(KeyCode.Space) && enabled){
            timer.Stop();
            chargingUp = false;
            if (_controller.isGrounded)
            {
                velocity.y = Mathf.Sqrt((timer.ElapsedMilliseconds / 1000f) * 4f * jumpHeight * -gravity);
            }
        }
        else if (_controller.isGrounded && !chargingUp && enabled){
            velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        _controller.move(velocity * Time.deltaTime);
    }
}
