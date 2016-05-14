using UnityEngine;
using System.Collections;

public class PlayerStatusScript : MonoBehaviour {

    #region Variables

    // Health and hunger fields
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float maxHunger;
    private float health;
    private float hunger;
    [SerializeField]
    private float healthFactor;
    [SerializeField]
    private float hungerFactor;
    private float time;

    // Energy fields
    private float energy;
    [SerializeField]
    private float maxEnergy;
    private bool noEnergyLeft;

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

    // Drone fields
    [SerializeField]
    private float staticDroneDrain;
    [SerializeField]
    private float dynamicDroneDrain;
    private bool droneInUse;
    private bool droneMoving;
    [SerializeField]
    private GameObject droneObject;
    private DroneControler drone;

    // Machete fields
    private GameObject machete;

    private bool enablePlayerInput = true;

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

    public bool UseBattery() {
        if (batteries > 0) {
            energy += BATTERY_REG;
            batteries--;
            noEnergyLeft = false;
            return true;
        }
        return false;
    }

    public void SetDroneNotInUse() {
        droneInUse = false;
        droneMoving = false;
    }

    public void SetDroneIdle() {
        droneMoving = false;
    }

    public void SetEnablePlayerInput(bool flag) {
        this.enablePlayerInput = flag;
    }

    public bool NoEnergyLeft() {
        Debug.Log("total energu" + energy);
        return noEnergyLeft;
    }

    #endregion
        
    // Use this for initialization
    void Start() {
        health = maxHealth;
        hunger = maxHunger;
        energy = maxEnergy;
        droneInUse = false;
        droneMoving = false;
        drone = droneObject.GetComponent<DroneControler>();
        machete = transform.Find("MainCamera").Find("Machete").gameObject;
    }

    // Update is called once per frame
    void Update() {
        UpdateHungerAndHealth();
        UpdateEnergy();
        //UpdateSpeed();
        if (enablePlayerInput) {
            UpdateInput();
        }
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
                energy = Clamp(energy - dynamicDroneDrain * Time.deltaTime, maxEnergy, 0);
            }
            else {
                energy = Clamp(energy - staticDroneDrain * Time.deltaTime, maxEnergy, 0);
            }
        }
        if (energy == 0) {
            if (!UseBattery()) {
                noEnergyLeft = true;
            }
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
        if (Input.GetAxisRaw("UseRation") > 0) {
            UseRation();
        }
        if (Input.GetAxisRaw("Interact") > 0) {
            // INTERACT
        }
        if (Input.GetAxisRaw("DroneToggle") > 0)
        {
            // CHANGE TO DRONE
            if (droneInUse && !droneMoving)
            {
                drone.Resume();
            }
            else
            {
                drone.Restart(transform.position + transform.forward * 3);
            }
            droneInUse = true;
            droneMoving = true;

        }
        if (Input.GetAxisRaw("PrimaryFire") > 0) {
            machete.GetComponent<MacheteScript> ().SwingMachete();
        }
    }

    #endregion
     
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Ration")) {
            rations += 1;
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Battery")) {
            batteries += 1;
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Drone")) {
            drone.PickUpDrone();
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.collider.gameObject.CompareTag("Drone")) {
            drone.PickUpDrone();
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
