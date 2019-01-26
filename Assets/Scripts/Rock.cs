using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object that hits the water and causes a wave
/// </summary>
public class Rock : MonoBehaviour {

    SphereCollider sphereCollider;

    /// <summary>
    /// Sets the max distance of the wave when its thrown
    /// </summary>
    [SerializeField]
    private int maxDistance = 250;
    public int MaxDistance
    {
        get
        {
            return maxDistance;
        }
    }

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();

        if(sphereCollider == null)
        {
            sphereCollider = this.gameObject.AddComponent<SphereCollider>();
            Debug.LogWarning("No Sphere Collidor On Rock: " + this.gameObject.name + " Adding One.");
        }

    }

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
        if(transform.position.y < -10.0f)
        {
            Destroy(this.gameObject);
        }

	}

    private void OnCollisionEnter(Collision collision)
    {
        //if we hit the water switch to trigger so we don't collide and make two waves
        if(collision.collider.tag == "Water")
        {
            sphereCollider.isTrigger = true;
        }
    }
}
