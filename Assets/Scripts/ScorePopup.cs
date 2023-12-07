using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScorePopup : MonoBehaviour
{
    public static ScorePopup Instance;

    public Animator p1Popup, p2Popup;

    public TextMeshProUGUI p1ScoreText, p2ScoreText;

    private void Awake()
    {
        Instance = this;
    }

    public void PopupScores()
    {
        StartCoroutine(PopupScoresCoroutine());
    }

    private IEnumerator PopupScoresCoroutine()
    {
        UpdateScoreCard(p1ScoreText, ScoreUI.instance.p1RoundScore.ToString() + "/10");
        p1Popup.gameObject.SetActive(true);
        p1Popup.SetTrigger("Activate");

        yield return new WaitForSeconds(4);

        UpdateScoreCard(p2ScoreText, ScoreUI.instance.p2RoundScore.ToString() + "/10");
        p1Popup.gameObject.SetActive(false);
        p2Popup.gameObject.SetActive(true);
        p2Popup.SetTrigger("Activate");

        yield return new WaitForSeconds(3);

        p2Popup.gameObject.SetActive(false);
        ScoreUI.instance.UpdateText();
    }

    private void UpdateScoreCard(TextMeshProUGUI scoreText, string score)
    {
        scoreText.text = score;
    }
}
