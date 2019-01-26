using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Moves the first person player and throws rocks
/// </summary>
public class PlayerScript : MonoBehaviour {

    /// <summary>
    /// The camera object set in inspector so it can be rotated 
    /// </summary>
    [SerializeField]
    private GameObject cameraObject;

    /// <summary>
    /// Prefab for the Rock set in inspector
    /// </summary>
    [SerializeField]
    private GameObject rockObject;

    /// <summary>
    /// Spawn Location of the thrown rock set in inspector, should be a sub object of player in front of camera
    /// </summary>
    [SerializeField]
    private GameObject rockSpawn;

    /// <summary>
    /// Text to be updated for each throw that shows score
    /// </summary>
    [SerializeField]
    private Text throwText;

    /// <summary>
    /// Speed at which player can move
    /// </summary>
    [SerializeField]
    private float moveSpeed = 0;

    /// <summary>
    /// Mouse sensitivity for the player
    /// </summary>
    [SerializeField]
    private float MouseSpeed = 6;

    /// <summary>
    /// Slider reference set in inspector that shows the power of the throw of the player
    /// </summary>
    [SerializeField]
    private Slider PowerSlider;

    /// <summary>
    /// Maximum Power of the throw
    /// </summary>
    [SerializeField]
    private float MaxPower = 5;

    /// <summary>
    /// Minimum Power of the throw
    /// </summary>
    [SerializeField]
    private float MinPower = 2;

    private CharacterController characterController;

    private int numberFired;
    public int NumberFired
    {
        get
        {
            return numberFired;
        }
    }

    private void Awake()
    {
        characterController = this.GetComponent<CharacterController>();
    }

    // Use this for initialization
    void Start () {

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        PowerSlider.maxValue = MaxPower;
        PowerSlider.minValue = MinPower;

        numberFired = 0;
	}

	// Update is called once per frame
	void Update () {

        //--------------MOVEMENT-----------------

        float HorizontalMove = Input.GetAxis("Horizontal") * Time.deltaTime;
        float VerticalMove = Input.GetAxis("Vertical") * Time.deltaTime;

        Vector3 MoveVector = ((transform.forward * VerticalMove) + (transform.right * HorizontalMove)) * moveSpeed;

        float HorizontalLook = Input.GetAxis("Mouse X") * Time.deltaTime * MouseSpeed;
        float VerticalLook = Input.GetAxis("Mouse Y") * Time.deltaTime * MouseSpeed;

        transform.Rotate(Vector3.up, HorizontalLook);
        cameraObject.transform.Rotate(Vector3.right, -VerticalLook);

        characterController.Move(MoveVector);

        //--------------THROWING ROCKS-----------------

        float MouseWheelMove = Input.GetAxis("Mouse ScrollWheel");

        PowerSlider.value += MouseWheelMove;

        if (Input.GetButtonDown("Fire1"))
        {
            GameObject newRock = Instantiate(rockObject);

            Rigidbody newRockRigidbody = newRock.GetComponent<Rigidbody>();

            newRock.transform.position = rockSpawn.transform.position;
            newRock.transform.rotation = rockSpawn.transform.rotation;

            newRockRigidbody.AddForce(newRock.transform.forward * PowerSlider.value, ForceMode.Impulse);
            newRockRigidbody.AddTorque(Random.insideUnitSphere, ForceMode.Impulse);

            numberFired += 1;

            throwText.text = "Rocks Thrown: " + numberFired.ToString();
        }

        //return to menu if escape is pressed
        if(Input.GetButtonDown("Cancel"))
        {
            SceneManager.LoadScene(0);
        }

	}
}
