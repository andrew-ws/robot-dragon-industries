using UnityEngine;
using System.Collections;

public class LeaderboardManager : MonoBehaviour {

    public string[] lbNames = new string[3];
    public int[] lbScores = new int[3];

    string inits = "AAA";

    bool submitted = false;

    LevelManager lm;

    // Use this for initialization
    void Start () {
        if (!PlayerPrefs.HasKey(lm.level + "populated"))
            populateLeaderboard();

        // retrieve leaderboard to memory
        readLeaderboard();
	}

    public void init(LevelManager lm)
    {
        this.lm = lm;
    }

    private void populateLeaderboard()
    {
        PlayerPrefs.SetString(lm.level + "name1", "MLG");
        PlayerPrefs.SetInt(lm.level + "score1", 5000);
        PlayerPrefs.SetString(lm.level + "name2", "AOK");
        PlayerPrefs.SetInt(lm.level + "score2", 3000);
        PlayerPrefs.SetString(lm.level + "name3", "MEH");
        PlayerPrefs.SetInt(lm.level + "score3", 1000);
        // don't do this again
        PlayerPrefs.SetInt(lm.level + "populated", 1);
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
        writeLeaderboard();
    }

    private void readLeaderboard()
    {
        for (int i = 0; i < lbNames.Length; i++)
        {
            lbNames[i] = PlayerPrefs.GetString(lm.level + "name" + (i + 1));
            lbScores[i] = PlayerPrefs.GetInt(lm.level + "score" + (i + 1));
        }
    }

    private void writeLeaderboard()
    {
        for (int i = 0; i < lbNames.Length; i++)
        {
            PlayerPrefs.SetString(lm.level + "name" + (i + 1), lbNames[i]);
            PlayerPrefs.SetInt(lm.level + "score" + (i + 1), lbScores[i]);
        }
        PlayerPrefs.Save();
    }

    public void draw()
    {
        string lbstring = "Leaderboard!!!\n";
        for (int i = 0; i < lbNames.Length; i++)
        {
            lbstring += (i + 1) + ") " + lbNames[i] + ": " + lbScores[i]
                + "\n";
        }
        GUIStyle lbStyle = GUI.skin.label;
        lbStyle.fontSize = 26;
        lbStyle.normal.textColor = Color.white;
        GUIStyle initsStyle = GUI.skin.textField;
        initsStyle.fontSize = 26;
        initsStyle.alignment = TextAnchor.MiddleCenter;
        GUI.Label(new Rect(100, 100, 400, 400), lbstring);
        if (qualifiesForLeaderboard(lm.totalMoney) && !submitted)
        {
            GUI.Label(new Rect(Screen.width - 200, 50, 200, 50), "High score!");
            inits = GUI.TextField(new Rect(Screen.width - 200, 100,
                100, 50), inits, 3).ToUpper();
            if (GUI.Button(new Rect(Screen.width - 200, 175, 100, 50), "Submit!!")
                && !inits.Equals(""))
            {
                insertInLeaderboard(inits, lm.totalMoney);
                submitted = true;
            }
        }
    }
}
