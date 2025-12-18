using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuSetup : MonoBehaviour {

    [Header("UI")]
    public TMP_Text welcomeText;
    public TMP_Text starText;
    public Image avatarImage;
    public GameObject gamesPanel;

    private AuthService authService;
    private DatabaseService dbService; 

    public async void Start()
    {
        authService = new AuthService();
        dbService = new DatabaseService();

        if (FirebaseManager.Instance == null)
        {
            Debug.LogError("Error: FirebaseManager instance not found. Make sure to start the game from the 'LoginScene'.");
            SceneManager.LoadScene("AuthScene");
            return;
        }

        if (FirebaseManager.Instance.Auth == null)
        {
            Debug.LogError("Error: Firebase Auth is not initialized yet.");
            SceneManager.LoadScene("AuthScene");
            return;
        }
        var currentUser = FirebaseManager.Instance.Auth.CurrentUser;

        if (currentUser == null)
        {
            SceneManager.LoadScene("AuthScene");
            return;
        }

        if (welcomeText != null) welcomeText.text = "Loading...";

        UserData userData = await dbService.GetUserData(currentUser.UserId);

        if (userData != null && welcomeText != null)
        {
            welcomeText.text = "Welcome, " + userData.name;
            starText.text = $"Stars : {userData.stars.ToString()}";
            Sprite loadedSprite = Resources.Load<Sprite>("Lulu_The_Kitty_Witch_Avatar");
            avatarImage.sprite = loadedSprite;
        }
        else if (welcomeText != null)
        {
            welcomeText.text = "Welcome, " + currentUser.Email;
            starText.text = "0";

        }
    }

    public void PressToLogout()
    {
        authService.Logout();
        SceneManager.LoadScene("AuthScene");
    }

    public void PressToGoGame3()
    {
        SceneManager.LoadScene("Game3Scene");
    }

    public void ShowGamesPanel()
    {
        gamesPanel.SetActive(true);
    }

    public void HideGamesPanel()
    {
        gamesPanel.SetActive(false);
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
