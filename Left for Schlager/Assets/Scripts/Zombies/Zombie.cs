using UnityEngine;
/// <summary>
/// Main zombie class
/// Looks for the player and chases him if he is found
/// </summary>
public class Zombie : MonoBehaviour {

    #region Variable declarations
    [SerializeField]
    private float health;

    [Header("Mood-profiles")]
    public MoodProfile angryProfile = new MoodProfile();
    public MoodProfile happyProfile = new MoodProfile();
    public MoodProfile sadProfile = new MoodProfile();
    private MoodProfile currentMoodProfile;

    [Header("Mood")]
    public States mood;
    public enum States {Happy, Sad, Angry};
    public GameObject[] eyes;

    [Header("Range")]
    [SerializeField]
    private float viewRange = 25f;
    [SerializeField]
    private float attackRange = 5f;

    [Header("Movement")]
    private float thinkTimer = 5f;
    private float thinkTimerStart;
    private float randomUnitCircleRadius = 10f;
    private bool isAggressive;

    public bool isChasing = false;

    [SerializeField]
    private float damage;

    private PlayerDetector playerDetector;
    private NavMeshAgent agent;
    public Transform playerTransform;
    #endregion

    void Start () {
        playerTransform = null;
        agent = GetComponent<NavMeshAgent>();
        thinkTimerStart = thinkTimer;   //Remember the original timer value, as we will use thinktimer to count time
        playerDetector = GetComponentInChildren<PlayerDetector>();
        SetUpMood();
        
	}

    #region moodSetups
    private void SetUpMood() {
        //Check the mood we are set to and update the currentMoodProfile accordingly
        switch (mood) {
            case States.Angry:
                currentMoodProfile = angryProfile;
                break;
            case States.Happy:
                currentMoodProfile = happyProfile;
                break;
            case States.Sad:
                currentMoodProfile = sadProfile;
                break;
            default:
                currentMoodProfile = happyProfile;
                break;
        }

        //Update all variables which are decided by our currentMoodProfile
        playerDetector.inUse = currentMoodProfile.playerDetectorEnabled;
        UpdateEyeColor();
        thinkTimerStart = currentMoodProfile.thinkTime;
        randomUnitCircleRadius = currentMoodProfile.strayDistance;
        agent.speed = currentMoodProfile.runSpeed;
        isAggressive = currentMoodProfile.isAggressive;
        //If we are no longer aggressive, forget the player
        if (!isAggressive) {
            playerTransform = null;
        }
            
    }

    //Update the color of our eyes according to our mood
    private void UpdateEyeColor() {
        for (int i = 0; i < eyes.Length; i++) {
            eyes[i].GetComponent<MeshRenderer>().material.color = currentMoodProfile.eyeColor;
        }
    }

    #endregion

    void Update () {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitinfo;

        //Count down the timer and think if the timer has reached 0
        thinkTimer -= Time.deltaTime;
        if(thinkTimer < 0) {
            Think();
            thinkTimer = thinkTimerStart;
        }

        if (isAggressive) {
            //Check if we can see the player
            if (Physics.Raycast(ray, out hitinfo, viewRange)) {
                //If the target is the player, start chasing him and save his transform
                if (hitinfo.collider.tag == "Player") {
                    if (isChasing == false) {
                        isChasing = true;
                        if (playerTransform == null) {          //If we dont have the player transform, save it
                            playerTransform = hitinfo.collider.GetComponent<Transform>();
                        }
                    }
                }
            }

            //Check if we can hit the player
            if (Physics.Raycast(ray, out hitinfo, attackRange)) {
                if(hitinfo.collider.tag == "Player") {
                    agent.Stop();           //If we are in range to hit, dont keep chasing
                    playerTransform.gameObject.GetComponent<PlayerStatusScript>().TakeDamage(damage);
                } else {
                    Chase();        //Not the plyer, we can chase
                }
            } else {
                Chase();        //Nothing hit, keep running forest
            }

            //If we are chasing, set our destination to the player
            if (isChasing) {
                float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
                if (distanceToPlayer > currentMoodProfile.giveUpDistance) {
                    isChasing = false;
                    playerTransform = null;
                }
            }
        }
        
        CheckHealth();  //Check if we are done
        //Debug ray, NOT TO BE IN FINAL
        Debug.DrawRay(ray.origin, ray.direction * viewRange, Color.red);
	}

    //Tells the agent to chase if we are in chasing-phase
    private void Chase() {
        if (isChasing) {
            agent.Resume();
            agent.SetDestination(playerTransform.position);
        } 
    }

    //Take the given damage
    public void TakeDamage(float amount) {
        Debug.Log("Took DMG: " + amount);
        health -= amount;
        Debug.Log("TAKE DAMAGE, now: " + health.ToString());
        if(health <= 0)
        {
            Debug.Log("Is destroyed");
            Destroy(this.gameObject);
        }
    }

    //Check if we are dead lel
    private void CheckHealth() {
        if(health <= 0) {
            Destroy(gameObject);
        }
    }

    //Think and move to a random position if we are not chasing
    private void Think() {
        if (!isChasing) {
            //Get a random position in a circle around us
            Vector3 newPos = transform.position + new Vector3(Random.insideUnitCircle.x * randomUnitCircleRadius, transform.position.y, Random.insideUnitCircle.y * randomUnitCircleRadius);
            agent.SetDestination(newPos);
        }
    }
}
