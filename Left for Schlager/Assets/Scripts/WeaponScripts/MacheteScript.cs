using UnityEngine;
using System.Collections;

public class MacheteScript : MonoBehaviour{

    [SerializeField]
    private float attackDamage;
    private bool attacking = false;
    private Animator animator;

    // Use this for initialization
    void Start() {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwingMachete() {
        if (attacking == false)
        {
            attacking = true;
            animator.SetBool("attacking", true);
        }
    }

    public void StopAttacking()
    {
        attacking = false;
        animator.SetBool("attacking", false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Zombie") && attacking) {
            other.gameObject.GetComponent<Zombie>().TakeDamage(attackDamage);
        }
    }
}
