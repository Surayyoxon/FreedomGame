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

    [Header("Victory UI")]
    [SerializeField] private GameObject victoryPanel;

    [Header("Victory Sound")]
    [SerializeField] private AudioClip victorySound;
    [SerializeField] private float victorySoundVolume = 1f;

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
        // Intro ochiq
        if (introPanel != null)
        {
            introPanel.SetActive(true);
        }

        // Build UI boshida yopiq
        if (buildPanel != null)
        {
            buildPanel.SetActive(false);
        }

        // Victory boshida yopiq
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
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