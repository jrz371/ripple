using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Drop manager handles the waves and pushing objects
/// </summary>
public class DropManager : MonoBehaviour {

    /// <summary>
    /// Private class that acts as a storage container for all the wave information
    /// </summary>
    private class Wave
    {
        /// <summary>
        /// Start positions on vertex grid of wave
        /// </summary>
        public int StartX, StartY;

        /// <summary>
        /// Current distance of wave from center, radius of circle outward
        /// </summary>
        public float CurrentDistance;

        /// <summary>
        /// The max distance of travel of the wave
        /// </summary>
        public float MaxDistance;

        /// <summary>
        /// How fast the wave spreads outward to its max distance
        /// </summary>
        public float Speed;

        /// <summary>
        /// If the wave is done or not, if the wave is done it can be reused in the pool
        /// </summary>
        public bool Done;

        /// <summary>
        /// Waves only push the boat once so this is the flag for if it has done it or not
        /// </summary>
        public bool CanPush;

        /// <summary>
        /// The power of the wave when it pushes the boat
        /// </summary>
        public float Power;
    }

    /// <summary>
    /// The number of initial waves in the pool
    /// </summary>
    [SerializeField]
    private int startWaves = 20;

    /// <summary>
    /// The max distance a wave can travel before dying
    /// </summary>
    [SerializeField]
    private float startMaxDistance = 250.0f; 

    private GameObject boat;
    private Rigidbody boatRigidbody;

    /// <summary>
    /// The wave pool
    /// </summary>
    private List<Wave> Waves = new List<Wave>();

    private Water waterScript;
    private BoxCollider coll;

    private List<GameObject> Objects = new List<GameObject>();
    private List<Rigidbody> ObjectRigidbodies = new List<Rigidbody>();

    private void Awake()
    {
        waterScript = this.GetComponent<Water>();

        coll = this.GetComponent<BoxCollider>();

        coll.size = new Vector3(waterScript.widthQuads * waterScript.quadSize, 0.2f, waterScript.heightQuads * waterScript.quadSize);

    }

    // Use this for initialization
    void Start () {

        boat = GameObject.Find("Boat");
        boatRigidbody = boat.GetComponent<Rigidbody>();

        for(int i = 0; i < startWaves; i++)
        {
            Waves.Add(new Wave());
            Waves[i].MaxDistance = startMaxDistance;
            Waves[i].Speed = 1;
            Waves[i].Done = true;
            Waves[i].CanPush = true;
            Waves[i].Power = 1;
        }

        //objects are another other thing on the water (lilly pads) that can be pushed by the waves
        Objects.AddRange(GameObject.FindGameObjectsWithTag("Object"));

        for(int x = 0; x < Objects.Count; x++)
        {
            ObjectRigidbodies.Add(Objects[x].GetComponent<Rigidbody>());
        }

	}
	
	// Update is called once per frame
	void Update ()
    {
        
    }

