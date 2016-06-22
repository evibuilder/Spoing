using UnityEngine;
using System.Collections;
using Prime31;
using camera;

public class BallController : MonoBehaviour
{
    public float gravity = -35f;
    public float walkSpeed = 3;

    private CharacterController2D _controller;
    private Transform target;
    private bool enabled;

    void Start()
    {
        _controller = gameObject.GetComponent<CharacterController2D>();
        enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            switch (enabled)
            {
                case true:
                    enabled = false;
                    break;
                case false:
                    enabled = true;
                    break;
                default:
                    break;
            }
        }

        Vector3 velocity = _controller.velocity;
        velocity.x = 0;

        if (Input.GetAxis("Horizontal") < 0 && enabled)
        {
            velocity.x = walkSpeed * -1;
        }
        else if (Input.GetAxis("Horizontal") > 0 && enabled)
        {
            velocity.x = walkSpeed;
        }

        velocity.y += gravity * Time.deltaTime;

        _controller.move(velocity * Time.deltaTime);
    }
}
