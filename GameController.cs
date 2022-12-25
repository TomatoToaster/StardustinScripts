using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    private int score = 0;
    public TextMeshProUGUI scoreText;
    public GameObject[] enemySpawners;
    public GameObject dustPrefab;
    private Player playerRef;
    public int initialMinDust;
    public int initialMaxDust;
    private int minDust;
    private int maxDust;
    public float scoreToSpeedFactor;



    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        resetDustSpawnRate();
        spawnEdges();
        SpawnEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space)) {
        //     SpawnEnemy();
        // }

        // TODO eventually build pause screen but Escape key will return us to Main Menu for now
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene(0);
        }
    }

    // Adds a score to the total in the UI
    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = "Score: " + score;
    }

    // Spawn an enemy in one of the enemySpawners
    public void SpawnEnemy()
    {
        float speedIncrease = score * scoreToSpeedFactor;
        enemySpawners[Random.Range(0, enemySpawners.Length)].GetComponent<EnemySpawner>().SpawnJellyEnemy(speedIncrease);

    }

    // Spawns dust pickups at position provided (usually after enemy dies)
    public void SpawnDust(Vector2 pos)
    {
        int dustAmount = Random.Range(minDust, maxDust + 1);
        float rotationShift = 360.0f / dustAmount;
        float zRotation = Random.Range(0, 360);
        for (int i = 0; i < dustAmount; i++) {
            Instantiate(dustPrefab, pos, Quaternion.Euler(0, 0, zRotation));
            zRotation += rotationShift;
        }
    }

    // Modifies the min/max dust spawning temporarily (has to be reset with resetDustSpawnRate())
    public void modifyDustSpawnRate(int minInclusive, int maxInclusive)
    {
        minDust = minInclusive;
        maxDust = maxInclusive;
    }

    public void resetDustSpawnRate()
    {
        minDust = initialMinDust;
        maxDust = initialMaxDust;
    }

    public void spawnEdges()
    {
        float halfHeight = Camera.main.orthographicSize;
        float halfWidth = halfHeight * Camera.main.aspect;

        GameObject topEdge = new GameObject("TopEdge");
        topEdge.transform.position = new Vector2(0, halfHeight);
        EdgeCollider2D topEdgeCollider = topEdge.AddComponent<EdgeCollider2D>();
        topEdgeCollider.points = (new Vector2[] {
            new Vector2(-halfWidth, 0),
            new Vector2(halfWidth, 0)
        });

        GameObject rightEdge = new GameObject("RightEdge");
        rightEdge.transform.position = new Vector2(halfWidth, 0);
        EdgeCollider2D rightEdgeCollider = rightEdge.AddComponent<EdgeCollider2D>();
        rightEdgeCollider.points = (new Vector2[] {
            new Vector2(0, halfHeight),
            new Vector2(0, -halfHeight)
        });

        GameObject bottomEdge = new GameObject("BottomEdge");
        bottomEdge.transform.position = new Vector2(0, -halfHeight);
        EdgeCollider2D bottomEdgeCollider = bottomEdge.AddComponent<EdgeCollider2D>();
        bottomEdgeCollider.points = (new Vector2[] {
            new Vector2(-halfWidth, 0),
            new Vector2(halfWidth, 0)
        });

        GameObject leftEdge = new GameObject("LeftEdge");
        leftEdge.transform.position = new Vector2(-halfWidth, 0);
        EdgeCollider2D leftEdgeCollider = leftEdge.AddComponent<EdgeCollider2D>();
        leftEdgeCollider.points = (new Vector2[] {
            new Vector2(0, halfHeight),
            new Vector2(0, -halfHeight)
        });

    }

    // Reloads the Scene after waiting a bit, using WaitToRestart()
    public void ReloadLevel()
    {
        StartCoroutine(WaitToRestart());
    }

    private IEnumerator WaitToRestart()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(1);
    }

}
