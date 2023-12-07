using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    private TextMeshProUGUI p1ScoreText, p2ScoreText;
    public int p1Score, p2Score;
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
        if(currentPlayer)
        {
            if (currentPlayer.name == "p1")
            {
                Debug.Log("Adding P1 Score: " + DeliveryManager.Instance.lastScore);
                p1Score += DeliveryManager.Instance.lastScore;
                p1ScoreText.text = p1Score.ToString();
            }
            else if (currentPlayer.name == "p2")
            {
                p2Score += DeliveryManager.Instance.lastScore;
                p2ScoreText.text = p2Score.ToString();
            }
        }        
    }

    public string ScoreText(int score)
    {
        return score.ToString() + "/30";
    }
}
