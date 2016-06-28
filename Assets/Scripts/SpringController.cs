using UnityEngine;
using System.Collections;
using Prime31;
using System.Diagnostics;

public class SpringController : MonoBehaviour
{
    public float gravity = -35f;
    public float walkSpeed = 3;
    public float jumpHeight = 2;
    public float launchDistance = 1;

    private CharacterController2D _controller;
    private SpringJoint2D joint;
    private Rigidbody2D ballBody;
    private Stopwatch timer;
    private bool chargingUp;
    private bool isActive;
    private bool launching;

    void Start()
    {
        _controller = gameObject.GetComponent<CharacterController2D>();
        ballBody = GameObject.Find("ball").GetComponent<Rigidbody2D>();
        timer = new Stopwatch();
        chargingUp = false;
        isActive = true;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            switch (isActive)
            {
                case true: isActive = false;
                    break;
                case false: isActive = true;
                    break;
                default:
                    break;
            }
        }

        if(isActive && Input.GetKeyDown(KeyCode.E) && !launching)
        {
            if (CalcDistance() <= launchDistance)
            {
                launching = true;
                transform.Rotate(0,0, -90);
            }
        }
        else if(isActive && launching)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                timer.Reset();
                timer.Start();
            }
            else if (Input.GetKeyUp(KeyCode.Space))
            {
                timer.Stop();
                launching = false;

                Vector3 power;
                power.x = 50;
                power.y = 0;
                power.z = 0;

                if(transform.position.x > ballBody.transform.position.x)
                {
                    power *= -1;
                }

                ballBody.gravityScale = 0;
                ballBody.isKinematic = false;
                ballBody.AddForce(power * 25f * (timer.ElapsedMilliseconds / 1000f));
                transform.Rotate(0, 0, 90);
            }
        }

            Vector3 velocity = _controller.velocity;
            velocity.x = 0;

            if (Input.GetAxis("Horizontal") < 0 && !chargingUp && !launching && isActive)
            {
                velocity.x = walkSpeed * -1;
            }
            else if (Input.GetAxis("Horizontal") > 0 && !chargingUp && !launching && isActive)
            {
                velocity.x = walkSpeed;
            }

            if (Input.GetKeyDown(KeyCode.Space) && isActive)
            {
                timer.Reset();
                timer.Start();
                chargingUp = true;
            }
            if (Input.GetKeyUp(KeyCode.Space) && isActive)
            {
                timer.Stop();
                chargingUp = false;
                if (_controller.isGrounded)
                {
                    velocity.y = Mathf.Sqrt((timer.ElapsedMilliseconds / 1000f) * 4f * jumpHeight * -gravity);
                }
            }
            else if (_controller.isGrounded && !chargingUp && !launching && isActive)
            {
                velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
            }

            velocity.y += gravity * Time.deltaTime;

            _controller.move(velocity * Time.deltaTime);
        
    }

    public float CalcDistance()
    {
        float x1;
        float x2;
        float y1;
        float y2;

        x1 = GameObject.Find("ball").transform.position.x;
        y1 = GameObject.Find("ball").transform.position.y;

        x2 = gameObject.transform.position.x;
        y2 = gameObject.transform.position.y;

        return Mathf.Sqrt(Mathf.Pow(x1 - x2, 2) + Mathf.Pow(y1 - y2, 2));
    }

}
