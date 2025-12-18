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

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;

            if (dependencyStatus == DependencyStatus.Available)
            {
                // تهيئة Authentication
                Auth = FirebaseAuth.DefaultInstance;

                // =========================================================
                // الحل الجذري لمشكلة Database Reference null
                // =========================================================
                try
                {
                    // 🔴🔴🔴 ضع الرابط الذي نسخته هنا بين علامات التنصيص 🔴🔴🔴
                    string databaseUrl = "https://educational-kids-game-public-default-rtdb.europe-west1.firebasedatabase.app/";

                    // استخدام الرابط المباشر بدلاً من DefaultInstance
                    DbReference = FirebaseDatabase.GetInstance(databaseUrl).RootReference;

                    Debug.Log("✅ Database connected successfully to: " + databaseUrl);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"❌ Database connection failed: {ex.Message}");
                }
                // =========================================================

                IsInitialized = true;

                if (Auth != null)
                {
                    Auth.StateChanged += HandleAuthStateChanged;
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