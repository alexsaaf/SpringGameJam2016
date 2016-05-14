using UnityEngine;
/// <summary>
/// Detects if a player is near us.
/// if it is, set the zombies playertransform and tell it to chase
/// inUse is used by different moods because only some moods will use this component
/// </summary>
public class PlayerDetector : MonoBehaviour {

    private Transform playerTransform;

    private Zombie zombie;

    public bool inUse = true;

    void Start() {
        zombie = GetComponentInParent<Zombie>();
    }


    void OnTriggerEnter(Collider colli) {
        if (inUse) {
            if (colli.tag == "Player") {
                playerTransform = colli.transform;
                if (zombie.playerTransform == null) {
                    zombie.playerTransform = playerTransform;
                    zombie.isChasing = true;
                }
            }
        }
        
    }
}
