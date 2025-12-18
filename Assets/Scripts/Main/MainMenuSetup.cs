using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuSetup : MonoBehaviour
{
    public GameObject gamesPanel;

    void Start()  // ⭐ أضف الـ function دي
    {
        // شيك لو جاي من اللعبة
        if (PlayerPrefs.GetInt("ShowGamesPanel", 0) == 1)
        {
            // إظهار الـ panel
            ShowGamesPanel();

            // مسح الـ flag
            PlayerPrefs.DeleteKey("ShowGamesPanel");
        }
        else
        {
            // لو جاي من مكان تاني، خلي الـ panel مخفي
            HideGamesPanel();
        }
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

    public void StartPuzzle()
    {
        SceneManager.LoadScene("Scene_JigsawGame");
    }

    void Update()
    {
    }
}