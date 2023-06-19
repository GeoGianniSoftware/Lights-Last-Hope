using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject warningPage;
    public GameObject creditsPage;
    public GameObject controlsPage;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startScene() {
        SceneManager.LoadScene(1);
    }

    public void openCredits() {
        creditsPage.SetActive(true);
    }

    public void openControls() {
        controlsPage.SetActive(true);
    }

    public void closeCredits() {
        creditsPage.SetActive(false);
    }

    public void closeControls() {
        controlsPage.SetActive(false);

    }

    public void openWarning() {
        warningPage.SetActive(true);
    }

    public void openLink() {
        Application.OpenURL("https://twitter.com/Ge0Gianni");
    }
    public void closeWarning() {
        warningPage.SetActive(false);
    }

    public void Exit() {
        Application.Quit();
    }
}
