using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Holds callbacks for button events and updates the UI
/// </summary>
public class MenuScript : MonoBehaviour {

    /// <summary>
    /// Text for the level 1 high score. Set in inspector
    /// </summary>
    [SerializeField]
    private Text levelOneText;

    /// <summary>
    /// Text for the level 2 high score. Set in inspector
    /// </summary>
    [SerializeField]
    private Text levelTwoText;

    /// <summary>
    /// Text for the level 3 high score. Set in inspector
    /// </summary>
    [SerializeField]
    private Text levelThreeText;

    /// <summary>
    /// Level one high score pulled from player prefs
    /// </summary>
    private int levelOneBest;

    /// <summary>
    ///  Level two high score pulled from player prefs
    /// </summary>
    private int levelTwoBest;

    /// <summary>
    /// Level three high score pulled from player prefs
    /// </summary>
    private int levelThreeBest;

	// Use this for initialization
	void Start () {

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        levelOneBest = PlayerPrefs.GetInt("Level 1", -1);
        levelTwoBest = PlayerPrefs.GetInt("Level 2", -1);
        levelThreeBest = PlayerPrefs.GetInt("Level 3", -1);

        if(levelOneBest != -1)
        {
            levelOneText.text = "Best: " + levelOneBest;
        }
        else
        {
            levelOneText.text = "No Score Yet";
        }

        if (levelTwoBest != -1)
        {
            levelTwoText.text = "Best: " + levelTwoBest;
        }
        else
        {
            levelTwoText.text = "No Score Yet";
        }

        if (levelThreeBest != -1)
        {
            levelThreeText.text = "Best: " + levelThreeBest;
        }
        else
        {
            levelThreeText.text = "No Score Yet";
        }

    }

    /// <summary>
    /// Loads level, called by the UnityEvent on the Level Select Buttons
    /// </summary>
    /// <param name="levelIndex"></param>
    public void MoveLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
    }

    /// <summary>
    /// Called by the UnityEvent for the Exit button
    /// </summary>
    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}