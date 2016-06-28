using UnityEngine;
using System.Collections;

public class BatteryScript : MonoBehaviour {

    public float gravity = -35f;

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
        }
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name == "ball")
        {
            _controller.isKinematic = false;
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
