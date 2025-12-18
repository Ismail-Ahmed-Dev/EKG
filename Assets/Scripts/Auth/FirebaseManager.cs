using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions; // مهم جداً لهذه المكتبة
using System;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance { get; private set; }
    public FirebaseAuth Auth { get; private set; }
    public DatabaseReference DbReference { get; private set; }

    // متغير لمعرفة هل الجاهزية تمت أم لا
    public bool IsInitialized { get; private set; } = false;

    public event Action<FirebaseUser> OnAuthStateChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeFirebase();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeFirebase()
    {
        Debug.Log("Starting Firebase Initialization...");

        // استخدام ContinueWithOnMainThread بدلاً من ContinueWith العادية
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;

            if (dependencyStatus == DependencyStatus.Available)
            {
                // تهيئة Authentication
                Auth = FirebaseAuth.DefaultInstance;

                // تهيئة Database (مع معالجة حالة عدم وجود الرابط)
                try
                {
                    DbReference = FirebaseDatabase.DefaultInstance.RootReference;
                }
                catch (Exception ex)
                {
                    Debug.LogError($"❌ Database URL missing or setup error: {ex.Message}");
                }

                IsInitialized = true;

                // Monitor authentication state changes
                if (Auth != null)
                {
                    Auth.StateChanged += HandleAuthStateChanged;
                    Debug.Log("✅ Firebase initialized successfully");
                }
            }
            else
            {
                Debug.LogError($"❌ Could not initialize Firebase: {dependencyStatus}");
            }
        });
    }

    private void HandleAuthStateChanged(object sender, EventArgs e)
    {
        if (Auth.CurrentUser != null)
        {
            Debug.Log($"✅ User signed in: {Auth.CurrentUser.Email}");
            OnAuthStateChanged?.Invoke(Auth.CurrentUser);
        }
        else
        {
            OnAuthStateChanged?.Invoke(null);
        }
    }

    private void OnDestroy()
    {
        if (Auth != null)
        {
            Auth.StateChanged -= HandleAuthStateChanged;
        }
    }
}