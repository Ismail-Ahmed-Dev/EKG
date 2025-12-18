//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//public class SoundSettingsUI : MonoBehaviour
//{
//    [Header("UI References")]
//    public Button muteButton;
//    public Image muteButtonImage;
//    public Sprite soundOnSprite;    
//    public Sprite soundOffSprite;   
//    public Slider masterVolumeSlider;
//    public TextMeshProUGUI volumePercentageText;

//    [Header("Settings")]
//    public bool showVolumePercentage = true;

//    private void Start()
//    {
//        InitializeUI();
//        SetupEventListeners();
//        UpdateUI();
//    }

//    private void InitializeUI()
//    {
//        if (AudioManager.Instance != null)
//        {
//            masterVolumeSlider.value = AudioManager.Instance.GetMusicVolume();
//            UpdateVolumeText(masterVolumeSlider.value);
//        }
//    }

//    private void SetupEventListeners()
//    {
//        if (muteButton != null)
//        {
//            muteButton.onClick.AddListener(ToggleMute);
//        }

//        if (masterVolumeSlider != null)
//        {
//            masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
//        }

//        if (AudioManager.Instance != null)
//        {
//            AudioManager.Instance.OnMuteStateChanged += OnMuteStateChanged;
//            AudioManager.Instance.OnMusicVolumeChanged += OnMusicVolumeChanged;
//        }
//    }

//    private void ToggleMute()
//    {
//        AudioManager.Instance?.ToggleMute();
//    }

//    private void OnMasterVolumeChanged(float volume)
//    {
//        AudioManager.Instance?.SetMasterVolume(volume);
//        UpdateVolumeText(volume);

//        if (volume <= 0f && !AudioManager.Instance.IsMuted())
//        {
//            AudioManager.Instance?.ToggleMute();
//        }
//        else if (volume > 0f && AudioManager.Instance.IsMuted())
//        {
//            AudioManager.Instance?.ToggleMute();
//        }
//    }

//    private void UpdateVolumeText(float volume)
//    {
//        if (volumePercentageText != null && showVolumePercentage)
//        {
//            int percentage = Mathf.RoundToInt(volume * 100);
//            volumePercentageText.text = $"{percentage}%";
//        }
//    }

//    private void OnMuteStateChanged(bool isMuted)
//    {
//        UpdateMuteButtonVisual(isMuted);

//        if (isMuted)
//        {
//            masterVolumeSlider.SetValueWithoutNotify(0f);
//            UpdateVolumeText(0f);
//        }
//        else
//        {
//            float currentVolume = AudioManager.Instance.GetMusicVolume();
//            masterVolumeSlider.SetValueWithoutNotify(currentVolume);
//            UpdateVolumeText(currentVolume);
//        }
//    }

//    private void OnMusicVolumeChanged(float volume)
//    {
//        if (!AudioManager.Instance.IsMuted())
//        {
//            masterVolumeSlider.SetValueWithoutNotify(volume);
//            UpdateVolumeText(volume);
//        }
//    }

//    private void UpdateMuteButtonVisual(bool isMuted)
//    {
//        if (muteButtonImage != null)
//        {
//            muteButtonImage.sprite = isMuted ? soundOffSprite : soundOnSprite;
//        }
//    }

//    private void UpdateUI()
//    {
//        if (AudioManager.Instance != null)
//        {
//            UpdateMuteButtonVisual(AudioManager.Instance.IsMuted());

//            if (!AudioManager.Instance.IsMuted())
//            {
//                float currentVolume = AudioManager.Instance.GetMusicVolume();
//                masterVolumeSlider.SetValueWithoutNotify(currentVolume);
//                UpdateVolumeText(currentVolume);
//            }
//        }
//    }

//    private void OnDestroy()
//    {
//         if (AudioManager.Instance != null)
//        {
//            AudioManager.Instance.OnMuteStateChanged -= OnMuteStateChanged;
//            AudioManager.Instance.OnMusicVolumeChanged -= OnMusicVolumeChanged;
//        }
//    }
//}