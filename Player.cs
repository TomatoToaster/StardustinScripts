using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Sprite[] keyframes;
    private SpriteRenderer spriteRenderer;
    private float remainingMovement = 0f;
    public float moveSpeed = 0f;

    // Whether the player is going to move on next click or not
    private bool moveState;

    // The projectile we shoot
    public GameObject bulletPrefab;

    // The distance from the ship to the bullet spawning
    public float bulletSpawnOffset;

    // The amount of charge (out of 1) that the player has
    public readonly float charge;


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        moveState = true;
    }

    // Update is called once per frame
    void Update()
    {

        // When clicking somewhere...
        if (Input.GetMouseButtonDown(0)) {
            // Point the player towards the mouse
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 vectorToTarget = worldPosition - (Vector2) transform.position;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, vectorToTarget);

            // If moveState is active, then add remaining movement so the player goes in that direction
            if (moveState) {
                remainingMovement = vectorToTarget.magnitude;

            // If not, shoot a bullet, and also stop moving
            } else {
                remainingMovement = 0;
                Instantiate(bulletPrefab, transform.position + transform.up * bulletSpawnOffset, transform.rotation);
            }

            // Then swap the moveState so we switch between shooting and moving
            moveState = !moveState;
        }

        // While there is remainingMovement left, keep move the ship in the direction it's facing (assuming ship sprite is facing upwards)
        if (remainingMovement > 0) {
            float movement = moveSpeed * Time.deltaTime;

            // Don't move more than there is remaining movement
            if (movement > remainingMovement) {
                movement = remainingMovement;
            }
            remainingMovement -= movement;
            transform.position += transform.up * movement;

            spriteRenderer.sprite = keyframes[1];
        } else {
            spriteRenderer.sprite = keyframes[0];
        }

        // If we're ready to move, show the thrusters active
        if (moveState) {

        }
    }
}
