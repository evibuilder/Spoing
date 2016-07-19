using UnityEngine;
using System.Collections;
using Prime31;


public class BugController : MonoBehaviour {

    public int maxDistance = 20;
    public int moveSpeed = 3;
    public float gravity = -35f;

    private int currDistance;
    private bool moveRight;
    private CharacterController2D _controller;

    // Use this for initialization
    void Start () {
        _controller = gameObject.GetComponent<CharacterController2D>();
        moveRight = true;
        currDistance = 0;
    }

    // Update is called once per frame
    void Update () {

        if (_controller.isGrounded == false)
        {
            Vector3 velocity = _controller.velocity;
            velocity.x = 0;
            velocity.y += gravity * Time.deltaTime;
            _controller.move(velocity * Time.deltaTime);
        }
        else
        {
            MoveHorizontal();
        }
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
        else if (moveRight == true && currDistance == maxDistance)
        {
            moveRight = false;
        }
        else if (moveRight == false && currDistance > 0)
        {
            velocity.x = moveSpeed * (-1);
            _controller.move(velocity * Time.deltaTime);
            currDistance -= moveSpeed;
        }
        else if (moveRight == false && currDistance == 0)
        {
            moveRight = true;
        }
    }
}
