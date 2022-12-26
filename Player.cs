using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    public float chargeMax = 1;
    public float chargeGrowth = 0.2f;
    public float chargeDecay = 0.2f;
    public float bulletChargeCost = 1;

    // The amount of charge (each bullet costs 1) that the player has
    private float charge;

    // Whether the player should just keep moving towards the cursor without checking for click
    public bool autoAccelerate = false;

    // Destination object's script that we use to show where the ship is currently traveling
    public Destination destination;


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        charge = 0;
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
            chargeUp(chargeGrowth * Time.deltaTime);
        } else {
            // If we've run out of movement and stopped moving, hide the destination
            spriteRenderer.sprite = keyframes[0];
            destination.hide();

            // Charge down while we aren't moving
            chargeDown(chargeDecay * Time.deltaTime);
        }

        // Right click will...
        if (Input.GetMouseButtonDown(1)) {
            // Shoot bullet only if we have enough to shoot a bullet
            if (getAvailableBullets() >= 1) {
                Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 vectorToTarget = worldPosition - (Vector2) transform.position;
                Quaternion directionToTarget = Quaternion.LookRotation(Vector3.forward, vectorToTarget);
                Instantiate(bulletPrefab, transform.position + transform.up * bulletSpawnOffsetUp, directionToTarget);

                // Remove charge for the cost of the bullet we just fired
                chargeDown(bulletChargeCost);
            }
        }

        // Spacebar will toggle auto accelerate
        if (Input.GetKeyDown(KeyCode.Space)) {
            autoAccelerate = !autoAccelerate;
        }
    }

    // Increase charge by amount
    public void chargeUp(float amount)
    {
        if (charge >= chargeMax) {
            return;
        }
        charge = Mathf.Min(charge + amount, chargeMax);
    }

    // Decrease charge by amount
    public void chargeDown(float amount)
    {
        if (charge <= 0f) {
            return;
        }
        charge = Mathf.Max(charge - amount, 0f);
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

    // Gets how many bullets we have available
    public int getAvailableBullets()
    {
        return Mathf.FloorToInt(charge / bulletChargeCost);
    }
}
