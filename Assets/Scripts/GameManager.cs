using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public GameObject gameOverText;
    public GameObject restartText;

    private DatabaseService dbService;
    private string currentUserId;

    [Header("Game State")]
    public int score = 0;
    public bool isGameOver = false;

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
        dbService = new DatabaseService();

        if (FirebaseManager.Instance != null && FirebaseManager.Instance.Auth.CurrentUser != null)
        {
            currentUserId = FirebaseManager.Instance.Auth.CurrentUser.UserId;
        }
        else
            Debug.LogError("Error in Database");
        InitializeGame();
    }

    private void Update()
    {
        if (isGameOver)
        {
            CheckRestartInput();
        }
    }

    private void InitializeGame()
    {
        isGameOver = false;
        score = 0;
        UpdateScoreUI();

        if (gameOverText != null) gameOverText.SetActive(false);
        if (restartText != null) restartText.SetActive(false);

        AudioManager.Instance?.StopAllSounds();
    }

    public void AddScore(int amount)
    {
        if (isGameOver) return;

        score += amount;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score : {score}";
        }
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;

        if (gameOverText != null) gameOverText.SetActive(true);
        if (restartText != null) restartText.SetActive(true);

        AudioManager.Instance?.PlayDeath();
        SaveScoreToFirebase();
    }

    private void SaveScoreToFirebase()
    {
        // 1. أولاً: تأكد أن الفايربيس موجود (حماية من الأخطاء لو بتجرب المشهد لوحده)
        if (FirebaseManager.Instance == null || FirebaseManager.Instance.Auth == null)
        {
            Debug.LogWarning("⚠️ لا يمكن حفظ النقاط: Firebase غير موجود (هل بدأت اللعبة من LoginScene؟)");
            return;
        }

        // 2. ثانياً: جلب المستخدم الحالي الآن
        var currentUser = FirebaseManager.Instance.Auth.CurrentUser;

        if (currentUser != null)
        {
            // الحفظ باستخدام الـ ID المباشر
            if (score > 0)
            {
                string userId = currentUser.UserId;
                _ = dbService.AddStars(userId, score);
                Debug.Log($"Adding {score} stars to total.");
            }
        }
        else
        {
            Debug.LogWarning("⚠️ No user logged in, score not saved.");
        }
    }

    private void CheckRestartInput()
    {
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    private void RestartGame()
    {
        SaveScoreToFirebase();
        AudioManager.Instance?.StopAllSounds();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}