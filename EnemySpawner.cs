using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject JellyEnemyPrefab;
    public GameObject PlayerRef;
    public float spawnRate = 0f;

    // Only one of these edge offsets should be > 0
    public float topEdgeOffset = 0f;
    public float rightEdgeOffset = 0f;
    public float bottomEdgeOffset = 0f;
    public float leftEdgeOffset = 0f;

    void Start()
    {
        // StartCoroutine(SpawnJellyEnemies());

        // Place the spawner according to the supplied screen offset
        float topEdge = Camera.main.orthographicSize;
        float rightEdge = topEdge * Camera.main.aspect;
        float bottomEdge = -topEdge;
        float leftEdge = -rightEdge;

        Vector2 originalPosition = transform.position;

        if (topEdgeOffset != 0) {
            transform.position = new Vector2(originalPosition.x, topEdge + topEdgeOffset);
        } else if (bottomEdgeOffset != 0) {
            transform.position = new Vector2(originalPosition.x, bottomEdge - bottomEdgeOffset);
        } else if (rightEdgeOffset != 0) {
            transform.position = new Vector2(rightEdge + rightEdgeOffset, originalPosition.y);
        } else if (leftEdgeOffset != 0) {
            transform.position = new Vector2(leftEdge - leftEdgeOffset, originalPosition.y);
        }

        // If no offsets are set, we will simply leave the Spawner in the position provided
    }

    void Update()
    {
    }

    public void SpawnJellyEnemy(float speedIncrease)
    {
        // Spawn a JellyEnemy on the spawner with a random rotation
        GameObject JellyEnemy = Instantiate(JellyEnemyPrefab, transform.position, Quaternion.Euler(0, 0, Random.Range(0, 359)));
        JellyEnemy.GetComponent<JellyEnemy>().player = PlayerRef;
        JellyEnemy.GetComponent<JellyEnemy>().moveSpeed += speedIncrease;
    }

    IEnumerator SpawnJellyEnemiesRoutine() {
        while(true) {

            yield return new WaitForSeconds(spawnRate);
        }
    }
}
