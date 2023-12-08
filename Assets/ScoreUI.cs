using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    private TextMeshProUGUI p1ScoreText, p2ScoreText;
    public int p1Score, p2Score;
    public int p1RoundScore, p2RoundScore;
    public static ScoreUI instance;

    private void Awake()
    {
        p1ScoreText = GameObject.Find("P1_Score").GetComponent<TextMeshProUGUI>();
        p2ScoreText = GameObject.Find("P2_Score").GetComponent<TextMeshProUGUI>();

        p1Score = 0;
        p1ScoreText.text = ScoreText(p1Score);

        p2Score = 0;
        p2ScoreText.text = ScoreText(p2Score);

        if (instance)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void OnEnable()
    {
        DeliveryResultUI.delivered += Delivered;
    }

    private void OnDisable()
    {
        DeliveryResultUI.delivered -= Delivered;
    }

    private void Delivered(GameObject currentPlayer, bool success)
    {
        Debug.Log("Test");
        Debug.Log(currentPlayer.name);

        if(currentPlayer)
        {
            if (currentPlayer.name == "p1")
            {
                p1Score += DeliveryManager.Instance.lastScore;
            }
            else if (currentPlayer.name == "p2")
            {
                p2Score += DeliveryManager.Instance.lastScore;
            }
        }        
    }

    public void UpdateText()
    {
        p1ScoreText.text = ScoreText(p1Score);
        p2ScoreText.text = ScoreText(p2Score);
    }

    public string ScoreText(int score)
    {
        return score.ToString() + "/30";
    }
}
