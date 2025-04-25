using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGames : MonoBehaviour
{
    public void RestartCurrentLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        SceneManager.LoadScene(currentSceneIndex);
    }
}