using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public static bool is_paused;
    public static MenuMode MenuMode;
    public Text MenuTitle;
    public Text MenuButtonText;
    public Text Score;

    public static int Scores { get; set; }

    public void StartButtonClick()
    {
        is_paused = false;
        this.gameObject.SetActive(false);
        MenuMode = MenuMode.Game;
    }

    public void ExitButtonClick()
    {
        Application.Quit();
    }

    void Start()
    {
        is_paused = true;
        Score.gameObject.SetActive(false);
    }

    void Update()
    {
        switch (MenuMode)
        {
            case MenuMode.Start:
                MenuTitle.text = "GTA 2D";
                MenuButtonText.text = "Поехали!";
                break;
            case MenuMode.Pause:
                MenuTitle.text = "Пауза";
                MenuButtonText.text = "Продолжить";
                Score.gameObject.SetActive(false);
                break;
            case MenuMode.GameOver:
                MenuTitle.text = "Игра закончена!";
                MenuButtonText.text = "Начать заново?";
                Score.gameObject.SetActive(true);
                Score.text = Scores.ToString();
                break;
        }

        if(Input.GetKeyDown(KeyCode.Escape) && (MenuMode == MenuMode.Game || MenuMode == MenuMode.Pause))
            StartButtonClick();
    }
}

public enum MenuMode
{
    Start,
    Game,
    Pause,
    GameOver
}