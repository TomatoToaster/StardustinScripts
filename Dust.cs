using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dust : MonoBehaviour
{
    private GameController gameController;
    private Rigidbody2D myRigidbody;
    public float moveSpeed;
    public float secondsOfLife;
    public float speedReward = 0.25f;
    private Vector2 lastVelocity;
    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        myRigidbody = gameObject.GetComponent<Rigidbody2D>();
        myRigidbody.velocity = transform.up * moveSpeed;
        SelfDesctruct(secondsOfLife);
    }

    // Update is called once per frame
    void Update()
    {
        lastVelocity = myRigidbody.velocity;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // If we hit a player, add score, increase player speed, and destroy this item
        if (collision.gameObject.tag == "Player") {
            if (gameController) {
                gameController.AddScore(500);
            }
            collision.gameObject.GetComponent<Player>().incrementSpeed(speedReward);
            Destroy(gameObject);
            return;
        }

        // Otherwise reflect off of the other object by reflecting the
        // lastVelocity off of the normal vector in the collision point
        float speed = lastVelocity.magnitude;
        Vector2 direction = Vector2.Reflect(lastVelocity.normalized, collision.GetContact(0).normal);
        myRigidbody.velocity = direction * speed;
    }
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    // Destroys this Dust after given time
    public void SelfDesctruct(float time)
    {
        StartCoroutine(WaitToSelfDestruct(time));
    }

    private IEnumerator WaitToSelfDestruct(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

}
