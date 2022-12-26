using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destination : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void show()
    {
        spriteRenderer.enabled = true;
    }

    public void hide()
    {
        spriteRenderer.enabled = false;
    }
}
