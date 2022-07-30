using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseControl : MonoBehaviour
{
    // Start is called before the first frame update
    float previousTimeScale = 1;
    //public Text pauselabel;
    public TMP_Text pauseLabel;

    public static bool isPaused = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }


    void TogglePause()
    {
        if (Time.timeScale > 0)
        {
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0;
            AudioListener.pause = true;
            pauseLabel.enabled = true;

            isPaused = true;
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = previousTimeScale;
            AudioListener.pause = false;
            pauseLabel.enabled = false;

            isPaused = false;
        }
    }
}
