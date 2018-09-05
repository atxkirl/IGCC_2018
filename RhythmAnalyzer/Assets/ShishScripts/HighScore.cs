using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScore : MonoBehaviour {

    private Vector2 scrollPos = new Vector2(300, 300);
    // Use this for initialization
    void Start () {


        //Debug.Log("Posting Score To Leaderboard...");
        //new GameSparks.Api.Requests.LogEventRequest()
        //    .SetEventKey("SUBMIT_SCORE")
        //    .SetEventAttribute("SCORE", 50)
        //    .Send((response) =>
        //    {
        //        if (!response.HasErrors)
        //        {
        //            Debug.Log("Score Posted Sucessfully...");
        //        }
        //        else
        //        {
        //            Debug.Log("Error Posting Score...");
        //        }
        //    });

        //new GameSparks.Api.Requests.LeaderboardDataRequest()
        //   .SetLeaderboardShortCode("SCORE_LEADERBOARD")
        //   .SetEntryCount(int.Parse(entryCount)) // we need to parse this text input, since the entry count only takes long
        //   .Send((response) =>
        //   {

        //       if (!response.HasErrors)
        //       {
        //           Debug.Log("Found Leaderboard Data...");
        //           outputData = System.String.Empty; // first clear all the data from the output
        //            foreach (GameSparks.Api.Responses.LeaderboardDataResponse._LeaderboardData entry in response.Data) // iterate through the leaderboard data
        //            {
        //               int rank = (int)entry.Rank; // we can get the rank directly
        //                string playerName = entry.UserName;
        //               string score = entry.JSONData["SCORE"].ToString(); // we need to get the key, in order to get the score
        //                outputData += rank + "   Name: " + playerName + "        Score:" + score + "\n"; // addd the score to the output text
        //            }
        //       }
        //       else
        //       {
        //           Debug.Log("Error Retrieving Leaderboard Data...");
        //       }

        //   });

    }

    private void OnGUI()
    {
        scrollPos.x = 300;
        scrollPos.y = 300;
        //GUILayout.BeginArea(new Rect((Screen.width - 400) / 2, (Screen.height - 300) / 2, 400, 300));
        GUILayout.Label("LEADERBOARD");
        GUILayout.BeginScrollView(scrollPos, GUILayout.Width(100), GUILayout.Height(100));
        GUILayout.Label("I feel so unsure As I take your hand and lead you to the dance floor As the music dies, something in your eyes Calls to mind the silver screen And all its sad good - byes I'm never gonna dance again Guilty feet have got no rhythm Though it's easy to pretend I know you're not a fool Should've known better than to cheat a friend And waste the chance that I've been given So I'm never gonna dance again The way I danced with you Time can never mend The careless whispers of a good friend To the heart and mind Ignorance is kind There's no comfort in the truth Pain is all you'll find I'm never gonna dance again Guilty feet have got no rhythm Though it's easy to pretend I know you're not a fool I should've known better than to cheat a friend And waste the chance that I've been given So I'm never gonna dance again The way I danced with you Never without your love Tonight the music seems so loud I wish that we could lose this crowd Maybe it's better this way We'd hurt each other with the things we'd want to say We could have been so good together We could have lived this dance forever But no one's gonna dance with me Please stay And I'm never gonna dance again Guilty feet have got no rhythm Though it's easy to pretend I know you're not a fool Should've known better than to cheat a friend And waste the chance that I've been given So I'm never gonna dance again The way I danced with you Now that you're gone (Now that you're gone) What I did's so wrong, so wrong That you had to leave me alone");
       // GUI.Box(new Rect(0, 0, 400, 100),);
        GUILayout.EndVertical();
    }
    // Update is called once per frame
    void Update () {
		
	}
}
