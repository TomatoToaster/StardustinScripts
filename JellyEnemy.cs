using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JellyEnemy : MonoBehaviour
{
    public GameObject player;
    public float moveSpeed = 3f;
    public float rotateSpeed = 90f;
    private GameController gameController;
    private TutorialController tutorialController;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        tutorialController = GameObject.FindGameObjectWithTag("GameController").GetComponent<TutorialController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 betweenVector = player.transform.position - transform.position;
        Quaternion betweenRotation = Quaternion.LookRotation(Vector3.forward, betweenVector);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, betweenRotation, Time.deltaTime * rotateSpeed);
        transform.position += transform.up * moveSpeed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player") {
            col.gameObject.SetActive(false);
            if (gameController) {
                gameController.ReloadLevel();
            }
            if (tutorialController) {
                tutorialController.simulateGameOver();
            }
        }
    }
}
