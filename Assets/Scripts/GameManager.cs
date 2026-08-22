using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game State")]
    public bool gameWon = false;

    [Header("Intro")]
    [SerializeField] private GameObject introPanel;

    [Header("Build UI")]
    [SerializeField] private GameObject buildPanel;

    [Header("Objective UI")]
    [SerializeField] private GameObject objectivePanel;

    [Header("Victory UI")]
    [SerializeField] private GameObject victoryPanel;

    [Header("Victory Sound")]
    [SerializeField] private AudioClip victorySound;
    [SerializeField] private float victorySoundVolume = 1f;

    [Header("Player")]
    [SerializeField] private PlayerController playerController;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        // =========================
        // INTRO
        // =========================

        if (introPanel != null)
        {
            introPanel.SetActive(true);
        }

        // =========================
        // BUILD UI
        // =========================

        if (buildPanel != null)
        {
            buildPanel.SetActive(false);
        }

        // =========================
        // OBJECTIVE UI
        // =========================

        if (objectivePanel != null)
        {
            objectivePanel.SetActive(false);
        }

        // =========================
        // VICTORY UI
        // =========================

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }

        // =========================
        // PLAYER
        // =========================

        if (playerController != null)
        {
            playerController.SetMovementEnabled(false);
        }
    }

    // =========================================
    // START GAME
    // =========================================

    public void StartGame()
    {
        // Intro yopiladi
        if (introPanel != null)
        {
            introPanel.SetActive(false);
        }

        // Build UI ochiladi
        if (buildPanel != null)
        {
            buildPanel.SetActive(true);
        }

        // Objective ochiladi
        if (objectivePanel != null)
        {
            objectivePanel.SetActive(true);
        }

        // Player yurishi ochiladi
        if (playerController != null)
        {
            playerController.SetMovementEnabled(true);
        }

        Debug.Log("GAME STARTED!");
    }

    // =========================================
    // WIN GAME
    // =========================================

    public void WinGame()
    {
        if (gameWon)
            return;

        gameWon = true;

        // Victory sound
        if (victorySound != null)
        {
            AudioSource.PlayClipAtPoint(
                victorySound,
                Vector3.zero,
                victorySoundVolume
            );
        }

        // Victory UI
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }

        // Build UI yopiladi
        if (buildPanel != null)
        {
            buildPanel.SetActive(false);
        }

        // Objective UI yopiladi
        if (objectivePanel != null)
        {
            objectivePanel.SetActive(false);
        }

        // Playerni to'xtatish
        if (playerController != null)
        {
            playerController.SetMovementEnabled(false);
        }

        Debug.Log("VILLAGE IS INDEPENDENT!");
    }

    // =========================================
    // RESTART
    // =========================================

    public void RestartGame()
    {
        Scene currentScene =
            SceneManager.GetActiveScene();

        SceneManager.LoadScene(
            currentScene.name
        );
    }
}