using UnityEngine;
using System.Collections;

public class RayScript : MonoBehaviour {

    private Rigidbody rigidBody;
    [SerializeField]
    private float TTL;
    private float time;
    [SerializeField]
    private float movementSpeed;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        time = 0;
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;
        if (time > TTL) {
            Destroy(gameObject);
        } else {
            rigidBody.MovePosition(transform.position + transform.forward * movementSpeed* Time.deltaTime);
        }
	}

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Zombie")) {
            // Create particle effect for explosion
            Destroy(other.gameObject);
        }
    }
}
