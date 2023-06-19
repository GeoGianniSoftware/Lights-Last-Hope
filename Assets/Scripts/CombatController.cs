using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatController : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth;
    public int currentHealth;

    [Header("Energy")]
    public float maxEnergy;
    public float currentEnergy;
    public float energyGainRate = .15f;
    public float maxEnergyDistance = 5f;
    float energyPercentage;
    public Image energyBar;
    bool canCastComplexSpells = true;

    [Header("Combat")]
    public List<IAbility> abilities = new List<IAbility>();
    public GameObject projectile;
    ThidPersonMovement TPM;


    [Header("UI")]
    public Transform heartPanel;
    public GameObject heartPrefab;
    List<healthUIHeart> uiHearts = new List<healthUIHeart>();


    public TorchScript torchObject;
    Transform torchPos;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
        TPM = GetComponent<ThidPersonMovement>();
        torchPos = torchObject.transform.parent;
        for (int i = 0; i < maxHealth; i++) {
            uiHearts.Add(Instantiate(heartPrefab, heartPanel).GetComponent<healthUIHeart>());
        }

        //Initalize Abilities
        List<IAbility> initializedAbilities = new List<IAbility>();
        foreach(IAbility ability in abilities) {
            initializedAbilities.Add(ScriptableObject.Instantiate(ability));
        }
        abilities = initializedAbilities;
        foreach (IAbility ability in abilities) {
            ability.CASTER = this.gameObject;
        }
    }

    // Update is called once per frame
    Vector3 targetPoint;

    void Update()
    {

        

        //Health and Energy
        Health();
        Energy();
        energyPercentage = currentEnergy/maxEnergy;
        energyBar.fillAmount = energyPercentage;

        if (Vector3.SqrMagnitude(torchObject.transform.position - transform.position) <= maxEnergyDistance*maxEnergyDistance) {
            currentEnergy += energyGainRate * Time.deltaTime;
        }


        if (Input.GetKeyDown(KeyCode.P)) {
            ToggleTorch();
            
        }


        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit hit;
        // Check whether your are pointing to something so as to adjust the direction
        
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point;
        else
            targetPoint = ray.GetPoint(1000); // You may need to change this value according to your needs

        if (Input.GetMouseButtonDown(0)) {
            CastAbility(0);
        }
        if (Input.GetMouseButtonDown(1)) {
            CastAbility(1);
        }

    }

    void CastAbility(int index) {
        if (abilities[index].COMPLEX && !canCastComplexSpells)
            return;
        
        if (currentEnergy >= abilities[index].COST) {
            abilities[index].setupProjectile(TPM.transform.forward, targetPoint);
            abilities[index].Cast();
        }
    }

    int lastHealth;
    void Health() {
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        else if(currentHealth < 0) {
            currentHealth = 0;
        }

        if(lastHealth != currentHealth) {
            int heartsRemaining = currentHealth;
            for (int i = 0; i < uiHearts.Count; i++) {
                if (heartsRemaining > 0) {

                    uiHearts[i].full = true;
                    heartsRemaining--;
                }
                else {
                    uiHearts[i].full = false;
                }
            }
       }

        lastHealth = currentHealth;
    }

    void Energy() {
        if (currentEnergy > maxEnergy)
            currentEnergy = maxEnergy;
        else if (currentEnergy < 0)
            currentEnergy = 0;
        else {
            canCastComplexSpells = !torchObject.isBeingHeld;
            
        }
    }

    public void ToggleTorch() {
        if (torchObject.isBeingHeld) {
            torchObject.transform.SetParent(null);
            torchObject.isBeingHeld = false;
        }
        else if (Vector3.SqrMagnitude(torchObject.transform.position - transform.position) <= maxEnergyDistance) {
            torchObject.transform.SetParent(torchPos);

            torchObject.transform.position = torchPos.transform.position;
            torchObject.transform.rotation = Quaternion.identity;
            torchObject.isBeingHeld = true;
        }
    }

    public void TakeDamage(Damage dmg) {
        currentHealth-= dmg.amount;
        if(currentHealth > 0 && dmg.respawn) {
            TPM.controller.enabled = false;
            transform.position = TPM.lastSafeSpot;
            TPM.controller.enabled = true;
        }
    }

    public void LoseEnergy(float amt) {
        currentEnergy -= amt;
    }
    public void GainHeart(int amt) {
        maxHealth+= amt;
        for (int i = 0; i < amt; i++) {
            uiHearts.Add(Instantiate(heartPrefab, heartPanel).GetComponent<healthUIHeart>());
        }
        
    }
    public void Heal(int amt) {
        if(currentHealth < maxHealth) {
            currentHealth++;
        }
    }
}
