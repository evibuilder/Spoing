﻿using UnityEngine;
using System.Collections;
using Prime31;

public class BallController : MonoBehaviour
{
    public float gravity = -35f;
    public float walkSpeed = 3;
    public float jumpHeight = 2;
    public float swingLength = 3;

    private CharacterController2D _controller;
    private DistanceJoint2D joint;
    private Rigidbody2D springBody;
    private Rigidbody2D ballBody;
    private bool isActive;
    private bool falling;
    private bool swinging;
    private float originY;
    private float currentY;

    void Start()
    {
        _controller = gameObject.GetComponent<CharacterController2D>();
        joint = gameObject.GetComponent<DistanceJoint2D>();
        joint.enabled = false;
        springBody = GameObject.Find("spring").GetComponent<Rigidbody2D>();
        ballBody = gameObject.GetComponent<Rigidbody2D>();

        isActive = false;
        falling = false;
        swinging = false;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            switch (isActive)
            {
                case true:
                    isActive = false;
                    break;
                case false:
                    isActive = true;
                    break;
                default:
                    break;
            }
        }

        if (isActive && Input.GetKeyDown(KeyCode.E))
        {
            if(CalcDistance() <= swingLength)
            {
                joint.connectedBody = springBody;

                Vector2 springPos;
                springPos.x = 0;
                springPos.y = 0;

                joint.connectedAnchor = springPos;

                joint.distance = swingLength;

                _controller.enabled = false;
                ballBody.isKinematic = false;
                ballBody.gravityScale = 1f;

                swinging = true;

                joint.enabled = true;
            }
        }

        if(isActive && Input.GetKeyUp(KeyCode.Space) && swinging)
        {
            joint.connectedBody = null;

            _controller.enabled = true;
            ballBody.isKinematic = true;

            joint.enabled = false;

            swinging = false;

            Vector3 momentum = _controller.velocity;
            momentum.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
            momentum.y += gravity * Time.deltaTime;
            _controller.move(momentum * Time.deltaTime);
        }

        Vector3 velocity = _controller.velocity;
        velocity.x = 0;

        if (Input.GetAxis("Horizontal") < 0 && isActive)
        {
            velocity.x = walkSpeed * -1;
        }
        else if (Input.GetAxis("Horizontal") > 0 && isActive)
        {
            velocity.x = walkSpeed;
        }

        if (!swinging && !_controller.isGrounded)
        velocity.y += gravity * Time.deltaTime;
        
        _controller.move(velocity * Time.deltaTime);

        if (_controller.velocity.y < currentY)
        {
            falling = true;
            originY = currentY;
        }
        else
        {
            falling = false;
        }

        currentY = velocity.y;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name == "spring" && falling && isActive)
        {
            Vector3 velocity = _controller.velocity;

            float distanceFallen = originY - _controller.velocity.y;

            velocity.y = Mathf.Sqrt(4f * jumpHeight * -gravity + distanceFallen);

            velocity.y += gravity * Time.deltaTime;

            _controller.move(velocity * Time.deltaTime);
        }
    }

    public float CalcDistance()
    {
        float x1;
        float x2;
        float y1;
        float y2;

        x1 = GameObject.Find("spring").transform.position.x;
        y1 = GameObject.Find("spring").transform.position.y;

        x2 = gameObject.transform.position.x;
        y2 = gameObject.transform.position.y;

        return Mathf.Sqrt(Mathf.Pow(x1 - x2, 2) + Mathf.Pow(y1 - y2, 2));
    }

}
