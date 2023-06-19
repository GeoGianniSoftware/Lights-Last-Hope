using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutsceneActivate : MonoBehaviour
{
    public CinemachineFreeLook mainCamera;
    public Transform destination;
    GameObject playerRef;
    public Image victoryScreen;
    public GameObject playerUI;
    public GameObject FPS;
    public GameObject cursor;
    public GameObject interactText;
    public AudioClip victoryMusic;
    public GameObject areatext, areatext1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(triggered)
            victoryScreen.color = Color.Lerp(victoryScreen.color, new Color(victoryScreen.color.r, victoryScreen.color.g, victoryScreen.color.b, victoryScreen.color.a + Time.deltaTime*.5f), .18f);
        if (victoryScreen.color.a > 1.0f) {
            //End Game
            SceneManager.LoadScene(0);
        }
    }

    bool triggered = false;
    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            if (!triggered) {
                playerRef = FindObjectOfType<ThidPersonMovement>().gameObject;
                FinalCutsceene();
                triggered = true;
            }
        }
    }

    void FinalCutsceene() {
        if(FindObjectOfType<MusicManager>() != null)
        Object.FindObjectOfType<MusicManager>().GetComponent<AudioSource>().PlayOneShot(victoryMusic);

        playerUI.SetActive(false);
        mainCamera.Priority = -1;
        Animator anim = playerRef.GetComponent<ThidPersonMovement>().Anim;
        playerRef.GetComponent<ThidPersonMovement>().enabled = false;
        cursor.SetActive(false);
        FPS.SetActive(false);
        playerRef.GetComponent<CharacterController>().enabled = false;
        playerRef.GetComponent<CombatController>().enabled = false;
        playerRef.GetComponent<InteractionController>().enabled = false;
        NavMeshAgent NMA = playerRef.GetComponent<NavMeshAgent>();
        NMA.enabled = true;
        interactText.SetActive(false);
        areatext.SetActive(false);
        areatext1.SetActive(false);
        playerRef.GetComponent<NavMeshAgent>().SetDestination(destination.position);
        anim.SetFloat("Speed", 10f);
        victoryScreen.gameObject.SetActive(true);
    }
}
