using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossManager : MonoBehaviour
{
    public List<Enemy> bossRoomEnemies = new List<Enemy>();
    public string BossName;
    public int totalMaxHealth;
    public int currentTotalHealth = 1;
    bool defeated = false;
    public bool active = false;
    public GameObject bossUI;
    public Image bossHealthbar;
    public Image bossHealthbarDamage;
    public Text bossNameText;
    public IOOutput bossDoor;
    public IOInput locked;
    public IOOutput activateDoor;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Enemy e in bossRoomEnemies) {
            totalMaxHealth += e.maxHealth;
        }
        if(bossDoor != null && locked == null)
            bossDoor.activated = true;
    }

    // Update is called once per frame
    float damageBarTimer = 1f;
    int lastcurrentHealth;
    void Update() {

        if(activateDoor != null) {
            activateDoor.activated = defeated;
        }

        if (locked != null) {
            if (!locked.activated) {
                bossDoor.activated = false;
                foreach (Enemy e in bossRoomEnemies)
                    e.enabled = false;
            }
            else {
                foreach (Enemy e in bossRoomEnemies)
                    if (!e.enabled)
                        e.enabled = true;
            }
        }
        if(locked == null || locked.activated) {
            if (currentTotalHealth > 0) {

                int tempTotal = 0;
                foreach (Enemy e in bossRoomEnemies) {
                    if (e.currentHealth <= 0)
                        continue;

                    tempTotal += e.currentHealth;
                    if (active)
                        e.SendMessage("Agro", SendMessageOptions.DontRequireReceiver);
                    else if (e.Aggroed) {
                        active = true;
                    }
                }
                lastcurrentHealth = tempTotal;
                if (lastcurrentHealth != currentTotalHealth) {
                    damageBarTimer = 1f;
                }
                currentTotalHealth = tempTotal;

            }
            if (active) {

                if (bossDoor != null)
                    bossDoor.activated = false;
                damageBarTimer -= Time.deltaTime;
            }
            if (damageBarTimer <= 0) {
                bossHealthbarDamage.fillAmount = (float)currentTotalHealth / (float)totalMaxHealth;
            }


            if (active) {
                bossUI.SetActive(true);
                bossNameText.text = BossName;

                if (currentTotalHealth <= 0) {
                    defeated = true;
                    bossHealthbar.fillAmount = 0;
                    bossDoor.activated = true;
                    bossUI.SetActive(false);
                    this.enabled = false;
                }
                else {
                    bossHealthbar.fillAmount = Mathf.Lerp(bossHealthbar.fillAmount, (float)currentTotalHealth / (float)totalMaxHealth, .01f);
                }

            }
        }
        
    }
}
