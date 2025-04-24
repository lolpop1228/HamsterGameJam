using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public TextMeshProUGUI scoreText;
    public int maxScore = 10; // Set your max score here
    private int score = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        UpdateScoreUI(); // Show score at the start
    }

    public void AddScore(int amount)
    {
        if (score < maxScore)
        {
            score += amount;

            if (score > maxScore)
                score = maxScore;

            UpdateScoreUI();
        }
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Read Clues: " + score + "/" + maxScore;
    }

    public bool HasReachedMaxScore()
    {
        return score >= maxScore;
    }
}
