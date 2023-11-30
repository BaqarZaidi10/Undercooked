using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    private TextMeshProUGUI p1WinsText, p1FailsText, p2WinsText, p2FailsText;
    public int p1Wins, p2Wins, p1Fails, p2Fails;
    public static ScoreUI instance;

    private void Awake()
    {
        p1WinsText = GameObject.Find("P1_Wins").GetComponent<TextMeshProUGUI>();
        p2WinsText = GameObject.Find("P2_Wins").GetComponent<TextMeshProUGUI>();
        p1FailsText = GameObject.Find("P1_Fails").GetComponent<TextMeshProUGUI>();
        p2FailsText = GameObject.Find("P2_Fails").GetComponent<TextMeshProUGUI>();

        p1Wins = 0;        
        p1WinsText.text = p1Wins.ToString();

        p1Fails = 0;
        p1FailsText.text = p1Fails.ToString();

        p2Wins = 0;
        p2WinsText.text = p2Wins.ToString();

        p2Fails = 0;
        p2FailsText.text = p2Fails.ToString();

        if(instance)
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
            if (success)
            {
                if (currentPlayer.name == "p1")
                {
                    p1Wins++;
                    p1WinsText.text = p1Wins.ToString();
                }
                else if (currentPlayer.name == "p2")
                {
                    p2Wins++;
                    p2WinsText.text = p2Wins.ToString();
                }
            }
            else
            {
                if (currentPlayer.name == "p1")
                {
                    p1Fails++;
                    p1FailsText.text = p1Fails.ToString();
                }
                else if (currentPlayer.name == "p2")
                {
                    p2Fails++;
                    p2FailsText.text = p2Fails.ToString();
                }
            }
        }        
    }
}
