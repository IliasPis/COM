using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetManager : MonoBehaviour
{
    [Header("Scene Reset")]
    public string sceneName; // Name of the current scene for reload

    [Header("References")]
    public SavePrefs boyPrefs; // Reference to the SavePrefs script for the boy
    public SavePrefs girlPrefs; // Reference to the SavePrefs script for the girl

    public void PlayAgain()
    {
        // Step 1: Reset PlayerPrefs
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs cleared.");

        // Step 2: Clear Name Input Fields for Boy and Girl
        if (boyPrefs != null)
        {
            boyPrefs.ResetInputField();
        }

        if (girlPrefs != null)
        {
            girlPrefs.ResetInputField();
        }

        Debug.Log("Input fields for boy and girl cleared.");

        // Step 3: Reload Scene
        ReloadScene();
    }

    private void ReloadScene()
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            sceneName = SceneManager.GetActiveScene().name;
        }

        SceneManager.LoadScene(sceneName);
        Debug.Log("Scene reloaded to initial state.");
    }
}
