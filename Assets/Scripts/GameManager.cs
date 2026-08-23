using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game State")]
    public bool gameWon = false;

    [Header("Intro")]
    [SerializeField] private GameObject introPanel;

    [Header("Build UI")]
    [SerializeField] private GameObject buildPanel;

    [Header("Build Buttons")]
    [SerializeField] private Button solarButton;
    [SerializeField] private Button wellButton;
    [SerializeField] private Button farmButton;
    [SerializeField] private Button workshopButton;

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
        // BUILD BUTTONS
        // =========================

        // Faqat Solar boshida ochiq
        if (solarButton != null)
        {
            solarButton.interactable = true;
        }

        if (wellButton != null)
        {
            wellButton.interactable = false;
        }

        if (farmButton != null)
        {
            farmButton.interactable = false;
        }

        if (workshopButton != null)
        {
            workshopButton.interactable = false;
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
        if (introPanel != null)
        {
            introPanel.SetActive(false);
        }

        if (buildPanel != null)
        {
            buildPanel.SetActive(true);
        }

        if (objectivePanel != null)
        {
            objectivePanel.SetActive(true);
        }

        if (playerController != null)
        {
            playerController.SetMovementEnabled(true);
        }

        Debug.Log("GAME STARTED!");
    }

    // =========================================
    // UNLOCK WELL
    // =========================================

    public void UnlockWell()
    {
        if (wellButton != null)
        {
            wellButton.interactable = true;
        }

        Debug.Log("Well unlocked!");
    }

    // =========================================
    // UNLOCK FARM
    // =========================================

    public void UnlockFarm()
    {
        if (farmButton != null)
        {
            farmButton.interactable = true;
        }

        Debug.Log("Farm unlocked!");
    }

    // =========================================
    // UNLOCK WORKSHOP
    // =========================================

    public void UnlockWorkshop()
    {
        if (workshopButton != null)
        {
            workshopButton.interactable = true;
        }

        Debug.Log("Workshop unlocked!");
    }

    // =========================================
    // TASK UNLOCK METHODS
    // =========================================

    public void UnlockWellTask()
    {
        Debug.Log(
            "New Objective: Collect 3 materials for Well."
        );
    }

    public void UnlockFarmTask()
    {
        Debug.Log(
            "New Objective: Collect 3 seeds for Farm."
        );
    }

    public void UnlockWorkshopTask()
    {
        Debug.Log(
            "New Objective: Collect 3 parts for Workshop."
        );
    }

    // =========================================
    // WIN GAME
    // =========================================

    public void WinGame()
    {
        if (gameWon)
            return;

        gameWon = true;

        if (victorySound != null)
        {
            AudioSource.PlayClipAtPoint(
                victorySound,
                Vector3.zero,
                victorySoundVolume
            );
        }

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }

        if (buildPanel != null)
        {
            buildPanel.SetActive(false);
        }

        if (objectivePanel != null)
        {
            objectivePanel.SetActive(false);
        }

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