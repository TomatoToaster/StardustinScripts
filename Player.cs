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

    // The amount of charge (out of 1) that the player has
    private float charge;

    // Whether the player should just keep moving towards the cursor without checking for click
    public bool autoAccelerate = false;


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
            chargeUp(chargeGrowth * Time.deltaTime);
        } else {
            spriteRenderer.sprite = keyframes[0];
            chargeDown(chargeDecay * Time.deltaTime);
        }

        // Right click will...
        if (Input.GetMouseButtonDown(1)) {
            // Shoot bullet onlf if we're at max charge
            if (getChargePercent() == 1) {
                Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 vectorToTarget = worldPosition - (Vector2) transform.position;
                Quaternion directionToTarget = Quaternion.LookRotation(Vector3.forward, vectorToTarget);
                Instantiate(bulletPrefab, transform.position + transform.up * bulletSpawnOffsetUp, directionToTarget);
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

    // Get chargePercentage with range: [0-1]f
    public float getChargePercent()
    {
        return charge / chargeMax;
    }
}
