using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoblinMageController : Enemy
{
    

    public Transform target;
    NavMeshAgent NMA;
    Animator ANIM;
    [Header("Health")]


    [Header("Combat")]
    public float speed;
    public float agroRange = 5f;
    public float engagementRange = 15;
    public float backupRange = 10f;

    Vector3 movePos;
    public bool backingUp;
    public float attackCooldown;

    [Header("Weapons")]

    float cooldownTimer;
    float resetTimer;
    bool attacking;
    public List<IAbility> possibleAbilities = new List<IAbility>();
    public IAbility mageAbility;
    public Transform spellPos;
    public Collider kickCollider;
    public Damage kickDamage;

    public List<Collider> ragdoll = new List<Collider>();

    float backupTimer;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        NMA = GetComponent<NavMeshAgent>();
        ANIM = GetComponent<Animator>();
        NMA.speed = speed;
        target = FindObjectOfType<ThidPersonMovement>().transform;
        if(possibleAbilities.Count > 0)
        mageAbility = possibleAbilities[Random.Range(0, possibleAbilities.Count)];

        kickCollider.GetComponent<EnemyWeapon>().dmg = kickDamage;

        mageAbility = IAbility.Instantiate(mageAbility);
        mageAbility.CASTER = this.gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        ANIM.SetFloat("Speed", NMA.velocity.magnitude);

        resetTimer -= Time.deltaTime;
        cooldownTimer -= Time.deltaTime;

        if(resetTimer <= 0) {

            int attackType = 0;
            ANIM.SetInteger("AttackType", attackType);
            resetTimer = 1f;
        }

        if(backupTimer <= 0) {
            backupTimer = 2f;
        }
        backupTimer -= Time.deltaTime;

        ANIM.SetBool("Attacking", attacking);
        float SqrDistance = float.MaxValue;
        if (target != null)
         SqrDistance = Vector3.SqrMagnitude(target.transform.position - transform.position);

        if ( SqrDistance <= agroRange*agroRange && !Aggroed) {
            ANIM.SetTrigger("Taunt");
            
            Aggroed = true;

        }
        else if(Aggroed && SqrDistance > (agroRange*agroRange)*2) {
            Aggroed = false;
        }

        if (backingUp && NMA.isActiveAndEnabled) {
            attacking = false;
            NMA.destination = movePos;
            NMA.speed = speed;
            if(NMA.remainingDistance <= 1f) {
                StopBacking();
            }

        }
 


        if (target != null && Aggroed && NMA.isActiveAndEnabled && !backingUp) {
   
                NMA.destination = target.position;          


            if (!backingUp && NMA.remainingDistance != 0 && NMA.remainingDistance <= backupRange) {

                
                RaycastHit hit = new RaycastHit();
                Ray backRay = new Ray(transform.position, -transform.forward);


                if (Physics.Raycast(backRay, out hit, backupRange - NMA.remainingDistance)) {
                    movePos = hit.point + transform.forward;
                }
                else {
                    movePos = transform.position - transform.forward*(backupRange);
                }
                if(backupTimer <= 0) {

                    backingUp = true;
                }
                return;
            }

            if (NMA.remainingDistance <= engagementRange && NMA.remainingDistance > backupRange && cooldownTimer <= 0) {
                if(NMA.remainingDistance <= backupRange+3f) {

                    NMA.speed = 0;

                }
                Vector3 newLookPos = target.transform.position + target.GetComponent<ThidPersonMovement>().currentMoveVelocity.normalized;
                newLookPos.y = transform.position.y;
                transform.LookAt(newLookPos - transform.right * .05f);

                attacking = true;
                cooldownTimer = attackCooldown;
                return;
            } 
            else {
                attacking = false;
            }
        }
    }

    void StopBacking() {

        backingUp = false;
        NMA.destination = target.position;
        
    }

    public void Agro() {
        if(!Aggroed) {
            ANIM.SetTrigger("Taunt");

            Aggroed = true;

        }
    }

    public void CastAbility() {
        spellPos.LookAt(target.position);
        mageAbility.setupProjectile(spellPos.forward, (((target.GetComponent<ThidPersonMovement>().currentMoveVelocity*(Random.Range(.55f,.35f))) + target.position) -(Vector3.up*2f) - transform.right*1));
        mageAbility.Cast(spellPos);
    }
    

    public void HaultMovement() {
        NMA.speed = 0;
    }


    public enum ColliderParam
    {
        Spear,
        Shield,
        Kick
    }
    public void EnableCollider(ColliderParam go) {
        Collider temp = null;
        switch (go) {
            case ColliderParam.Kick:
                temp = kickCollider;
                break;

        }
        if(temp != null)
        temp.enabled = true;
    }

    public void DisableCollider(ColliderParam go) {
        Collider temp = null;
        switch (go) {
            case ColliderParam.Kick:
                temp = kickCollider;
                break;

        }

        temp.enabled = false;
    }

    public void ResumeMovement() {
        NMA.speed = speed;
    }

    public void ResetTaunt() {
        ANIM.ResetTrigger("Taunt");
    }

    public void TakeDamage(Damage dmg) {
        currentHealth -= dmg.amount;
        if (currentHealth <= 0)
            Die();
    }

    public void Die() {
        ANIM.SetTrigger("Death");
        ANIM.SetInteger("DeathType", Random.Range(0, 4));
        NMA.enabled = false;
    }

    public void PostDeath() {
        this.GetComponent<CharacterController>().enabled = false;
        this.GetComponent<Collider>().enabled = false;
        EnableRagdoll();
        this.gameObject.AddComponent<Rigidbody>();
        this.enabled = false;
    }

    void EnableRagdoll() {
        foreach (Collider c in ragdoll) {
            c.enabled = true;
            //c.gameObject.AddComponent<Rigidbody>();
        }
    }
}
