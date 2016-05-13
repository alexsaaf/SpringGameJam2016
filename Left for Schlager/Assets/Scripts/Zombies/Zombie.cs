using UnityEngine;
using System.Collections;

public class Zombie : MonoBehaviour {

    public int health = 100;

    [SerializeField]
    private float viewRange = 25f;
    [SerializeField]
    private float attackRange = 5f;

    public bool isChasing = false;

    private NavMeshAgent agent;
    public Transform playerTransform;


	void Start () {
        playerTransform = null;
        agent = GetComponent<NavMeshAgent>();
	}
	
	void Update () {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitinfo;

        //Check if we can see the player
        if(Physics.Raycast(ray, out hitinfo, viewRange)) {

            if(hitinfo.collider.tag == "Player") {
                if (isChasing == false) {
                    isChasing = true;
                    if(playerTransform == null) {
                        playerTransform = hitinfo.collider.GetComponent<Transform>();
                    }
                }
            }
        }

        //Check if we can hit the player
        if (Physics.Raycast(ray, out hitinfo, attackRange)) {



        }

        //If we are chasing, set our destination
        if (isChasing) {
            agent.SetDestination(playerTransform.position);
        }
        CheckHealth();
        Debug.DrawRay(ray.origin, ray.direction * viewRange, Color.red);
	}

    public void TakeDamage(int amount) {
        health -= amount;
    }

    private void CheckHealth() {
        if(health <= 0) {
            Destroy(gameObject);
        }
    }
}
