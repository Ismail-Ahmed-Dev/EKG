using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    [Header("UI References")]
    public GameObject container;
    public Button resumeButton;
    public Button restartButton;
    public Button mainMenuButton;

    [Header("Sound Settings")]
    public Button muteButton;
    public Image muteButtonImage;
    public Sprite soundOnIcon;    
    public Sprite soundOffIcon;   
    public Slider volumeSlider;
    public TextMeshProUGUI volumeText;

    private bool isPaused = false;

    void Start()
    {
        if (resumeButton != null)
            resumeButton.onClick.AddListener(ResumeGame);

        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(GoToMainMenu);
        }

        if (muteButton != null)
            muteButton.onClick.AddListener(ToggleMute);

        if (volumeSlider != null)
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

        if (container != null)
            container.SetActive(false);

        UpdateSoundUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E))
        {
            if (!isPaused)
                PauseGame();
            else
                ResumeGame();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        if (container != null)
            container.SetActive(true);
        Time.timeScale = 0;

        AudioManager.Instance?.StopFootsteps();

        UpdateSoundUI();
    }

    public void ResumeGame()
    {
        isPaused = false;
        if (container != null)
            container.SetActive(false);
        Time.timeScale = 1;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        if (AudioManager.Instance != null)
        {
        AudioManager.Instance?.StopAllSoundsWithoutPlay();
        }
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void ToggleMute()
    {
        AudioManager.Instance?.ToggleMute();
        UpdateSoundUI();
    }

    private void OnVolumeChanged(float volume)
    {
        AudioManager.Instance?.SetMasterVolume(volume);
        UpdateSoundUI();

        if (volume <= 0f && !AudioManager.Instance.IsMuted())
        {
            AudioManager.Instance?.ToggleMute();
            UpdateSoundUI();
        }
        else if (volume > 0f && AudioManager.Instance.IsMuted())
        {
            AudioManager.Instance?.ToggleMute();
            UpdateSoundUI();
        }
    }

    private void UpdateSoundUI()
    {
        if (AudioManager.Instance == null) return;

        if (muteButtonImage != null)
        {
            muteButtonImage.sprite = AudioManager.Instance.IsMuted() ? soundOffIcon : soundOnIcon;
        }

        if (volumeSlider != null)
        {
            if (AudioManager.Instance.IsMuted())
            {
                volumeSlider.SetValueWithoutNotify(0f);
            }
            else
            {
                volumeSlider.SetValueWithoutNotify(AudioManager.Instance.GetMusicVolume());
            }
        }

        if (volumeText != null)
        {
            float volume = AudioManager.Instance.IsMuted() ? 0f : AudioManager.Instance.GetMusicVolume();
            int percentage = Mathf.RoundToInt(volume * 100);
            volumeText.text = $"{percentage}%";
        }
    }

    public bool IsGamePaused()
    {
        return isPaused;
    }
}