using UnityEngine;
using System.Collections;
using Prime31;
using System.Diagnostics;

public class MagnetBehavior : MonoBehaviour {

    public int direction = 1; //1 for horizontal, 0 for vertical
    public int maxDistance = 20;
    public int moveSpeed = 3;
    public float magnetStrength = 50f;
    public LevelManager levelManager;


    private int currDistance;
    private bool moveRight;
    private bool moveDown;
    private float magnetWidth;
    private CharacterController2D _controller;
    private DistanceJoint2D joint;
    private Rigidbody2D springBody;
    private CharacterController2D springController;
    private float prevGrav;
    private float prevAngDrag;
    private float prevLinDrag;
    private float prevMass;
    private Quaternion rotation;
    private Stopwatch respawnTimer;

    void Start () {
        currDistance = 0;
        moveRight = true;
        moveDown = true;

        _controller = gameObject.GetComponent<CharacterController2D>();
        joint = GameObject.Find("spring").GetComponent<DistanceJoint2D>();
        joint.enabled = false;
        springBody = GameObject.Find("spring").GetComponent<Rigidbody2D>();
        springController = GameObject.Find("spring").GetComponent<CharacterController2D>();
    
        magnetWidth = gameObject.GetComponent<BoxCollider2D>().size.x;

        prevGrav = springBody.gravityScale;
        prevAngDrag = springBody.angularDrag;
        prevLinDrag = springBody.drag;
        prevMass = springBody.mass;
        rotation = GameObject.Find("spring").GetComponent<Transform>().rotation;
        respawnTimer = new Stopwatch();
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetValues();
        }

        if (respawnTimer != null &&respawnTimer.IsRunning && respawnTimer.ElapsedMilliseconds > 2000)
        {
            if (levelManager != null)
            {
                respawnTimer.Stop();
                levelManager.Kill();
                ResetValues();
            }
        }
        

        if (CalcDistance() <= magnetStrength && Aligned() && joint.enabled == false)
        {
            Magnetize();
        }

        if (joint.enabled)
        {
            Pull();
        }


        if (direction == 1)
        {
            MoveHorizontal();
        }
        else if(direction == 0)
        {
            MoveVertical();
        }


    }

    public bool Aligned()
    {
        if (direction == 1)
        {
            float springX = GameObject.Find("spring").transform.position.x;
            float magnetX = gameObject.transform.position.x;

            float magnetWidth = gameObject.GetComponent<BoxCollider2D>().size.x;

            if (springX > magnetX && springX < magnetX + magnetWidth)
                return true;
            else
                return false;
        }
        else if (direction == 0)
        {
            float springY = GameObject.Find("spring").transform.position.y;
            float magnetY = gameObject.transform.position.y;

            float magnetWidth = gameObject.GetComponent<BoxCollider2D>().size.y;

            if (springY > magnetY && springY < magnetY + magnetWidth)
                return true;
            else
                return false;
        }
        else
            return false;
        
    }
     
    private void Magnetize()
    {
        joint.connectedBody = gameObject.GetComponent<Rigidbody2D>();

        Vector2 magnetPos;
        magnetPos.x = 0;
        magnetPos.y = 0 - magnetWidth;

        joint.connectedAnchor = magnetPos;
        joint.distance = CalcDistance();

        springController.enabled = false;
        springBody.isKinematic = false;
        springBody.gravityScale = 0f;
        springBody.drag = 0f;
        springBody.angularDrag = 0f;
        springBody.mass = 0f;
        GameObject.Find("spring").GetComponent<SpringController>().SendMessage("ChangeGravity", 0f);

        joint.enabled = true;
    }

    private void ResetValues()
    {
        joint.connectedBody = null;
        joint.enabled = false;
        springBody.gravityScale = prevGrav;
        springBody.mass = prevMass;
        springBody.drag = prevLinDrag;
        springBody.angularDrag = prevAngDrag;
        springBody.isKinematic = true;
        GameObject.Find("spring").GetComponent<SpringController>().SendMessage("ChangeGravity", -35f);
        springController.enabled = true;
        Quaternion newRotation = GameObject.Find("spring").GetComponent<Transform>().rotation;
        GameObject.Find("spring").GetComponent<Transform>().rotation = Quaternion.Slerp(newRotation, rotation, Time.time * 1f);
    }

    private void Pull()
    {
        if (joint.distance > 0.005f)
        {
            joint.distance -= 0.005f;
        }
        else if (joint.distance == 0.005f && !respawnTimer.IsRunning)
        {
            respawnTimer.Reset();
            respawnTimer.Start();
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


    void MoveHorizontal()
    {
        Vector3 velocity = _controller.velocity;
        velocity.x = 0;

        if (moveRight == true && currDistance < maxDistance)
        {
            velocity.x = moveSpeed;
            _controller.move(velocity * Time.deltaTime);
            currDistance += moveSpeed;
        }
        else if(moveRight == true && currDistance == maxDistance)
        {
            moveRight = false;
        }
        else if(moveRight == false && currDistance > 0)
        {
            velocity.x = moveSpeed * (-1);
            _controller.move(velocity * Time.deltaTime);
            currDistance -= moveSpeed;
        }
        else if(moveRight == false && currDistance == 0)
        {
            moveRight = true;
        }
    }

    void MoveVertical()
    {
        Vector3 velocity = _controller.velocity;
        velocity.y = 0;

        if(moveDown == true && currDistance < maxDistance)
        {
            velocity.y = moveSpeed;
            _controller.move(velocity * Time.deltaTime);
            currDistance += moveSpeed;
        }
        else if(moveDown == true && currDistance == maxDistance)
        {
            moveDown = false;
        }
        else if(moveDown == false && currDistance > 0)
        {
            velocity.y = moveSpeed * (-1);
            _controller.move(velocity * Time.deltaTime);
            currDistance -= moveSpeed;
        }
        else if(moveDown == false && currDistance == 0)
        {
            moveDown = true;
        }

    }
}
