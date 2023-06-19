using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControl : MonoBehaviour
{
    public float timescale;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = timescale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
