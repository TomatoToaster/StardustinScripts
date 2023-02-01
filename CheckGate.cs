using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGate : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite completedSprite;
    public float waitTime;

    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player") {
            spriteRenderer.sprite = completedSprite;
            StartCoroutine(destroyAfterDelay());
        }
    }
    IEnumerator destroyAfterDelay()
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
}
