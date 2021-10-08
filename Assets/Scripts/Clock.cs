using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clock : MonoBehaviour
{
    private float time;
    private Text Timer;

    void Start()
    {
        time = 0;
        Timer = GetComponent<Text>();
    }

    void Update()
    {
        if (Menu.is_paused) return;

        time += Time.deltaTime;
        int t = (int)time;

        int sec = t % 60;
        int min = t / 60;
        int hour = t / 3600;

        int min2 = 0;
        if (min > 59)
            min2 = min - hour * 60;
        else
            min2 = min;

        Timer.text =
            ((min > 59) ? hour + ":" : "" ) +
            ((min2 < 10) ? "0" + min2 : min2.ToString()) + ":" +
            ((sec < 10) ? "0" + sec : sec.ToString());
    }
}
