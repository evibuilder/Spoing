using UnityEngine;
using System.Collections;
using Prime31;
using System.Diagnostics;

public class BallController : MonoBehaviour
{
    public float gravity = -35f;
    public float walkSpeed = 3;
    public float jumpHeight = 2;
    public float maxSwingLength = 6;
    public float minSwingLength = 1;
    public LineRenderer lineSprite;


    private CharacterController2D _controller;
    private DistanceJoint2D joint;
    private Rigidbody2D springBody;
    private Rigidbody2D ballBody;
    private Transform springTransform;
    private bool isActive;
    private bool falling;
    private bool swinging;
    private float originY;
    private float currentY;
    private bool beingLaunched;
    private Stopwatch timer;
    private float currentSwingLength;



    void Start()
    {
        _controller = gameObject.GetComponent<CharacterController2D>();
        joint = gameObject.GetComponent<DistanceJoint2D>();
        joint.enabled = false;
        springBody = GameObject.Find("spring").GetComponent<Rigidbody2D>();
        ballBody = gameObject.GetComponent<Rigidbody2D>();
        timer = new Stopwatch();
        springTransform = GameObject.Find("spring").GetComponent<Transform>();


        isActive = false;
        falling = false;
        swinging = false;
        beingLaunched = false;

        lineSprite.enabled = false;
    }

    void Update()
    {
        if (timer != null && !timer.IsRunning && GetComponent<Rigidbody2D>().isKinematic == false)
        {
            timer.Reset();
            timer.Start();
        }

        if (timer.IsRunning && timer.ElapsedMilliseconds > 2000)
        {
            timer.Stop();
            GetComponent<Rigidbody2D>().isKinematic = true;
        }

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
            if (CalcDistance() <= maxSwingLength && SpringAbove())
            {
                joint.connectedBody = springBody;

                Vector2 springPos;
                springPos.x = 0;
                springPos.y = 0;

                joint.connectedAnchor = springPos;

                currentSwingLength = CalcDistance();
                joint.distance = currentSwingLength;

                _controller.enabled = false;
                ballBody.isKinematic = false;
                ballBody.gravityScale = 2f;
                ballBody.mass = 2f;

                swinging = true;

                joint.enabled = true;

                LineRendererSetup();
            }
        }

        if (isActive && swinging && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)))
        {
            currentSwingLength -= 0.05f;

            if (currentSwingLength <= minSwingLength)
            {
                currentSwingLength = minSwingLength;
            }
        }
        else if (isActive && swinging && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)))
        {
            currentSwingLength += 0.05f;
            if (currentSwingLength >= maxSwingLength)
            {
                currentSwingLength = maxSwingLength;
            }
        }

        if (isActive && swinging)
        {
            joint.distance = currentSwingLength;
            ballBody.isKinematic = false;

            LineRendererUpdate();
        }

        if (isActive && Input.GetKeyUp(KeyCode.Space) && swinging)
        {
            joint.connectedBody = null;

            _controller.enabled = true;
            ballBody.isKinematic = true;

            joint.enabled = false;
            lineSprite.enabled = false;

            swinging = false;

            Vector3 momentum = _controller.velocity;
            momentum.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
            momentum.y += gravity * Time.deltaTime;
            _controller.move(momentum * Time.deltaTime);
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

        currentY = velocity.y;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name == "spring" && !_controller.isGrounded)
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

    void LineRendererSetup()
    {
        lineSprite.SetColors(Color.black, Color.black);
        lineSprite.SetWidth(0.5f, 1f);
        lineSprite.sortingLayerName = "default";
        lineSprite.enabled = true;
    }

    void LineRendererUpdate()
    {
        Vector3[] points = new Vector3[2];
        points[0].Set(springTransform.position.x, springTransform.position.y, springTransform.position.z);
        points[1].Set(transform.position.x, transform.position.y, transform.position.z);

        lineSprite.SetPositions(points);
    }

    public void Activate()
    {
        isActive = true;
    }

    private bool SpringAbove()
    {
        float y1;
        float y2;

        y1 = GameObject.Find("spring").transform.position.y;
        y2 = gameObject.transform.position.y;

        float topOfBall = y2 + (gameObject.GetComponent<BoxCollider2D>().size.y / 2);
        float botOfSpring = y1 - (GameObject.Find("spring").GetComponent<BoxCollider2D>().size.y / 2);

        if (topOfBall < botOfSpring) return true;
        else return false;
    }

    public bool IsFalling()
    {
        if (_controller.isGrounded)
            return false;
        else
            return true;
    }

    public bool IsLaunched()
    {
        return beingLaunched;
    }

    public void SetLaunched(bool value)
    {
        beingLaunched = value;
    }
}
