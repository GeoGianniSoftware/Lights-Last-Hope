using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
    public bool destroyOnHit;
    public Damage damage;
    public float gravityMod;
    public bool sticks;
    public GameObject onHitFXPrefab;


    GameObject stickPoint;
    Rigidbody RB;
    bool active = true;
    public float lifeTime;
    // Start is called before the first frame update
    void Awake()
    {
        RB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 nextPos = transform.position + RB.velocity * Time.deltaTime;

        Ray ray = new Ray(transform.position, nextPos);
        RaycastHit hit;
        transform.LookAt(transform.position + RB.velocity);

        if (Physics.Linecast(transform.position, nextPos, out hit) && active) {
            if (hit.collider != null) {
                transform.position = hit.point;
                Hit(hit.collider, hit.point);
            }
        }

        RB.AddForce(Vector3.up * -Physics.gravity.y * gravityMod, ForceMode.Acceleration);

        

    }

    private void Update() {
        if (stickPoint != null) {
            transform.position = stickPoint.transform.position;
            transform.rotation = stickPoint.transform.rotation;
        }
        else {
            this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other != null) {
            Hit(other, transform.position);
        }
    }

    void Hit(Collider hitObject, Vector3 hitPos) {
        if (hitObject != null && active) {
            if (hitObject.tag == "Projectile")
                return;

            //Disable Projectile FX and Collider
            if (GetComponent<BoxCollider>()) {
                GetComponent<BoxCollider>().enabled = false;
            }
            if (GetComponentInChildren<Light>())
                Destroy(GetComponentInChildren<Light>().gameObject);
            if (GetComponentInChildren<TrailRenderer>())
                Destroy(GetComponentInChildren<TrailRenderer>().gameObject);

            //Enable Gravity
            RB.useGravity = true;

            //Damage
            
            damage.setSourceAndPos(this.gameObject, hitPos);

            hitObject.transform.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);

            //OnHitFXs
            if(onHitFXPrefab != null) {
                GameObject hitFX = Instantiate(onHitFXPrefab, transform.position, Quaternion.identity);
                hitFX.transform.LookAt(transform.position);
            }
            
            //Stick Physics and Destroy
            if (destroyOnHit)
                Destroy(gameObject);
            else {
                if (sticks) {
                    RB.isKinematic = true;
                    stickPoint = new GameObject("stickpoint");
                    if (hitObject.GetComponent<Enemy>()) {

                        stickPoint.AddComponent<ArrowStick>();
                        stickPoint.GetComponent<ArrowStick>().attachedEnemy = hitObject.GetComponent<Enemy>();
                    }
                    stickPoint.transform.position = transform.position;
                    stickPoint.transform.rotation = transform.rotation;
                    stickPoint.transform.SetParent(hitObject.transform);
                }
                
               
                Destroy(gameObject, lifeTime);
            }
            //Disable hits
            active = false;
        }
    }

    public void AddForce(Vector3 force, ForceMode mode) {
        RB.AddForce(force, mode);
    }
}
