using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyHealthBar : MonoBehaviour
{
    public Enemy enemy;
    public Image healthbarFill;
    GameObject playerRef;

    // Start is called before the first frame update
    void Start()
    {
        playerRef = FindObjectOfType<ThidPersonMovement>().gameObject;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }



    // Update is called once per frame
    void Update()
    {
        healthbarFill.fillAmount = (float)enemy.currentHealth / (float)enemy.maxHealth;
        if(enemy.currentHealth <= 0) {
            Destroy(gameObject);
        }
        if(Vector3.Distance(transform.position, playerRef.transform.position) < 20f) {
            this.GetComponent<Canvas>().enabled = true;
            transform.LookAt(playerRef.transform);
        }
        else {
            this.GetComponent<Canvas>().enabled = false;
        }
    }
}
