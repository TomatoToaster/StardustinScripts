using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float moveSpeed = 0f;
    private Rigidbody2D myRigidbody;
    private GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        myRigidbody = gameObject.GetComponent<Rigidbody2D>();
        myRigidbody.velocity = transform.up * moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // Kill the enemy and spawn dust off their body
        if (col.tag == "Enemy") {
            gameController.AddScore(100);
            gameController.SpawnDust(col.gameObject.transform.position);
            Destroy(col.gameObject);
            Destroy(gameObject);
            gameController.SpawnEnemy();
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
