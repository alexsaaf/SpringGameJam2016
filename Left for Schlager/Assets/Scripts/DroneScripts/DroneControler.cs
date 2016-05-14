using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class DroneControler : MonoBehaviour {

    #region Variables
    [SerializeField]
    private float moveSpeed = 5F;
    [SerializeField]
    private float hightToRise  = 30F;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float mouseSensitivity = 4F;
    private Vector3 input;
    private Vector3 inputMouse;
    private Rigidbody rb;
    private bool animationDone;
    private bool gotenStartPose;
    private float startHight;

    [SerializeField]
    private float primaryTimer = 1F;
    [SerializeField]
    private float secondaryTimer = 1F;
    [SerializeField]
    private float key1Timer = 1F;
    [SerializeField]
    private float key2Timer = 1F;
    [SerializeField]
    private float key3Timer = 1F;

    private float primaryTimerStart;
    private float secondaryTimerStart;
    private float key1TimerStart;
    private float key2TimerStart;
    private float key3TimerStart;

    private bool primaryReady = true;
    private bool secondaryReady = true;
    private bool key1Ready = true;
    private bool key2Ready = true;
    private bool key3Ready = true;

    private Camera camera;
    private Camera playerCamera;
    private UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController playerController;
    private AudioListener audioListener;
    private AudioListener playerAudioListener;

    private bool togglePressed;
    [SerializeField]
    private float pressedTime = 1F;
    #endregion

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        animationDone = false;
        gotenStartPose = false;
        ControleDrone();
        togglePressed = false;
        pressedTime = 0F;

        playerCamera = player.GetComponentInChildren<Camera>();
        camera = GetComponentInChildren<Camera>();
        playerController = player.GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>();
        audioListener = player.GetComponentInChildren<Camera>().GetComponent<AudioListener>();
        playerAudioListener = GetComponentInChildren<Camera>().GetComponent<AudioListener>();

        ControleDrone();

        primaryTimerStart = primaryTimer;
        secondaryTimerStart = secondaryTimer;
        key1TimerStart = key1Timer;
        key2TimerStart = key2Timer;
        key3TimerStart = key3Timer;
    }

    // to call when to after pickup of the drone
    private void Reset() {
        animationDone = false;
        gotenStartPose = false;
    }

    // Used to start from the begining not resuming
    public void Restart(Vector3 startPosition) {
        Reset();
        transform.position = startPosition;
        ControleDrone();
        GetComponent<Renderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
    }

    // Used to resume the control of the drone
    public void Resume() {
        ControleDrone();
        GetComponent<Renderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
    }

    //Used to pause the drone and return control to the player
    private void Pause() {
        ControlPlayer();
        // player.SetDroneIdle();
    }

    // Disable drone and return it to the user
     private void Kill() {
        ControlPlayer();
        GetComponent<Renderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        Reset();
        // player.SetDroneNotInUse();
    }

    private GameObject GetObjectInLine() {
        Ray rayDirection = new Ray(transform.position, transform.up * -1);
        RaycastHit hitInfo;
        if (Physics.Raycast(rayDirection, out hitInfo, 100F)) {
            if (hitInfo.transform.tag == "Zombie") {
                return hitInfo.collider.gameObject;
            }
            else return hitInfo.collider.gameObject;
        }
        else {
            return null;
        }
    }

    void FixedUpdate() {
        if (rb.position.y > startHight + hightToRise) {
            animationDone = true;
        }
        if (!animationDone) {
            if (gotenStartPose) {
                startHight = rb.position.y;
            }
            rb.MovePosition(transform.position + new Vector3(0, moveSpeed, 0) * Time.deltaTime);
        }
        else {
            // movement of the drone
            input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            inputMouse = new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y"))*mouseSensitivity;
            if (input.magnitude == 0) {
                rb.MovePosition(transform.position + inputMouse * moveSpeed * Time.deltaTime);
            }
            else {
                rb.MovePosition(transform.position + Vector3.ClampMagnitude(input + inputMouse, 1F) * moveSpeed * Time.deltaTime);
            }
        }
    }

    private void TickTimers() {
        float dt = Time.deltaTime;
        if (primaryTimer > 0 && !primaryReady) {
            primaryTimer -= dt;
        }
        else if (primaryTimer < 0) {
            primaryTimer = primaryTimerStart;
            primaryReady = true;
        }
        if (secondaryTimer > 0) {
            secondaryTimer -= dt;
        }
        else if (secondaryTimer < 0) {
            secondaryTimer = secondaryTimerStart;
            secondaryReady = true;
        }
        if (key1Timer > 0) {
            key1Timer -= dt;
        }
        else if (key1Timer < 0) {
            key1Timer = key1TimerStart;
            key1Ready = true;
        }
        if (key2Timer > 0) {
            key2Timer -= dt;
        }
        else if (key2Timer < 0) {
            key2Timer = key2TimerStart;
            key2Ready = true;
        }
        if (key3Timer > 0) {
            key3Timer -= dt;
        }
        else if (key3Timer < 0) {
            key3Timer = key3TimerStart;
            key3Ready = true;
        }
    }

    // Update is called once per frame
    void Update () {
        TickTimers();
        if (animationDone) {
            // Check time for when the DroneToggle is pressed
            if (Input.GetAxisRaw("DroneToggle") > 0 && !togglePressed) {
                pressedTime = Time.time;
                togglePressed = true;
                // if the time is below 1 sec go into idle else go back to user
            } if (Input.GetAxisRaw("DroneToggle") == 0 && togglePressed) {
                float pressedDeltaTime = Time.time - pressedTime;
                print("deltatime pressed " + pressedDeltaTime);
                if (pressedDeltaTime > 1F) {
                    Kill();
                }
                else {
                    Pause();
                }
            }
            // El-Chock
            if (Input.GetAxisRaw("SecondaryFire") > 0 && secondaryReady) {
                GameObject gb = GetObjectInLine();
                if (gb != null) {
                    print(gb.tag);
                }
                print("found not");
                secondaryReady = false;
            }
            // Zoom
            if (Input.GetAxisRaw("PrimaryFire") > 0 && primaryReady) {
                print("secondary fire");
                primaryReady = false;
            }
            // Moln (Emotion change)
            if (Input.GetAxisRaw("Key1") > 0 && key1Ready) {
                print("pressed Key" +1);
                key1Ready = false;
            }
            // Bomb, Invis-Cloud ...
            if (Input.GetAxisRaw("Key2") > 0 && key2Ready) {
                print("pressed Key" +2);
                key2Ready = false;
            }
            // El-Bomb (Consumes much energy)
            if (Input.GetAxisRaw("Key3") > 0 && key3Ready) {
                print("pressed Key" +3);
                key3Ready = false;
            }

        }
	}

    // Disable player and start drone control
    private void ControleDrone() {
        rb.useGravity = false;
        // Camera Enable/Disable
        if (playerCamera != null && camera != null) {
            playerCamera.enabled = false;
            camera.enabled = true;
        } else {
            Debug.LogError("Camera is null for player or the drone");
        }
        // Cotnroller Enable/Disable
        if (playerController != null) {
            playerController.enabled = false;
        }
        else {
            Debug.LogError("Controler of given player is null");
        }
        this.enabled = true;
        //AudioListener Enable/Disable
        if (playerAudioListener != null && audioListener != null) {
            playerAudioListener.enabled = false;
            audioListener.enabled = true;
        }
        else {
            Debug.LogError("Player or drone AudioListener is null");
        }
    }

    // Diable control of drone and enable player
    private void ControlPlayer() {
        // Camera Enable/Disable
        if (playerCamera != null && camera != null) {
            playerCamera.enabled = true;
        camera.enabled = false;
        } else {
            Debug.LogError("Camera is null for player or the drone");
        }
        // Cotnroller Enable/Disable
        if (playerController != null) {
            playerController.enabled = true;
        }
        else {
            Debug.LogError("Controler of given player is null");
        }
        this.enabled = false;
        //AudioListener Enable/Disable
        if (playerAudioListener != null && audioListener != null) {
            playerAudioListener.enabled = true;
            audioListener.enabled = false;
        }
        else {
            Debug.LogError("Player or drone AudioListener is null");
        }
    }
}
