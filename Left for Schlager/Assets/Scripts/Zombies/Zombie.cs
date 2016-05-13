using UnityEngine;
using System.Collections;

public class Zombie : MonoBehaviour {

    public int health = 100;

    public States state;
    public enum States { Happy, Sad, Mad};


    [SerializeField]
    private float viewRange = 25f;
    [SerializeField]
    private float attackRange = 5f;

    [SerializeField]
    private float thinkTimer = 5f;
    private float thinkTimerStart;

    public float randomUnitCircleRadius = 10f;

    public bool isChasing = false;

    private NavMeshAgent agent;
    public Transform playerTransform;


	void Start () {
        playerTransform = null;
        agent = GetComponent<NavMeshAgent>();
        thinkTimerStart = thinkTimer;
	}
	
	void Update () {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitinfo;

        thinkTimer -= Time.deltaTime;
        if(thinkTimer < 0) {
            Think();
            thinkTimer = thinkTimerStart;
        }

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

    private void Think() {
        if (!isChasing) {
            Vector3 newPos = transform.position + new Vector3(Random.insideUnitCircle.x * randomUnitCircleRadius, transform.position.y, Random.insideUnitCircle.y * randomUnitCircleRadius);
            agent.SetDestination(newPos);
        }
    }
}
