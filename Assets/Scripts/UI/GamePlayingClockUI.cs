using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayingClockUI : MonoBehaviour
{
    // Reference to the UI image representing the game timer
    [SerializeField] private UnityEngine.UI.Image timerImage;

    // Update is called once per frame
    private void Update()
    {
        // Set the fill amount of the timer image based on the normalized game playing timer value
        timerImage.fillAmount = GameManager_.Instance.GetGamePlayingTimerNormalized();
    }
}
