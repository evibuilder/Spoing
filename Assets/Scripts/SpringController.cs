using UnityEngine;
using System.Collections;
using Prime31;
using System.Diagnostics;
using camera;

public class SpringController : MonoBehaviour
{
    public float gravity = -35f;
    public float walkSpeed = 3;
    public float jumpHeight = 2;
    public float baseLaunchSpeed = 10f;

    private float BaseCameraTimer = 460;
    private CharacterController2D _controller;
    private CharacterController2D _ballController;
    private SpringJoint2D joint;
    private Stopwatch timer;
    private Stopwatch jumpTimer;
    private bool chargingUp;
    private bool isActive;
    private bool launching;
    private float launchDistance;
    private Quaternion originalRotation;
    private Quaternion newRotation;
    private Rigidbody2D ballBody;
    private CameraScript _camera;

    void Start()
    {
        _controller = gameObject.GetComponent<CharacterController2D>();
        _ballController = GameObject.Find("ball").GetComponent<CharacterController2D>();
        timer = new Stopwatch();
        jumpTimer = new Stopwatch();
        ballBody = GameObject.Find("ball").GetComponent<Rigidbody2D>();
        _camera = GameObject.Find("Main Camera").GetComponent<CameraScript>();

        chargingUp = false;
        isActive = true;
        launchDistance = GetComponent<BoxCollider2D>().size.y;
    }

    void Update()
    {
        if (isActive && Input.GetKeyDown(KeyCode.E) && !launching)
        {
            if (CalcDistance() <= launchDistance)
            {
                launching = true;
                originalRotation = transform.rotation;

                float ballX = GameObject.Find("ball").GetComponent<Transform>().position.x;
                float springX = gameObject.GetComponent<Transform>().position.x;

                if(springX > ballX) newRotation = Quaternion.Euler(0, 0, 90);
                else newRotation = Quaternion.Euler(0, 0, 270);
                
                transform.rotation = Quaternion.Slerp(originalRotation, newRotation, Time.time * 1f);

            }
        }
        else if (isActive && Input.GetKeyDown(KeyCode.E) && launching)
        {
            launching = false;
            transform.rotation = Quaternion.Slerp(newRotation, originalRotation, Time.time * 1f);
        }
        else if (isActive && launching)
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

                Vector3 force = _ballController.velocity;

                force.x = Mathf.Sqrt(timer.ElapsedMilliseconds / 1000f) * baseLaunchSpeed;
                force.y = 5f;

                Transform spring = GetComponent<Transform>();
                Transform ball = GameObject.Find("ball").GetComponent<Transform>();

                if (ball.position.x < spring.position.x) {
                    force.x *= -1;
                }

                UnityEngine.Debug.Log("initial velocity is " + force.x);

                _ballController.move(force * Time.deltaTime);

                transform.rotation = Quaternion.Slerp(newRotation, originalRotation, Time.time * 1f);

                isActive = false;
                GameObject.Find("ball").GetComponent<BallController>().SendMessage("SetActive", true);
                GameObject.Find("ball").GetComponent<BallController>().SendMessage("SetLaunched", true);

                GameObject.Find("Main Camera").SendMessage("SwitchCamera");
            }
        }

        if (_controller.isGrounded && _controller.ground != null && _controller.ground.tag == "MovingPlatform")
        {
            this.transform.parent = _controller.ground.transform;
        }
        else
        {
            if (this.transform.parent != null)
            {
                transform.parent = null;
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
            GameObject.Find("Main Camera").SendMessage("SetFollow", true);
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
            GameObject.Find("Main Camera").SendMessage("SetFollow", false);

            if (jumpTimer.IsRunning)
            {
                jumpTimer.Reset();
                jumpTimer.Start();
            }
            else
            {
                jumpTimer.Start();
            }
        }

        velocity.y += gravity * Time.deltaTime;

        float delta = 110f * jumpHeight;
        float cameraTimer = BaseCameraTimer + delta;

        if (jumpTimer.ElapsedMilliseconds > cameraTimer)
        {
            GameObject.Find("Main Camera").SendMessage("SetFollow", true);
        }

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

    public void ChangeGravity(float input)
    {
        gravity = input;
    }

    public bool IsActive()
    {
        return isActive;
    }

    public void SetActive(bool state)
    {
        isActive = state;
    }

    public bool IsCharging()
    {
        return chargingUp;
    }

    public bool IsLaunching()
    {
        return launching;
    }

}
