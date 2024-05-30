using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 70f;
    [SerializeField] private int extraLives = 0;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject gameWinScreen;
    [SerializeField] private TMPro.TextMeshProUGUI timerText;
    [SerializeField] private TMPro.TextMeshProUGUI livesText;
    [SerializeField] private TMPro.TextMeshProUGUI coinsText;

    private CharacterController2D controller;
    private Animator animator;
    private bool isJumping = false;
    private bool isInvincible = false;
    private bool isGameOver = false;
    private float invincibilityTimer = 0f;
    private float startTime = 0f;
    private int coinCount = 0;

    void Start()
    {
        controller = GetComponent<CharacterController2D>();
        animator = GetComponent<Animator>();
        startTime = Time.time;
    }

    void Update()
    {
        if (!isGameOver)
        {
            UpdateText();

            Moving();

            Jumping();

            Invincibling();

            if (Time.time - startTime >= 60)
            {
                GameOver();
            }
        }
    }

    void UpdateText()
    {
        float timeRemaining = 60 - (Time.time - startTime);
        livesText.text = "X " + extraLives;
        timerText.text = "Time: " + Mathf.RoundToInt(timeRemaining);
    }

    void Moving()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        controller.Move(horizontalInput * speed * Time.fixedDeltaTime, false, false);
        animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
    }

    void Jumping()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            controller.Move(0f, false, true); // Используем jump в CharacterController2D
            isJumping = true;
        }
    }

    void Invincibling()
    {
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0)
            {
                isInvincible = false;
                GetComponent<SpriteRenderer>().color = Color.white;
            }
            else
            {
                GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
    }

    void GameOver(bool die = false)
    {
        if (die)
            animator.SetBool("isAlive", false);

        gameOverScreen.SetActive(true);
        Time.timeScale = 0;
        isGameOver = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }

        if (collision.gameObject.CompareTag("Enemy") && !isInvincible)
        {
            if (extraLives > 0)
            {
                extraLives--;
                isInvincible = true;
                invincibilityTimer = 2f;
            }
            else
            {
                GameOver(true);
            }
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Mushroom"))
        {
            extraLives++;
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Finish"))
        {
            int currentLevel = SceneManager.GetActiveScene().buildIndex;
            int nextLevel = currentLevel + 1;

            if (nextLevel < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextLevel);
            }
            else
            {
                gameWinScreen.SetActive(true);
                Time.timeScale = 0;
                isGameOver = true;
            }
        }

        if (other.gameObject.CompareTag("Coin"))
        {
            coinCount++;
            coinsText.text = "X " + coinCount;
            Destroy(other.gameObject);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

