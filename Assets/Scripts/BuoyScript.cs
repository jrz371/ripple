using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Is the end game object, script rotates back and forth so it looks nice
/// </summary>
public class BuoyScript : MonoBehaviour {

    /// <summary>
    /// How quickly the buoy rotates back and forth
    /// </summary>
    [SerializeField]
    public float rotationSpeed = 12;

    /// <summary>
    /// The angle in each direction the buoy moves
    /// Total angle of movement is 2x this
    /// </summary>
    [SerializeField]
    public float angleOfTravel = 10;

    /// <summary>
    /// Multiplier to flip the current direction of rotation
    /// </summary>
    private float AngleDirection = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        float CurrentAngle = transform.rotation.eulerAngles.z;

        //Clamp the angle
        if(CurrentAngle > 270)
        {
            CurrentAngle -= 360;
        }

        //make the bobbing back and forth movement
        if (Mathf.Abs(CurrentAngle) >= angleOfTravel)
        {
            AngleDirection *= -1;
            transform.Rotate(Vector3.forward, rotationSpeed * AngleDirection * Time.deltaTime);
        }

        transform.Rotate(Vector3.forward, rotationSpeed * AngleDirection * Time.deltaTime);

    }
}
