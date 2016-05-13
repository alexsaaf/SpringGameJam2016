using UnityEngine;
using System.Collections;

public class DroneControler : MonoBehaviour {
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float hightToRise;
    [SerializeField]
    private GameObject player;
    private Vector3 input;
    private Rigidbody rb;
    private bool animationDone;
    private bool gotenStartPose;
    private float startHight;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        animationDone = false;
        gotenStartPose = false;
        player.GetComponentInChildren<Camera>().enabled = false;
        player.GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>().enabled = false;
        GetComponentInChildren<Camera>().enabled = true;

    }

    // to call when to after pickup of the drone
    void Reset() {
        animationDone = false;
        gotenStartPose = false;
    }

    void Restart(Vector3 startPosition) {
        Reset();
        transform.position = startPosition;
        ControleDrone();
    }

    // Update is called once per frame
    void Update () {
        if (rb.position.y > startHight + hightToRise) {
            animationDone = true;
        }
        if (!animationDone)
        {
            if (gotenStartPose)
            {
                startHight = rb.position.y;
            }
            rb.MovePosition(transform.position + new Vector3(0, moveSpeed, 0) * Time.deltaTime);
        }
        else {
            input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            rb.MovePosition(transform.position + input * moveSpeed * Time.deltaTime);
            if (Input.GetAxis("Fire2") > 0)
            {
                ControlPlayer();
            }
        }
	}

    private void ControleDrone() {
        rb.useGravity = false;
        player.GetComponentInChildren<Camera>().enabled = false;
        GetComponentInChildren<Camera>().enabled = true;
        player.GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>().enabled = false;
        GetComponent<DroneControler>().enabled = true;
    }

    private void ControlPlayer() {
        rb.useGravity = true;
        player.GetComponentInChildren<Camera>().enabled = true;
        GetComponentInChildren<Camera>().enabled = false;
        player.GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>().enabled = true;
        GetComponent<DroneControler>().enabled = false;
    }
}
