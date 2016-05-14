using UnityEngine;
using System.Collections;

public class BombEngin : MonoBehaviour {

    private SphereCollider collider;
    private float startTime;
    [SerializeField]
    private float timeToExplode = 4F;
    [SerializeField]
    private float hitBoxRadious = 40;
    [SerializeField]
    private float explosionDamage = 20;
    [SerializeField]
    private float explosionDuration = 0.5F;
    private float timeOffExplosion;
    private bool exploded = false;

	// Use this for initialization
	void Start () {
        collider = GetComponent<SphereCollider>();
        collider.radius = hitBoxRadious;
        collider.enabled = false;
        startTime = Time.time;
	}

	// Update is called once per frame
	void Update () {
        if ((Time.time - startTime) > timeToExplode) {
            Debug.Log("Exploding");
            collider.enabled = true;
            timeToExplode = Time.time;
            exploded = true;
        }
        if ((Time.time - timeOffExplosion) > explosionDuration && exploded) {
            Debug.Log("Destroying");
            Destroy(gameObject);
        }
	}

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Zombie")) {
            other.transform.parent.GetComponent<Zombie>().TakeDamage(explosionDamage);
        }
    }
}