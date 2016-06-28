using UnityEngine;
using System.Collections;

public class BatteryScript : MonoBehaviour {

    public float moveSpeed = 4.5f;

    private Vector3 velocity;
    private Rigidbody2D _controller;

    void Start()
    {
        _controller = gameObject.GetComponent<Rigidbody2D>();
    }

    public void OnTriggerStay2D(Collider2D col)
    {
        if(col.name == "ball")
        {
            _controller.isKinematic = false;

            velocity.x += moveSpeed;
            velocity.y = 0;
            velocity.z = 0;

            if (transform.position.x < col.transform.position.x)
            {
                velocity *= -1;
            }

            _controller.AddForce(velocity);
        }
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name == "ball")
        {
            _controller.isKinematic = false;

            velocity.x += moveSpeed;
            velocity.y = 0;
            velocity.z = 0;

            if(transform.position.x < col.transform.position.x)
            {
                velocity *= -1;
            }

            _controller.AddForce(velocity);
        }
    }

    public void OnTriggerExit2D(Collider2D col)
    {
        if (col.name == "ball")
        {
            _controller.isKinematic = true;
        }
    }
}
