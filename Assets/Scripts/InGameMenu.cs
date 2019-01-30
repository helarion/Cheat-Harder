using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    public void ResumeGame()
    {
        GameManager.instance.SetMenu(false);
    }

    public void ExitGame()
    {
        SceneManager.LoadScene(0);
    }
}
