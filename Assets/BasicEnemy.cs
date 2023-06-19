using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class BasicEnemy : Enemy
{
    public enum attackType
    {
        THRUST,
        MELEE,
        RANGED
    }
    public enum attackState
    {
        Targeting,
        Charging,
        Attacking,
        Cooldown,
        Dead
    }

    [Header("General")]
    public float speed;


    [Header("Movement")]
    public float speedMod;
    public bool getsKnockedback;
    public float knockbackAmt = 1f;
    float speedDebuffTime;
    NavMeshAgent NMA;

    Damage lastHit;
    Rigidbody RB;


    [Header("Attack")]
    public attackType AttackType;
    public Damage damage;
    public attackState currentAttackState;
    public float agroRange = 10f;
    public float channelRange;
    public float attackRange;
    public float targetLockTime;
    public float attackTime;
    public float attackCooldown;
    public Vector2 attackForce;

    float atime;
    bool isAttacking = false;
    bool isReturning = false;

    public Transform target;
    public Vector3 returnPos;

    // Start is called before the first frame update
    void Start() {
        gameObject.AddComponent<Rigidbody>();
        RB = gameObject.GetComponent<Rigidbody>();
        NMA = GetComponent<NavMeshAgent>();
        RB.isKinematic = true;
        target = FindObjectOfType<ThidPersonMovement>().transform;
        
    }


    GameObject hitTarget;
    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, target.position) <= agroRange) {
            Aggroed = true;
        }

        if(speedDebuffTime > 0) {
            speedDebuffTime -= Time.deltaTime;

        }
        else {
            speedMod = 1;
        }
        if(currentHealth <= 0 && NMA.enabled) {
            NMA.isStopped = true;
            NMA.enabled = false;

        }

        if (NMA.enabled && NMA.isActiveAndEnabled) {
            NMA.speed = speed * speedMod;
            if (NMA.speed <= 0 && NMA.isOnNavMesh) {
                NMA.isStopped = true;
            }
            else {
                if(NMA.enabled && NMA.isActiveAndEnabled)
                    NMA.isStopped = false;
            }

            if(target != null && !isReturning && Aggroed) {

                NMA.SetDestination(target.position);
            }else if (isReturning) {
                NMA.SetDestination(returnPos);
            }
            if (NMA.path.status == NavMeshPathStatus.PathInvalid)
                return;
            
            

            if (checkAttackRaycast(channelRange) && target != null) {
                currentAttackState = attackState.Targeting;
                NMA.speed = 0;
                NMA.stoppingDistance = channelRange;
                atime -= Time.deltaTime;
            }
            else {
                currentAttackState = attackState.Targeting;
                hitTarget = null;
                NMA.stoppingDistance = 0;
                NMA.speed = speed;
                atime = targetLockTime;
            }

            if (atime <= 0 && currentAttackState == attackState.Targeting && hitTarget != null) {
                StartCoroutine(Attack());
            }
        }

        if (currentAttackState == attackState.Targeting && target != null && isReturning == false) {
            Vector3 newLookPos = target.transform.position;
            newLookPos.y = transform.position.y;
            transform.LookAt(newLookPos);
            
        }else if (isReturning) {
            Vector3 newLookPos = returnPos;
            newLookPos.y = transform.position.y;
            if(currentAttackState != attackState.Cooldown && currentAttackState != attackState.Attacking)
            transform.LookAt(newLookPos);
            if (NMA.isActiveAndEnabled && NMA.remainingDistance <= .5f)
                isReturning = false;
        }

        if (currentHealth <= 0) {
            NMA.speed = 0;
            RB.drag = 1;
            this.transform.rotation = Quaternion.Euler(180, transform.rotation.y, transform.rotation.z);
            currentAttackState = attackState.Dead;
        }

        //Thrust Attack
        if (isAttacking && currentAttackState == attackState.Attacking && AttackType == attackType.THRUST) {
            List<Collider> cols = new List<Collider>();
            cols.AddRange(Physics.OverlapSphere(transform.position, attackRange, 1<<target.gameObject.layer));

            if (cols.Contains(target.GetComponent<Collider>()) && hitTarget != null) {
                damage.setSourceAndPos(this.gameObject, transform.position);
                hitTarget.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
                isAttacking = false;
                hitTarget = null;
                isReturning = true;
                return;
            }
        }

        
    }


    public void DebuffSpeed(IAbility.Debuff debuff) {
        speedMod = debuff.debuffAmount; speedDebuffTime = debuff.debuffTime;
    }

    public void TakeDamage(Damage dmg) {
        lastHit = dmg;
        currentHealth -= dmg.amount;
        if(getsKnockedback || currentHealth<= 0)
        StartCoroutine(knockBack());
    }

    IEnumerator knockBack() {
        NMA.enabled = false;
        if (currentHealth > 0) {
            RB.constraints = RigidbodyConstraints.FreezeRotation;

        }
        RB.isKinematic = false;

        Vector3 diff = transform.position - lastHit.position;
        diff.y = 0;
        RB.AddForce((diff * lastHit.knockBackForce)* knockbackAmt, ForceMode.Impulse);
        yield return new WaitForSeconds(1f);


        if (currentHealth > 0) {
            NMA.enabled = true;
            RB.isKinematic = true;
        }
        RB.constraints = RigidbodyConstraints.None;




    }



    bool checkAttackRaycast(float range) {
        if (target == null)
            return false;

        Ray ray = new Ray(transform.position + transform.forward, transform.forward);
        RaycastHit[] hits = Physics.RaycastAll(ray, range, 1<<target.gameObject.layer);

        if (hits.Length > 0) {
                hitTarget = hits[0].transform.gameObject;
                return true;
        }

        return false;
    }

    void LockRigidbody() {
        RB.isKinematic = true;
        NMA.enabled = true;
        NMA.Warp(transform.position);
        NMA.isStopped = false;
     
        RB.constraints = RigidbodyConstraints.None;
    }

    void UnlockRigidbody() {
        if (NMA.enabled) {
            NMA.isStopped = true;
            NMA.enabled = false;
            RB.isKinematic = false;
        }

        RB.constraints = RigidbodyConstraints.FreezeRotation;
    }

    IEnumerator Attack() {
        
        switch (AttackType) {
            case attackType.THRUST:
                currentAttackState = attackState.Charging;
                if (NMA.enabled && currentHealth > 0) {
                    UnlockRigidbody();
                }
                else
                    yield break;

                Vector3 targetLock = target.transform.position;
                returnPos = transform.position;


                yield return new WaitForSeconds(attackTime);

                if (currentHealth <= 0)
                    yield break;

                currentAttackState = attackState.Attacking;
                RB.AddForce((transform.up * attackForce.y), ForceMode.Impulse);
                RB.AddForce(((targetLock - transform.position).normalized).normalized * attackForce.x, ForceMode.Impulse);
                isAttacking = true;

                yield return new WaitForSeconds(1f);
                if (currentHealth <= 0)
                    yield break;


                isAttacking = false;
                currentAttackState = attackState.Cooldown;

                yield return new WaitForSeconds(attackCooldown);


                if (currentHealth > 0) {

                    NMA.enabled = true;
                    if (NMA.isOnNavMesh)
                        LockRigidbody();
                }
                else {
                    RB.velocity = Vector3.zero;
                }

                break;
        }
        
        
    }


    public bool IsAgentOnNavMesh(GameObject agentObject) {
        Vector3 agentPosition = agentObject.transform.position;
        NavMeshHit hit;

        // Check for nearest point on navmesh to agent, within onMeshThreshold
        if (NavMesh.SamplePosition(agentPosition, out hit, 5f, NavMesh.AllAreas)) {
            // Check if the positions are vertically aligned
            if (Mathf.Approximately(agentPosition.x, hit.position.x)
                && Mathf.Approximately(agentPosition.z, hit.position.z)) {
                // Lastly, check if object is below navmesh
                return agentPosition.y >= hit.position.y;
            }
        }

        return false;
    }
}
