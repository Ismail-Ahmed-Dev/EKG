using Firebase.Database;
using System;
using System.Threading.Tasks;
using UnityEngine;

public class DatabaseService
{
    private DatabaseReference dbRef;

    public DatabaseService()
    {
        // التعديل هنا: نأخذ المرجع الجاهز من FirebaseManager بدلاً من خلقه من جديد
        if (FirebaseManager.Instance != null)
        {
            dbRef = FirebaseManager.Instance.DbReference;
        }
    }

    // Save user data (كما هي لم تتغير)
    public async Task<bool> SaveUserData(string userId, UserData data)
    {
        // حماية إضافية
        if (dbRef == null)
        {
            Debug.LogError("Database Reference is missing!");
            return false;
        }

        try
        {
            string json = JsonUtility.ToJson(data);
            await dbRef.Child("users").Child(userId).SetRawJsonValueAsync(json);
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Save Error: {ex.Message}");
            return false;
        }
    }

    // Get user data (هنا كان يحدث الخطأ)
    public async Task<UserData> GetUserData(string userId)
    {
        // 1. فحص الأمان: هل المرجع موجود؟
        if (dbRef == null)
        {
            // محاولة أخيرة لجلبه من المدير
            if (FirebaseManager.Instance != null)
                dbRef = FirebaseManager.Instance.DbReference;

            // إذا ظل فارغاً، نرمي خطأ واضح بدلاً من تحطيم اللعبة
            if (dbRef == null)
            {
                Debug.LogError("Critical Error: Database reference is null. Check Firebase Console configuration.");
                return null;
            }
        }

        try
        {
            // هذا السطر (57 سابقاً) هو الذي كان يسبب المشكلة إذا كان dbRef فارغاً
            var snapshot = await dbRef.Child("users").Child(userId).GetValueAsync();

            if (snapshot.Exists)
            {
                string json = snapshot.GetRawJsonValue();
                return JsonUtility.FromJson<UserData>(json);
            }
            else
            {
                Debug.LogWarning("User data not found for ID: " + userId);
                return null;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error loading data: " + ex.Message);
            return null;
        }
    }

    // Update stars count
    public async Task<bool> UpdateStars(string userId, int stars)
    {
        try
        {
            await dbRef.Child("users").Child(userId).Child("stars").SetValueAsync(stars);
            Debug.Log($"✅ Stars updated: {stars}");
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"❌ Error updating stars: {ex.Message}");
            return false;
        }
    }

    public async Task AddStars(string userId, int starsEarned)
    {
        if (dbRef == null) return;

        Debug.Log($"⏳ Attempting to add {starsEarned} stars for user: {userId}");

        try
        {
            // 1. Fetch current star count
            var snapshot = await dbRef.Child("users").Child(userId).Child("stars").GetValueAsync();

            long currentStars = 0;

            if (snapshot.Exists && snapshot.Value != null)
            {
                // Safely convert data regardless of type (int/long/double)
                currentStars = System.Convert.ToInt64(snapshot.Value);
            }

            Debug.Log($"🌟 Current stars: {currentStars}");

            // 2. Calculate new total
            long newTotal = currentStars + starsEarned;

            // 3. Save to database
            await dbRef.Child("users").Child(userId).Child("stars").SetValueAsync(newTotal);

            Debug.Log($"✅ Saved successfully! New total: {newTotal}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"❌ Failed to save stars: {ex.Message}");
        }
    }

    // Update avatar
    public async Task<bool> UpdateAvatar(string userId, string avatarId)
    {
        try
        {
            await dbRef.Child("users").Child(userId).Child("avatar").SetValueAsync(avatarId);
            Debug.Log($"✅ Avatar updated: {avatarId}");
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"❌ Error updating avatar: {ex.Message}");
            return false;
        }
    }

    // Update character
    public async Task<bool> UpdateCharacter(string userId, string characterId)
    {
        try
        {
            await dbRef.Child("users").Child(userId).Child("character").SetValueAsync(characterId);
            Debug.Log($"✅ Character updated: {characterId}");
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"❌ Error updating character: {ex.Message}");
            return false;
        }
    }
}