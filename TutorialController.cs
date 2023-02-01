using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    public Sprite checkBoxEmpty;
    public Sprite checkBoxFilled;
    public GameObject tutorialPanel;
    public TextMeshProUGUI tutorialTitle;
    public TextMeshProUGUI tutorialText;
    public Image tutorialCheckbox;
    public GameObject player;

    public float stepDelayTime;
    public float respawnDelay;

    private GameObject currentObjective;

    public int stepIndex = 0;
    public int endTutorialStep = 1;

    public GameObject[] objectives;

    [TextArea(1, 8)]
    public string[] dialogue;

    // Start is called before the first frame update
    void Start()
    {
        showPanel(true);
        setUpStep(stepIndex);
    }

    // Update is called once per frame
    void Update()
    {
        // Once the currentObjective gameObject has no children it's time for
        // the next step
        if (currentObjective && currentObjective.transform.childCount == 0) {
            Destroy(currentObjective);
            StartCoroutine(NextStepAfterDelay());
        }

        // TODO eventually build pause screen but Escape key will return us to Main Menu for now
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene(0);
        }
    }

    void showPanel(bool visibility)
    {
        tutorialPanel.SetActive(visibility);
    }

    public void simulateGameOver()
    {
        // Destroy all the children of the current objective so we can go to the
        // next step
        foreach (Transform child in currentObjective.transform) {
            Destroy(child.gameObject);
        }

        StartCoroutine(simulateRespawn());
    }

    IEnumerator simulateRespawn()
    {
        yield return new WaitForSeconds(respawnDelay);

        // Reset the player's position
        player.transform.position = Vector3.zero;
        player.transform.rotation = Quaternion.identity;
        player.GetComponent<Player>().resetMovement();
        player.SetActive(true);

    }

    void setStepText(int dialogueIndex)
    {
        tutorialTitle.text = "Part " + (dialogueIndex + 1);
        tutorialText.text = dialogue[dialogueIndex];
    }

    void setUpStep(int stepIndex)
    {
        if (stepIndex == endTutorialStep) {
            SceneManager.LoadScene(1);
            return;
        }
        currentObjective = objectives[stepIndex];
        currentObjective.SetActive(true);
        setCheckmark(false);
        setStepText(stepIndex);
    }

    void setCheckmark(bool active) {
        tutorialCheckbox.sprite = active ? checkBoxFilled : checkBoxEmpty;
    }

    IEnumerator NextStepAfterDelay()
    {
        setCheckmark(true);
        yield return new WaitForSeconds(stepDelayTime);
        stepIndex += 1;
        setUpStep(stepIndex);
    }
}
