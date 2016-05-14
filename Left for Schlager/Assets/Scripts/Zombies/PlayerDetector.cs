using UnityEngine;
using System.Collections;

public class PlayerDetector : MonoBehaviour {

    private Transform playerTransform;

    private Zombie zombie;

    void Start() {
        zombie = GetComponentInParent<Zombie>();
    }


    void OnTriggerEnter(Collider colli) {
        if(colli.tag == "Player") {
            playerTransform = colli.transform;
            if(zombie.playerTransform == null) {
                zombie.playerTransform = colli.transform;
                zombie.isChasing = true;
            }
        }
    }
}
