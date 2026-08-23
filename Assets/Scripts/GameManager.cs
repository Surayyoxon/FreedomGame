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

    [Header("Dependency UI")]
    [SerializeField] private GameObject dependencyPanel;

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
        // =========================================
        // INTRO
        // =========================================

        if (introPanel != null)
        {
            introPanel.SetActive(true);
        }

        // =========================================
        // BUILD UI
        // =========================================

        if (buildPanel != null)
        {
            buildPanel.SetActive(false);
        }

        // =========================================
        // OBJECTIVE UI
        // =========================================

        if (objectivePanel != null)
        {
            objectivePanel.SetActive(false);
        }

        // =========================================
        // DEPENDENCY UI
        // =========================================

        if (dependencyPanel != null)
        {
            dependencyPanel.SetActive(false);
        }

        // =========================================
        // VICTORY UI
        // =========================================

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }

        // =========================================
        // BUILD BUTTONS
        // =========================================

        // Faqat Solar boshida ochiq
        if (solarButton != null)
        {
            solarButton.interactable = true;
        }

        // Qolganlari lock
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

        // =========================================
        // PLAYER
        // =========================================

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

        // Dependency UI ochiladi
        if (dependencyPanel != null)
        {
            dependencyPanel.SetActive(true);
        }

        // Player yurishi ochiladi
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
        if (wellButton == null)
        {
            Debug.LogError(
                "Well Button GameManager Inspector'ga ulanmagan!"
            );

            return;
        }

        wellButton.interactable = true;

        Debug.Log(
            "💧 WELL BUTTON UNLOCKED!"
        );
    }

    // =========================================
    // UNLOCK FARM
    // =========================================

    public void UnlockFarm()
    {
        if (farmButton == null)
        {
            Debug.LogError(
                "Farm Button GameManager Inspector'ga ulanmagan!"
            );

            return;
        }

        farmButton.interactable = true;

        Debug.Log(
            "🌾 FARM BUTTON UNLOCKED!"
        );
    }

    // =========================================
    // UNLOCK WORKSHOP
    // =========================================

    public void UnlockWorkshop()
    {
        if (workshopButton == null)
        {
            Debug.LogError(
                "Workshop Button GameManager Inspector'ga ulanmagan!"
            );

            return;
        }

        workshopButton.interactable = true;

        Debug.Log(
            "🔧 WORKSHOP BUTTON UNLOCKED!"
        );
    }

    // =========================================
    // TASK LOGS
    // =========================================

    public void UnlockWellTask()
    {
        Debug.Log(
            "🎯 New Objective: Find 3 materials for Well."
        );
    }

    public void UnlockFarmTask()
    {
        Debug.Log(
            "🎯 New Objective: Find 3 seeds for Farm."
        );
    }

    public void UnlockWorkshopTask()
    {
        Debug.Log(
            "🎯 New Objective: Find 3 parts for Workshop."
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

        // Dependency UI yopiladi
        if (dependencyPanel != null)
        {
            dependencyPanel.SetActive(false);
        }

        // Player to'xtaydi
        if (playerController != null)
        {
            playerController.SetMovementEnabled(false);
        }

        Debug.Log(
            "🏆 VILLAGE IS INDEPENDENT!"
        );
    }

    // =========================================
    // RESTART GAME
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