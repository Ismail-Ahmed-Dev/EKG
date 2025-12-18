using UnityEngine;
using Firebase.Auth;
using System;
using System.Threading.Tasks;

public class AuthService
{
    private FirebaseAuth auth
    {
        get
        {
            if (FirebaseManager.Instance != null && FirebaseManager.Instance.Auth != null)
                return FirebaseManager.Instance.Auth;
            return null;
        }
    }
    // Login user
    public async Task<(bool success, string message, FirebaseUser user)> Login(string email, string password)
    {
        if (auth == null)
        {
            return (false, "Firebase Auth not ready. Check internet or settings.", null);
        }
        try
        {
            var result = await auth.SignInWithEmailAndPasswordAsync(email, password);
            return (true, "Login successful!", result.User);
        }
        catch (Exception ex)
        {
            return (false, GetErrorMessage(ex), null);
        }
    }

    // Create new account
    public async Task<(bool success, string message, FirebaseUser user)> Signup(string email, string password)
    {
        if (auth == null)
        {
            return (false, "Firebase Auth not ready. Check internet or settings.", null);
        }
        try
        {
            var result = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
            return (true, "Account created successfully!", result.User);
        }
        catch (Exception ex)
        {
            return (false, GetErrorMessage(ex), null);
        }
    }

    // Logout user
    public void Logout()
    {

        auth.SignOut();
        Debug.Log("User logged out");
    }

    // Get user-friendly error messages
    private string GetErrorMessage(Exception ex)
    {
        string error = ex.Message.ToLower();

        if (error.Contains("email") && error.Contains("already"))
            return "Email is already in use";
        if (error.Contains("password") && error.Contains("weak"))
            return "Password is too weak";
        if (error.Contains("invalid-email"))
            return "Invalid email address";
        if (error.Contains("user-not-found"))
            return "User not found";
        if (error.Contains("wrong-password"))
            return "Wrong password";
        if (error.Contains("network"))
            return "Network error";

        return "Error: " + ex.Message;
    }
}   