using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject playerRef;
    void Start()
    {
        Object.DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) {
            GetComponent<AudioSource>().enabled = !GetComponent<AudioSource>().enabled;
        }

        if(playerRef == null && FindObjectOfType<ThidPersonMovement>()) {
            playerRef = FindObjectOfType<ThidPersonMovement>().gameObject;
            transform.position = playerRef.transform.position;
        }
    }
}
