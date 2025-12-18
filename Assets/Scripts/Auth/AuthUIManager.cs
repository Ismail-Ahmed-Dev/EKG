using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class AuthUIManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject loginPanel;
    public GameObject signupPanel;

    [Header("Login Fields")]
    public TMP_InputField loginEmail;
    public TMP_InputField loginPassword;
    public Button loginButton;
    public Button goToSignupBtn;

    [Header("Signup Fields")]
    public TMP_InputField signupName;
    public TMP_InputField signupEmail;
    public TMP_InputField signupPassword;
    public Button signupButton;
    public Button goToLoginBtn;

    [Header("Feedback")]
    public TMP_Text messageText;
    //public GameObject loadingPanel;

    private AuthService authService;
    private DatabaseService dbService;

    private void Start()
    {
        authService = new AuthService();
        dbService = new DatabaseService();

        // Setup button listeners
        loginButton.onClick.AddListener(OnLoginClicked);
        signupButton.onClick.AddListener(OnSignupClicked);
        goToSignupBtn.onClick.AddListener(() => ShowPanel(false));
        goToLoginBtn.onClick.AddListener(() => ShowPanel(true));

        // Check if user is already logged in
        FirebaseManager.Instance.OnAuthStateChanged += OnUserLoggedIn;

        ShowPanel(true);
        //HideLoading();
    }

    private void OnUserLoggedIn(Firebase.Auth.FirebaseUser user)
    {
        if (user != null)
        {
            // User is logged in, go to main scene
            SceneManager.LoadScene("MainScene");
        }
    }

    private async void OnLoginClicked()
    {
        string email = loginEmail.text.Trim();
        string password = loginPassword.text;

        if (!ValidateLoginInputs(email, password)) return;

        //ShowLoading();

        var result = await authService.Login(email, password);

        //HideLoading();

        if (result.success)
        {
            ShowMessage(result.message, Color.green);
            // Will automatically transition via OnUserLoggedIn
        }
        else
        {
            ShowMessage(result.message, Color.red);
        }
    }

    private async void OnSignupClicked()
    {
        string name = signupName.text.Trim();
        string email = signupEmail.text.Trim();
        string password = signupPassword.text;

        if (!ValidateSignupInputs(name, email, password)) return;

        //ShowLoading();

        var result = await authService.Signup(email, password);

        if (result.success)
        {
            // Save user data
            UserData userData = new UserData(name, email);
            bool saved = await dbService.SaveUserData(result.user.UserId, userData);

            //HideLoading();

            if (saved)
            {
                ShowMessage("Account created successfully! Logging in...", Color.green);
                await System.Threading.Tasks.Task.Delay(1500);
                ShowPanel(true);
            }
            else
            {
                ShowMessage("Account created but failed to save data", Color.yellow);
            }
        }
        else
        {
            //HideLoading();
            ShowMessage(result.message, Color.red);
        }
    }

    private bool ValidateLoginInputs(string email, string password)
    {
        if (string.IsNullOrEmpty(email))
        {
            ShowMessage("Please enter your email", Color.red);
            return false;
        }
        if (string.IsNullOrEmpty(password))
        {
            ShowMessage("Please enter your password", Color.red);
            return false;
        }
        return true;
    }

    private bool ValidateSignupInputs(string name, string email, string password)
    {
        if (string.IsNullOrEmpty(name))
        {
            ShowMessage("Please enter your name", Color.red);
            return false;
        }
        if (string.IsNullOrEmpty(email))
        {
            ShowMessage("Please enter your email", Color.red);
            return false;
        }
        if (string.IsNullOrEmpty(password))
        {
            ShowMessage("Please enter your password", Color.red);
            return false;
        }
        if (password.Length < 6)
        {
            ShowMessage("Password must be at least 6 characters", Color.red);
            return false;
        }
        return true;
    }

    private void ShowPanel(bool showLogin)
    {
        loginPanel.SetActive(showLogin);
        signupPanel.SetActive(!showLogin);
        ClearInputs();
        messageText.text = "";
    }

    private void ShowMessage(string message, Color color)
    {
        messageText.text = message;
        messageText.color = color;
    }

    //private void ShowLoading()
    //{
    //    if (loadingPanel != null)
    //        loadingPanel.SetActive(true);
    //}

    //private void HideLoading()
    //{
    //    if (loadingPanel != null)
    //        loadingPanel.SetActive(false);
    //}

    private void ClearInputs()
    {
        loginEmail.text = "";
        loginPassword.text = "";
        signupName.text = "";
        signupEmail.text = "";
        signupPassword.text = "";
    }

    private void OnDestroy()
    {
        if (FirebaseManager.Instance != null)
            FirebaseManager.Instance.OnAuthStateChanged -= OnUserLoggedIn;
    }
}