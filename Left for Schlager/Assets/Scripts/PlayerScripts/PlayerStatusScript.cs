using UnityEngine;
using System.Collections;

public class PlayerStatusScript : MonoBehaviour {

    #region Variables

    // Health fields
    [SerializeField]
    private float health;
    [SerializeField]
    private float hunger;
    [SerializeField]
    private float healthFactor;
    [SerializeField]
    private float hungerFactor;
    private float maxHealth;
    private float maxHunger;
    private float time;

    // Energy fields
    [SerializeField]
    private float energy;
    [SerializeField]
    private float staticDroneDrain;
    [SerializeField]
    private float dynamicDroneDrain;
    private float maxEnergy;
    private bool droneInUse;
    private bool droneMoving;

    // Player fields
    [SerializeField]
    private static float RATION_REG = 10;
    [SerializeField]
    private static float BATTERY_REG = 10;
    private float speed;
    [SerializeField]
    private float normalSpeed;
    [SerializeField]
    private int rations;
    [SerializeField]
    private int batteries;
    [SerializeField]
    private float hungerLimit;
    [SerializeField]
    private float hungrySpeed;

    #endregion

    #region Functions

    public void Hurt(float value) {
    	health = Clamp(health - value, maxHealth, 0);
    }

    public void UseRation() {
        if (rations > 0) {
            hunger = Clamp(RATION_REG, maxHunger, 0);
            rations--;
        }
    }

    public void UseBattery() {
        if (batteries > 0) {
            energy += BATTERY_REG;
            batteries--;
        }
    }

    public void SetDroneNotInUse() {
        droneInUse = false;
        droneMoving = false;
    }

    public void SetDroneIdle() {
        droneMoving = false;
    }

    #endregion

    // Use this for initialization
    void Start() {
        maxHealth = health;
        maxHunger = hunger;
        maxEnergy = energy;
        droneInUse = false;
        droneMoving = false;
    }

    // Update is called once per frame
    void Update() {
        UpdateHungerAndHealth();
        UpdateEnergy();
        //UpdateSpeed();
        UpdateInput();
    }

    #region Update_Functions

    private void UpdateHungerAndHealth() {
        time = Time.deltaTime;
        if (hunger > 0) {
            hunger = Clamp(hunger - hungerFactor * time, maxHunger, 0);
            if (health < maxHealth) {
                hunger = hunger - hungerFactor * time;
                health = health + healthFactor * time;
            }
        }
    }

    private void UpdateEnergy() {
        if (droneInUse) {
            if (droneMoving) {
                energy -= Clamp(dynamicDroneDrain * Time.deltaTime, maxEnergy, 0);
            }
            else {
                energy -= Clamp(staticDroneDrain * Time.deltaTime, maxEnergy, 0);
            }
        }
        if (energy == 0) {
            UseBattery();
        }
    }

    private void UpdateSpeed() {
        if (hunger < hungerLimit) {
            speed = hungrySpeed;
        }
        else if (hunger >= hungerLimit && speed != normalSpeed) {
            speed = normalSpeed;
        }
    }

    private void UpdateInput() {
        if (Input.GetKeyDown(KeyCode.R)) {
            UseRation();
        } else if (Input.GetKeyDown(KeyCode.E)) {
            // INTERACT
        } else if (Input.GetKeyDown(KeyCode.V)) {
            //  CHANGE TO DRONE
            droneInUse = true;
            droneMoving = true;
            //Restart, Resume
        }
    }

    #endregion
     
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Ration")) {
            rations += 1;
            Destroy(other.gameObject);
        } else if (other.gameObject.CompareTag("Battery")) {
            batteries += 1;
            Destroy(other.gameObject);
        }
    }

    private float Clamp(float value, float maxValue, float minValue) {
        if (value > maxValue) {
            return maxValue;
        } else if (value < minValue) {
            return minValue;
        } else {
            return value;
        }
    }
}
