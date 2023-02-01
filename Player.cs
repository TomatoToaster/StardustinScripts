using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Sprite[] keyframes;
    private SpriteRenderer spriteRenderer;
    private float remainingMovement = 0f;
    public float moveSpeed = 0f;

    // The projectile we shoot
    public GameObject bulletPrefab;

    // The distance from the ship to the bullet spawning
    public float bulletSpawnOffsetUp;

    public int bulletMax;
    public float chargeGrowth;
    public float chargeDecay;
    public float bulletChargeCost = 1;

    // The amount of charge (each bullet costs 1) that the player has
    private float charge;

    // The number of bullets the player has
    private int bullets;

    // Whether the player should just keep moving towards the cursor without checking for click
    public bool autoAccelerate = false;

    // Destination object's script that we use to show where the ship is currently traveling
    public Destination destination;

    // Orbit object's script that we use to add visual bullets
    public Orbit orbit;


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        charge = 0;
        bullets = 0;
    }

    // Update is called once per frame
    void Update()
    {

        // When left clicking somewhere or with auto accelerate
        if (Input.GetMouseButton(0) || autoAccelerate) {
            // If we specifically left clicked, turn off auto accelerate
            if (Input.GetMouseButton(0)) {
                autoAccelerate = false;
            }

            // Point the player towards the mouse
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 vectorToTarget = worldPosition - (Vector2) transform.position;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, vectorToTarget);

            // Place the destinationObj where we're going and show it
            destination.transform.position = worldPosition;
            destination.show();

            // Add remaining movement so the player moves to the direction of the mouse
            remainingMovement = vectorToTarget.magnitude;
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

            // Charge up while we are moving
            updateCharge(chargeGrowth * Time.deltaTime);
        } else {
            // If we've run out of movement and stopped moving, hide the destination
            spriteRenderer.sprite = keyframes[0];
            destination.hide();

            // Charge down while we aren't moving
            updateCharge(-chargeDecay * Time.deltaTime);
        }

        // Right click will...
        if (Input.GetMouseButtonDown(1)) {
            autoAccelerate = !autoAccelerate;
        }

        // Spacebar will...
        if (Input.GetKeyDown(KeyCode.Space)) {
            // Shoot bullet only if we have enough to shoot a bullet
            if (getAvailableBullets() >= 1) {
                Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 vectorToTarget = worldPosition - (Vector2) transform.position;
                Quaternion directionToTarget = Quaternion.LookRotation(Vector3.forward, vectorToTarget);
                Instantiate(bulletPrefab, transform.position + transform.up * bulletSpawnOffsetUp, directionToTarget);

                // Remove bullet since we just fired
                removeBullet();
            }
        }
    }

    // Increase/Decrease charge by amount
    public void updateCharge(float amount)
    {
        if (bullets >= bulletMax && amount >= 0) {
            return;
        } else if (bullets == 0 && charge <= 0 && amount < 0) {
            return;
        }
        charge += amount;
        if (charge < 0) {
            charge = 0;
            removeBullet();
        } else if (charge >= bulletChargeCost) {
            charge = 0;
            addBullet();
        }
    }

    // Reset charge amount
    public void chargeReset()
    {
        charge = 0f;
    }

    // Get raw charge value
    public float getCharge()
    {
        return charge;
    }

    private void addBullet()
    {
        bullets += 1;
        orbit.addBullet();
    }

    private void removeBullet()
    {
        bullets -= 1;
        orbit.removeBullet();
    }

    // Gets how many bullets we have available
    public int getAvailableBullets()
    {
        return bullets;
    }

    public void resetMovement()
    {
        remainingMovement = 0f;
    }

    public void incrementSpeed(float amount)
    {
        moveSpeed += amount;
    }
}
