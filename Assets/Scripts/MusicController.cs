using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Plays the background music on a static game object
/// </summary>
public class MusicController : MonoBehaviour {

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = this.GetComponent<AudioSource>();

        if(audioSource == null)
        {
            audioSource = this.gameObject.AddComponent<AudioSource>();
        }

    }

    // Use this for initialization
    void Start () {
        //music plays throughout the entire game so set this to not be destroyed
        DontDestroyOnLoad(this.gameObject);

	}

    // Update is called once per frame
    void Update() {
        //Space mutes and unmutes music
        if (Input.GetButtonDown("Jump"))
        {
            audioSource.mute = !audioSource.mute;
        }

	}
}
