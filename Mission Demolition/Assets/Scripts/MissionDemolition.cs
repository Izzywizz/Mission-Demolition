using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This serves as the game state manager for the game
/// </summary>

public enum GameMode
{
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S; //a privte Singleton

    [Header("Set in Inspector")]
    public Text uitLevel; // The UIText_Level Text
    public Text uitShots; //The UIText_Shots Text
    public Text uitButton; // The Text on UIButton_View
    public Vector3 castlePos; // The place to put castles
    public GameObject[] castles; // An array of the castles

    [Header("Set Dynamically")]
    public int level; //the current level
    public int levelMax; //the number of levels
    public int shotsTaken;
    public GameObject castle; //the current castle
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot"; // FollowCam mode

    // Start is called before the first frame update
    void Start()
    {
        S = this; //Singleton pattern   

        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    void StartLevel()
    {
        //Get rid of the old castle if one exisits
        if (castle != null)
        {
            Destroy(castle);
        }

        //Destroy old projectiles if they exist
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject pTemp in gos)
        {
            Destroy(pTemp);
        }

        //Instatiate the new castle, Set its position in the game world/ reset the shot coutner
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;

        //reset the camera/ clear off the projectile lines as wel
        SwitchView("Show Both");
        ProjectileLine.S.Clear();

        //Reset the goal so we haven't whacked into the green goal triangle
        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playing;
    }

    void UpdateGUI()
    {
        // Show the data in the GUITexts
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;
    }

    /// <summary>
    /// Allows the button to switch between camera modes, has a default option that grabs the text from the button
    /// </summary>
    /// <param name="eView">E view.</param>
    public void SwitchView(string eView = "")
    {
        if (eView == "")
        {
            eView = uitButton.text;
        }
        showing = eView;
        switch (showing)
        {
            case "Show Slingshot":
                FollowCam.POI = null;
                uitButton.text = "Show Castle";
                break;

            case "Show Castle":
                FollowCam.POI = S.castle;
                uitButton.text = "Show Both";
                break;

            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth");
                uitButton.text = "Show Slingshot";
                break;

            default:
                break;
        }
    }

    private void Update()
    {
        UpdateGUI();

        //Check for level end
        if ((mode == GameMode.playing) && Goal.goalMet)
        {
            //change mode to stop checking for level end
            mode = GameMode.levelEnd;
            //zoom out
            SwitchView("Show Both");
            //Start the next level in 2 seconds
            Invoke("NextLevel", 2f);
        }
    }

    void NextLevel()
    {
        level += 1;
        if (level == levelMax)
        {
            level = 0; //back to the begginning
        }
        StartLevel();
    }

    //static method that allows code anywhere to increment shotsTaken more specifically the Slingshot script
    public static void ShotFired()
    {
        S.shotsTaken += 1;
    }
}
