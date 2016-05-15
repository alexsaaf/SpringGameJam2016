using UnityEngine;
using System.Collections;

public class BombEngin : MonoBehaviour {

    [SerializeField]
    private float timeToExplode = 4F;
    [SerializeField]
    private float hitBoxRadious = 8;
    [SerializeField]
    private float explosionDamage = 20;
    public float energyConsuption = 10F;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
        if (0 < timeToExplode) {
            timeToExplode -= Time.deltaTime;
        }
        else {
            Debug.Log("Exploding");
            Collider[] colliders = Physics.OverlapSphere(transform.position, hitBoxRadious);

            foreach (Collider c in colliders) {
                if (c && c.CompareTag("Zombie")) {
                    print(c.transform.tag);

                    c.transform.GetComponent<Zombie>().TakeDamage(explosionDamage);

                }
            }
            Destroy(gameObject);
        }
    }
}