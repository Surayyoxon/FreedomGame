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

        Debug.Log("GAME STARTED!");
    }

    public void WinGame()
    {
        if (gameWon)
            return;

        gameWon = true;

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }

        if (buildPanel != null)
        {
            buildPanel.SetActive(false);
        }

        Debug.Log("VILLAGE IS INDEPENDENT!");
    }

    public void RestartGame()
    {
        Scene currentScene =
            SceneManager.GetActiveScene();

        SceneManager.LoadScene(
            currentScene.name
        );
    }
}