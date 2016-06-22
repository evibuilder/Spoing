using UnityEngine;
using System.Collections;
using Prime31;

public class MagnetBehavior : MonoBehaviour {

    public int direction = 1; //1 for horizontal, 0 for vertical
    public int maxDistance = 20;
    public int moveSpeed = 3;

    private int currDistance;
    private bool moveRight;
    private bool moveDown;
    private CharacterController2D _controller;

	void Start () {
        currDistance = 0;
        moveRight = true;
        moveDown = true;

        _controller = gameObject.GetComponent<CharacterController2D>();
    }

    void Update () {

        if(direction == 1)
        {
            MoveHorizontal();
        }
        else if(direction == 0)
        {
            MoveVertical();
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
