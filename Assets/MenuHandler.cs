using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour
{
    public void StartGame()
    {
        // הפקודה הזו טוענת את הסצנה של המשחק שלך
        SceneManager.LoadScene("SampleScene");
    }
}