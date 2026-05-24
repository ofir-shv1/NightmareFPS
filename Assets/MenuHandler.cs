using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    public void StartGame()
    {
        // This command loads your game scene
        SceneManager.LoadScene("SampleScene");
    }
}