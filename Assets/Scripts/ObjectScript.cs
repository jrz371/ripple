using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base script for an object on the water mesh that can slide back and forth
/// </summary>
public class ObjectScript : MonoBehaviour {

    /// <summary>
    /// The speed at which the object rotates after being pushed by a wave
    /// </summary>
    [SerializeField]
    private float rotationSpeed = 5;

    /// <summary>
    /// The max speed the object moves on top of the water
    /// </summary>
    [SerializeField]
    private float maxSpeed = 5;

    /// <summary>
    /// How quickly the object loses speed as it moves through the water
    /// </summary>
    [SerializeField]
    private float speedBleed = 4;

    [SerializeField]
    private float minSpeed = 0.25f;

    protected Rigidbody body;

    private void Awake()
    {
        body = this.GetComponent<Rigidbody>();

        if(body == null)
        {
            body = this.gameObject.AddComponent<Rigidbody>();
            Debug.LogWarning("No Rigidbody On: " + this.gameObject.name + " Adding One.");
        }

    }

    // Use this for initialization
    void Start () {
        

    }
	
	// Update is called once per frame
	void Update () {

        //Clamp the boats max speed so it doesn't race off
        if (body.velocity.magnitude > maxSpeed)
        {
            Vector3 velocity = body.velocity;
            velocity *= (maxSpeed / body.velocity.magnitude);
            body.velocity = velocity;
        }
        //slowly spin the boat to make it look nice while it moves
        else if (body.velocity.magnitude > 0)
        {
            transform.Rotate(Vector3.up, body.velocity.magnitude * rotationSpeed * Time.deltaTime);
        }

        //slowly slow down the boat so it seems like the water is dragging it
        if (body.velocity.magnitude > 0)
        {
            body.velocity -= ((body.velocity / speedBleed) * Time.deltaTime);
        }

        //Speed threshold before stopping
        if (body.velocity.magnitude < minSpeed)
        {
            body.velocity = Vector3.zero;
        }

    }
}
