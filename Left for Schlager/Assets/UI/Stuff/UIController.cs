using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public GameObject Player;
    private PlayerStatusScript pss;

    public GameObject uiGroup;
    public GameObject deathGroup;
    public GameObject winGroup;

    public Image healthBar;
    public Image energyBar;
    public Image hungerBar;

    public Text rationsText;
    public Text batteryText;

	void Start () {
        pss = Player.GetComponent<PlayerStatusScript>();
	}
	
	void Update () {

            healthBar.fillAmount = pss.health / pss.maxHealth;
            energyBar.fillAmount = pss.energy / pss.maxEnergy;
            hungerBar.fillAmount = pss.hunger / pss.maxHunger;

            rationsText.text = pss.rations.ToString();
            batteryText.text = pss.rations.ToString();
    }

    public void Win() {
        uiGroup.GetComponent<CanvasGroup>().alpha = 0;
        winGroup.GetComponent<CanvasGroup>().alpha = 1;
        
    }

    public void Die() {
        uiGroup.GetComponent<CanvasGroup>().alpha = 0;
        deathGroup.GetComponent<CanvasGroup>().alpha = 1;
    }
}
