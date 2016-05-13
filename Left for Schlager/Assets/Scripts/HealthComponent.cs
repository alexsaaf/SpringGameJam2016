using UnityEngine;
using System.Collections;

public class HealthComponent : MonoBehaviour {

	[SerializeField]
	private int health;
	[SerializeField]
	private int hunger;
	[SerializeField]
	private int healthFactor;
	[SerializeField]
	private int hungerFactor;
	private int maxHealth;
	private int maxHunger;

	private int Clamp(int value, int maxValue, int minValue) {
		if (value > maxValue) {
			return maxValue;
		} else if (value < minValue) {
			return minValue;
		} else {
			return value;
		}
	}

	// Use this for initialization
	void Start () {
		maxHealth = health;
		maxHunger = hunger;
	}

	public int Health {
		get{ 
			return health;
		}
		set{ 
			health = Clamp (value, maxHealth, 0);
		}
	}

	public int Hunger {
		get{ 
			return hunger;
		}
		set{ 
			hunger = Clamp (value, maxHunger, 0);
		}
	}

	public int HealthFactor {
		get{ 
			return healthFactor;
		}
		set{ 
			healthFactor = value;
		}
	}

	public int HungerFactor {
		get{ 
			return hungerFactor;
		}
		set{ 
			hungerFactor = value;
		}
	}

	public bool IsDead() {
		return (health == 0);
	}
	
	// Update is called once per frame
	void Update () {
		if (hunger > 0) {
			hunger = Clamp(hunger - hungerFactor, maxHunger, 0);
			if (health < 100) {
				hunger = hunger - hungerFactor;
				health = health + healthFactor;
			}
		}
	}
}
