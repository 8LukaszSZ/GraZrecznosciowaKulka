using UnityEngine;
using UnityEngine.SceneManagement; 
using TMPro; 

public class GameManager : MonoBehaviour
{
    public Transform playerTransform; 
    public float deathHeight = -10f; 

    [Header("UI Elements")]
    public GameObject gameOverPanel; 
    public TextMeshProUGUI scoreText; 

    private float maxHeight = 0f;
    private bool isGameOver = false;
    private bool hasTouchedPlatform = false;

    void Start()
    {
        maxHeight = playerTransform.position.y;
        gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (isGameOver || !hasTouchedPlatform) return; 

        if (playerTransform.position.y > maxHeight)
        {
            maxHeight = playerTransform.position.y;
        }

        if (playerTransform.position.y < deathHeight)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        isGameOver = true;
        
        scoreText.text = "Your score: " + Mathf.RoundToInt(maxHeight);
        gameOverPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnFirstPlatformTouch(float yPosition)
    {
        if (!hasTouchedPlatform)
        {
            hasTouchedPlatform = true;
            maxHeight = yPosition;
        }
    }
} 