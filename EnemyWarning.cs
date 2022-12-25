using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWarning : MonoBehaviour
{
    private SpriteRenderer myRenderer;
    private SpriteRenderer parentRenderer;

    private float topEdge;
    private float rightEdge;
    private Vector3 initialScale;
    public float scalingFactor;
    public float enlargeningStartDistance;
    void Start()
    {
        myRenderer = gameObject.GetComponent<SpriteRenderer>();
        parentRenderer = transform.parent.GetComponent<SpriteRenderer>();
        myRenderer.enabled = !parentRenderer.isVisible;
        topEdge = Camera.main.orthographicSize;
        rightEdge = topEdge * Camera.main.aspect;
        initialScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        // If the parent is visible, we don't need to see this warning, so hide
        // its renderer and do nothing else
        if (parentRenderer.isVisible) {
            myRenderer.enabled = false;
            return;

        // If the enemy ends up going back out of bounds, we need to reactivate the renderer
        } else if (!myRenderer.enabled) {
            myRenderer.enabled = true;
        }
        Vector3 parentPosition = transform.parent.position;
        Vector2 mySize = myRenderer.bounds.size;

        // On the right side
        if (parentPosition.x >= rightEdge) {
            transform.position = new Vector3(rightEdge - mySize.x / 2, parentPosition.y, parentPosition.z);
            transform.rotation = Quaternion.Euler(0, 0, 270);

        // On the left side
        } else if (parentPosition.x <= -rightEdge) {
            transform.position = new Vector3(-rightEdge + mySize.x / 2, parentPosition.y, parentPosition.z);
            transform.rotation = Quaternion.Euler(0, 0, 90);

        // On the top side
        } else if (parentPosition.y >= topEdge) {
            transform.position = new Vector3(parentPosition.x, topEdge - mySize.y / 2, parentPosition.z);
            transform.rotation = Quaternion.Euler(0, 0, 0);

        // On the bottom side
        } else if (parentPosition.y <= -topEdge) {
            transform.position = new Vector3(parentPosition.x, -topEdge + mySize.y / 2, parentPosition.z);
            transform.rotation = Quaternion.Euler(0, 0, 180);

        }

        // Enlarge the warning as the enemy gets closer
        Enlarge(Vector3.Distance(parentPosition, transform.position));
    }

    private void Enlarge(float dist)
    {
        if (dist >= enlargeningStartDistance) {
            transform.localScale = initialScale;
            return;
        }

        float factor = (enlargeningStartDistance - dist) * scalingFactor;
        transform.localScale = initialScale + factor * new Vector3(1, 1, 1);
    }
}
