using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Inherits from ObjectScript to have the same movement
/// Extends it with the end game collisions on buoy
/// </summary>
public class BoatScript : ObjectScript {

    /// <summary>
    /// The end game text showing your score and telling you to return to menu, set in inspector
    /// </summary>
    [SerializeField]
    private Text gameFinishText;

    private bool gameOver;

    private int totalFired;

    private void Awake()
    {

    }

    // Use this for initialization
    void Start () {
        
        gameOver = false;

        totalFired = 0;

	}
	
	// Update is called once per frame
	void Update () {


	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Finish" && !gameOver)
        {
            gameOver = true;

            totalFired = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().NumberFired;

            gameFinishText.gameObject.SetActive(true);

            gameFinishText.text = "You finished in " + totalFired.ToString() + " throws \n Press Escape to Return to Menu";

            int previousScore = PlayerPrefs.GetInt("Level " + SceneManager.GetActiveScene().buildIndex, -1);

            if (totalFired < previousScore && previousScore != -1)
            {
                PlayerPrefs.SetInt("Level " + SceneManager.GetActiveScene(), totalFired);
            }

        }
    }

}
