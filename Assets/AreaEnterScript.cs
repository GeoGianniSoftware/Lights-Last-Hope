using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AreaEnterScript : MonoBehaviour
{
    public string areaName;
    float timeVisible = 2f;
    bool triggered = false;
    public Text areaText;
    public Text areaTextShadow;

    public GameObject loadPart;
    public GameObject unloadPart;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    bool unopened = true;
    void Update()
    {
        if (triggered && unopened) {
            Destroy(gameObject, timeVisible*2);
            areaText.text = areaName;
            areaTextShadow.text = areaName;
            areaText.text = areaText.text.Replace("\\", "\n");
            areaTextShadow.text = areaText.text.Replace("\\", "\n");
            if (timeVisible > .8f) {

                areaText.color = Color.white;
                areaTextShadow.color = Color.black;
            }

            timeVisible -= Time.deltaTime;
            if(timeVisible<= 1f) {
                areaText.color = Color.Lerp(areaText.color, new Color(areaText.color.r, areaText.color.g, areaText.color.b, areaText.color.a - Time.deltaTime * 10f), .18f);
                areaTextShadow.color = Color.Lerp(areaTextShadow.color, new Color(areaTextShadow.color.r, areaTextShadow.color.g, areaTextShadow.color.b, areaTextShadow.color.a - Time.deltaTime * 10f), .18f);
            }
            else if(timeVisible <= 0.5f) {
                areaText.color = new Color(areaText.color.r, areaText.color.g, areaText.color.b, 0);
                areaTextShadow.color = new Color(areaTextShadow.color.r, areaTextShadow.color.g, areaTextShadow.color.b, 0);
                areaText.text = "";
                areaTextShadow.text = "";
                unopened = false;
            }
            
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            if(loadPart != null)
            loadPart.SetActive(true);
            if(unloadPart != null)
            unloadPart.SetActive(false);
            triggered = true;
        }
    }

    


}