    private void FixedUpdate()
    {
        //find the position of the boat on the mesh for pushing later
        Vector3 BoatRelativePosition = transform.InverseTransformDirection(boat.transform.position);
        BoatRelativePosition += waterScript.MeshCenter;
        BoatRelativePosition /= waterScript.quadSize;

        //flatten the mesh so we can add the waves
        waterScript.FlattenMesh();

        for (int x = 0; x < Waves.Count; x++)
        {
            if (Waves[x].Done == false)
            {
                Waves[x].CurrentDistance += Waves[x].Speed * Time.deltaTime / waterScript.quadSize;

                //lay the circles for the peak and valley of the wave
                //approximately a single period of the sine function
                waterScript.LayCircle(Waves[x].StartX, Waves[x].StartY, (int)Waves[x].CurrentDistance + 1, 0.25f);
                waterScript.LayCircle(Waves[x].StartX, Waves[x].StartY, (int)Waves[x].CurrentDistance + 2, 0.5f);
                waterScript.LayCircle(Waves[x].StartX, Waves[x].StartY, (int)Waves[x].CurrentDistance + 3, 0.25f);
                waterScript.LayCircle(Waves[x].StartX, Waves[x].StartY, (int)Waves[x].CurrentDistance - 1, -0.25f);
                waterScript.LayCircle(Waves[x].StartX, Waves[x].StartY, (int)Waves[x].CurrentDistance - 2, -0.5f);
                waterScript.LayCircle(Waves[x].StartX, Waves[x].StartY, (int)Waves[x].CurrentDistance - 3, -0.25f);

                //cull the wave if its done
                if (Waves[x].CurrentDistance >= Waves[x].MaxDistance)
                {
                    Waves[x].Done = true;
                }
                else
                {
                    //check if it reached the boat and if so push it
                    if (Vector2.Distance(new Vector2(Waves[x].StartX, Waves[x].StartY), new Vector2(BoatRelativePosition.x, BoatRelativePosition.z)) - Waves[x].CurrentDistance < 0.5 && Waves[x].CanPush)
                    {
                        Waves[x].CanPush = false;
                        float DistZ = BoatRelativePosition.z - Waves[x].StartY;
                        float DistX = BoatRelativePosition.x - Waves[x].StartX;

                        float ForceX = (DistX / (Mathf.Abs(DistX) + Mathf.Abs(DistZ))) * Waves[x].Speed;
                        float ForceZ = (DistZ / (Mathf.Abs(DistX) + Mathf.Abs(DistX))) * Waves[x].Speed;

                        boatRigidbody.AddForce(ForceX, 0, ForceZ, ForceMode.Impulse);
                    }

                    //same fot the objects
                    for (int i = 0; i < Objects.Count; i++)
                    {
                        Vector3 ObjectRelativePosition = transform.InverseTransformDirection(Objects[i].transform.position);
                        ObjectRelativePosition += waterScript.MeshCenter;
                        ObjectRelativePosition /= waterScript.quadSize;

                        if (Mathf.Abs(Vector2.Distance(new Vector2(Waves[x].StartX, Waves[x].StartY), new Vector2(ObjectRelativePosition.x, ObjectRelativePosition.z)) - Waves[x].CurrentDistance) < 0.5 && ObjectRigidbodies[i].velocity.magnitude == 0)
                        {
                            float DistZ = ObjectRelativePosition.z - Waves[x].StartY;
                            float DistX = ObjectRelativePosition.x - Waves[x].StartX;

                            float ForceX = (DistX / (Mathf.Abs(DistX) + Mathf.Abs(DistZ))) * (Waves[x].Speed / 2);
                            float ForceZ = (DistZ / (Mathf.Abs(DistX) + Mathf.Abs(DistX))) * (Waves[x].Speed / 2);

                            ObjectRigidbodies[i].AddForce(ForceX, 0, ForceZ, ForceMode.Impulse);

                        }
                    }

                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if a new rock hits us
        if(collision.collider.tag == "Rock")
        {

            Vector3 TransformPoint = transform.InverseTransformPoint(collision.contacts[0].point);
            TransformPoint += waterScript.MeshCenter;

            TransformPoint /= waterScript.quadSize;

            //try to find a wave
            int waveIndex = -1;

            for (int i = 0; i < Waves.Count; i++)
            {
                if (Waves[i].Done == true)
                {
                    waveIndex = i;
                    break;
                }
            }

            //if we can't find and unused wave add a new one
            if(waveIndex == -1)
            {
                Waves.Add(new Wave());
                waveIndex = Waves.Count - 1;
            }

            //reset the wave information
            Waves[waveIndex].Done = false;
            Waves[waveIndex].MaxDistance = collision.collider.gameObject.GetComponent<Rock>().MaxDistance;
            Waves[waveIndex].Speed = 3;
            Waves[waveIndex].StartX = (int)(TransformPoint.x);
            Waves[waveIndex].StartY = (int)(TransformPoint.z);
            Waves[waveIndex].CurrentDistance = 0;
            Waves[waveIndex].CanPush = true;
            Waves[waveIndex].Power = 5;
        }

    }

}
