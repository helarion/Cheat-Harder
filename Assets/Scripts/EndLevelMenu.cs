using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevelMenu : MonoBehaviour
{
    public TextMeshProUGUI playerName;

    public void Save()
    {
        if (!playerName.text.Equals(""))
        {
            GameManager.instance.SaveLastRecord(playerName.text);
            SceneManager.LoadScene(0);
        }
    }
}
