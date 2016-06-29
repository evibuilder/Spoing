using UnityEngine;
using System.Collections;

public class LifeAndDeath : MonoBehaviour {

    private SpriteRenderer spriteRenderer;
    public int lives = 3;

	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Water")
        {
            Kill();
            lives--;
        }
    }

    void Update()
    {
        if(lives <= 0)
        {
            GameOver();
        }
    }

    void Kill()
    {
        spriteRenderer.enabled = false;
    }

    void GameOver()
    {

    }
}
