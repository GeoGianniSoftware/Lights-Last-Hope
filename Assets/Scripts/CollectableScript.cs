using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableScript : MonoBehaviour
{
    public enum collectableType
    {
        Health,
        MaxHealth
    }
    public collectableType pickupType;
    public float rotationSpeed = .025f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed);
    }

    private void OnTriggerEnter(Collider other) {

        if (other.tag == "Player") {
            switch (pickupType) {
                case collectableType.Health:
                    if (other.GetComponent<CombatController>().currentHealth == other.GetComponent<CombatController>().maxHealth) {
                        break;
                    }
                    other.transform.SendMessage("Heal", 1, SendMessageOptions.DontRequireReceiver);
                    Destroy(gameObject);
                    break;
                case collectableType.MaxHealth:
                    other.transform.SendMessage("GainHeart", 1, SendMessageOptions.DontRequireReceiver);
                    other.transform.SendMessage("Heal", 1, SendMessageOptions.DontRequireReceiver);
                    Destroy(gameObject);
                    break;
            }

                
            
        }
    }
}
