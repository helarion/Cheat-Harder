using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreboardHanlder : MonoBehaviour
{
    public TextMeshProUGUI time01;
    public TextMeshProUGUI time02;
    public TextMeshProUGUI time03;
    public TextMeshProUGUI time04;
    public TextMeshProUGUI time05;
    public TextMeshProUGUI time06;
    public TextMeshProUGUI time07;
    public TextMeshProUGUI time08;
    public TextMeshProUGUI time09;
    public TextMeshProUGUI time10;
    public TextMeshProUGUI bonus01;
    public TextMeshProUGUI bonus02;
    public TextMeshProUGUI bonus03;
    public TextMeshProUGUI bonus04;
    public TextMeshProUGUI bonus05;
    public TextMeshProUGUI bonus06;
    public TextMeshProUGUI bonus07;
    public TextMeshProUGUI bonus08;
    public TextMeshProUGUI bonus09;
    public TextMeshProUGUI bonus10;
    public TextMeshProUGUI nick01;
    public TextMeshProUGUI nick02;
    public TextMeshProUGUI nick03;
    public TextMeshProUGUI nick04;
    public TextMeshProUGUI nick05;
    public TextMeshProUGUI nick06;
    public TextMeshProUGUI nick07;
    public TextMeshProUGUI nick08;
    public TextMeshProUGUI nick09;
    public TextMeshProUGUI nick10;

    private void Start()
    {
        Initialization();
        UpdateScoreboard();
    }

    public void Initialization()
    {
        for (int i = 1; i <= 10; i++)
        {
            if (!PlayerPrefs.HasKey("Time" + i)) PlayerPrefs.SetFloat("Time" + i, 0);
        }
    }

    public void Reset()
    {
        PlayerPrefs.DeleteAll();
        Initialization();
        UpdateScoreboard();
    }

    public void UpdateScoreboard()
    {
        time01.text = PlayerPrefs.GetFloat("Time1").ToString();
        time02.text = PlayerPrefs.GetFloat("Time2").ToString();
        time03.text = PlayerPrefs.GetFloat("Time3").ToString();
        time04.text = PlayerPrefs.GetFloat("Time4").ToString();
        time05.text = PlayerPrefs.GetFloat("Time5").ToString();
        time06.text = PlayerPrefs.GetFloat("Time6").ToString();
        time07.text = PlayerPrefs.GetFloat("Time7").ToString();
        time08.text = PlayerPrefs.GetFloat("Time8").ToString();
        time09.text = PlayerPrefs.GetFloat("Time9").ToString();
        time10.text = PlayerPrefs.GetFloat("Time10").ToString();

        bonus01.text = PlayerPrefs.GetInt("Bonus1").ToString();
        bonus02.text = PlayerPrefs.GetInt("Bonus2").ToString();
        bonus03.text = PlayerPrefs.GetInt("Bonus3").ToString();
        bonus04.text = PlayerPrefs.GetInt("Bonus4").ToString();
        bonus05.text = PlayerPrefs.GetInt("Bonus5").ToString();
        bonus06.text = PlayerPrefs.GetInt("Bonus6").ToString();
        bonus07.text = PlayerPrefs.GetInt("Bonus7").ToString();
        bonus08.text = PlayerPrefs.GetInt("Bonus8").ToString();
        bonus09.text = PlayerPrefs.GetInt("Bonus9").ToString();
        bonus10.text = PlayerPrefs.GetInt("Bonus10").ToString();

        nick01.text = PlayerPrefs.GetString("Nick1").ToString();
        nick02.text = PlayerPrefs.GetString("Nick2").ToString();
        nick03.text = PlayerPrefs.GetString("Nick3").ToString();
        nick04.text = PlayerPrefs.GetString("Nick4").ToString();
        nick05.text = PlayerPrefs.GetString("Nick5").ToString();
        nick06.text = PlayerPrefs.GetString("Nick6").ToString();
        nick07.text = PlayerPrefs.GetString("Nick7").ToString();
        nick08.text = PlayerPrefs.GetString("Nick8").ToString();
        nick09.text = PlayerPrefs.GetString("Nick9").ToString();
        nick10.text = PlayerPrefs.GetString("Nick10").ToString();
    }
}
