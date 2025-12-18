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
    public GameObject loadingPanel;

    private AuthService authService;
    private DatabaseService dbService;

    private void Start()
    {
        authService = new AuthService();
        dbService = new DatabaseService();

        // تعريف الأزرار
        loginButton.onClick.AddListener(OnLoginClicked);
        signupButton.onClick.AddListener(OnSignupClicked);
        goToSignupBtn.onClick.AddListener(() => ShowPanel(false));
        goToLoginBtn.onClick.AddListener(() => ShowPanel(true));

        // 1. إخفاء كل شيء مبدئياً لضمان عدم تداخل الشاشات
        loginPanel.SetActive(false);
        signupPanel.SetActive(false);
        if (loadingPanel != null) loadingPanel.SetActive(false); // تأكد أن التحميل مخفي

        // 2. الاشتراك في الحدث
        FirebaseManager.Instance.OnAuthStateChanged += OnAuthStateChanged;

        // 3. (الإصلاح هنا) التحقق اليدوي الفوري
        if (FirebaseManager.Instance.Auth.CurrentUser != null)
        {
            // ✅ حالة 1: المستخدم مسجل -> اذهب للعبة
            OnAuthStateChanged(FirebaseManager.Instance.Auth.CurrentUser);
        }
        else
        {
            // ✅ حالة 2: المستخدم غير مسجل (راجع من Logout) -> اظهر شاشة الدخول
            ShowPanel(true);
        }
    }

    private void OnAuthStateChanged(Firebase.Auth.FirebaseUser user)
    {
        if (user != null)
        {
            // ✅ المستخدم مسجل من قبل (تذكرني) -> اذهب للقائمة الرئيسية فوراً
            Debug.Log("User already logged in. Redirecting to MainMenu...");
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            // ❌ لا يوجد مستخدم محفوظ -> اظهر شاشة تسجيل الدخول
            ShowPanel(true); // true يعني اظهر Login
            if (messageText != null) messageText.text = "";
        }
    }

    private async void OnLoginClicked()
    {
        string email = loginEmail.text.Trim();
        string password = loginPassword.text;

        if (!ValidateLoginInputs(email, password)) return;

        ShowLoading();
        ShowMessage("", Color.white);

        var result = await authService.Login(email, password);

        HideLoading();

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

        // 1. التحقق من صحة المدخلات (قبل التحميل)
        if (!ValidateSignupInputs(name, email, password)) return;

        // 2. تفعيل شاشة التحميل
        ShowLoading();
        ShowMessage("", Color.white); // مسح أي رسائل سابقة

        // 3. محاولة إنشاء الحساب
        var result = await authService.Signup(email, password);

        // 🛑 حماية: التأكد أن الواجهة لم تدمر أثناء الانتظار
        if (this == null || gameObject == null) return;

        if (result.success)
        {
            // 4. حفظ بيانات المستخدم الإضافية
            UserData userData = new UserData(name, email);
            bool saved = await dbService.SaveUserData(result.user.UserId, userData);

            // 🛑 حماية ثانية بعد الانتظار الثاني
            if (this == null || gameObject == null) return;

            if (saved)
            {
                // ✅ نجاح تام:
                // لن نقوم بإخفاء التحميل (HideLoading) هنا،
                // لأننا نريد أن يظل المستخدم يرى "Loading" حتى يتم نقله للمشهد التالي تلقائياً
                ShowMessage("Account created! Entering game...", Color.green);
            }
            else
            {
                // ⚠️ الحساب أنشئ لكن البيانات لم تحفظ:
                HideLoading(); // نخفي التحميل ليراها المستخدم
                ShowMessage("Account created but failed to save data", Color.yellow);
            }
        }
        else
        {
            // ❌ فشل إنشاء الحساب:
            HideLoading(); // ضروري جداً إخفاء التحميل ليتمكن من المحاولة مجدداً
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

    private void ShowLoading()
    {
        if (loadingPanel != null)
            loadingPanel.SetActive(true);
    }

    private void HideLoading()
    {
        if (loadingPanel != null)
            loadingPanel.SetActive(false);
    }

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
            FirebaseManager.Instance.OnAuthStateChanged -= OnAuthStateChanged;
    }
}