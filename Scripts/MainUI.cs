using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainUI : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI elephantCounterText;

    public TextMeshProUGUI GameOverScoreText;
    public Image GameOverHighScoreBadge;

    public float Score;

    public void IncreaseScore(float x)
    {
        Score += x;

        moneyText.SetText("{0}", Score);
    }
}
