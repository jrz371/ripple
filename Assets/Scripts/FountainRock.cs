using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The fountain script that continually drops a rock object to create repetitive waves
/// </summary>
public class FountainRock : MonoBehaviour {

    /// <summary>
    /// The height the object sdrops from to create the waves around the fountain
    /// </summary>
    [SerializeField]
    private float dropHeight = 6.0f;

    Rigidbody rig;
    SphereCollider coll;

	// Use this for initialization
	void Start () {
        rig = GetComponent<Rigidbody>();
        coll = GetComponent<SphereCollider>();
	}
	
	// Update is called once per frame
	void Update () {
		
        if(coll.isTrigger == true)
        {
            coll.isTrigger = false;
            transform.Translate(0f, dropHeight, 0f);
            rig.velocity = Vector3.zero;
            rig.angularVelocity = Vector3.zero;
            transform.rotation = Quaternion.identity;
        }

	}

}
