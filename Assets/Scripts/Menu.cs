using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public Canvas menuCanvas;
    public Canvas scoreCanvas;
    public Canvas tutorial1Canvas;
    public Canvas tutorial2Canvas;

    public GameObject buttonMenu;
    public GameObject buttonScore;

    public bool tuto1;
    public bool tuto2;
    public bool isPressing;

    EventSystem eventSystem;

    private void Start()
    {
        eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
    }

    public void LaunchGame()
    {
        tuto2 = false;
        tutorial2Canvas.gameObject.SetActive(false);
        SceneManager.LoadScene(1);
    }

    public void ScoreButton()
    {
        eventSystem.SetSelectedGameObject(buttonScore);
        menuCanvas.gameObject.SetActive(false);
        scoreCanvas.gameObject.SetActive(true);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void BackButton()
    {
        eventSystem.SetSelectedGameObject(buttonMenu);
        menuCanvas.gameObject.SetActive(true);
        scoreCanvas.gameObject.SetActive(false);
    }

    public void ResetButton(ScoreboardHanlder scoreboard)
    {
        scoreboard.Reset();
    }

}