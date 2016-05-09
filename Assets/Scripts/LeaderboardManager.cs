using UnityEngine;
using System.Collections;

public class LeaderboardManager : MonoBehaviour {

    public string[] lbNames = new string[3];
    public int[] lbScores = new int[3];

	// Use this for initialization
	void Start () {
        if (!PlayerPrefs.HasKey("populated"))
            populateLeaderboard();

        // retrieve leaderboard to memory
        readLeaderboard();
	}

    private void populateLeaderboard()
    {
        PlayerPrefs.SetString("name1", "mlg");
        PlayerPrefs.SetInt("score1", 10000);
        PlayerPrefs.SetString("name2", "aok");
        PlayerPrefs.SetInt("score2", 8000);
        PlayerPrefs.SetString("name3", "meh");
        PlayerPrefs.SetInt("score3", 5000);
        // don't do this again
        PlayerPrefs.SetInt("populated", 1);
    }

    public bool qualifiesForLeaderboard(int score)
    {
        // does this beat the lowest leaderboard score?
        // call this before trying to prompt for initials
        return (score > lbScores[lbScores.Length - 1]);
    }

    public void insertInLeaderboard(string name, int score)
    {
        // sanity check first
        if (!qualifiesForLeaderboard(score)) return;
        for (int i = 0; i < lbNames.Length; i++)
        {
            if (score > lbScores[i])
            {
                for (int j = lbNames.Length - 1; j > i; j--)
                {
                    lbNames[j] = lbNames[j - 1];
                    lbScores[j] = lbScores[j - 1];
                }
                lbNames[i] = name;
                lbScores[i] = score;
                break;
            }
        }
        PlayerPrefs.Save();
    }

    private void readLeaderboard()
    {
        for (int i = 0; i < lbNames.Length; i++)
        {
            lbNames[i] = PlayerPrefs.GetString("name" + (i + 1));
            lbScores[i] = PlayerPrefs.GetInt("score" + (i + 1));
        }
    }

    private void writeLeaderboard()
    {
        for (int i = 0; i < lbNames.Length; i++)
        {
            PlayerPrefs.SetString("name" + (i + 1), lbNames[i]);
            PlayerPrefs.SetInt("score" + (i + 1), lbScores[i]);
        }
        PlayerPrefs.Save();
    }
}
