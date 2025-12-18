using UnityEngine;
using Firebase.Database;
using System.Threading.Tasks;

public class DatabaseService
{
    private DatabaseReference dbRef => FirebaseManager.Instance.DbReference;

    // Save user data
    // داخل DatabaseService.cs

    public async Task<bool> SaveUserData(string userId, UserData userData)
    {
        // فحص الأمان: هل المدير موجود؟ وهل قاعدة البيانات جاهزة؟
        if (FirebaseManager.Instance == null || FirebaseManager.Instance.DbReference == null)
        {
            Debug.LogError("❌ Database is not initialized yet!");
            return false;
        }

        try
        {
            string json = JsonUtility.ToJson(userData);
            await dbRef.Child("users").Child(userId).SetRawJsonValueAsync(json);
            Debug.Log("✅ User data saved successfully");
            return true;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"❌ Error saving data: {ex.Message}");
            return false;
        }
    }

    // Get user data
    public async Task<UserData> GetUserData(string userId)
    {
        try
        {
            var snapshot = await dbRef.Child("users").Child(userId).GetValueAsync();

            if (snapshot.Exists)
            {
                string json = snapshot.GetRawJsonValue();
                UserData userData = JsonUtility.FromJson<UserData>(json);
                Debug.Log($"✅ User data loaded: {userData.name}");
                return userData;
            }
            else
            {
                Debug.LogWarning("⚠️ No user data found");
                return null;
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"❌ Error loading data: {ex.Message}");
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