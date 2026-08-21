using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game State")]
    public bool gameWon = false;

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
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }
    }

    // =========================================
    // WIN GAME
    // =========================================

    public void WinGame()
    {
        if (gameWon)
            return;

        gameWon = true;

        Debug.Log("🎉 VILLAGE IS INDEPENDENT!");

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }

        // Player movementni keyin shu yerda to'xtatamiz
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