using UnityEngine;
using System.Collections;

public class PlayerStatusScript : MonoBehaviour {

    private HealthComponent health;
    [SerializeField]
    private int rationCount;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //Called whenever a Collider object that is also a trigger is touched.
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Ration")) {
            rationCount++;
            Destroy(other.gameObject);
        }
    }
}
