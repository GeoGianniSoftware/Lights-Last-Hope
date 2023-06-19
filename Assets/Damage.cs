using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Damage{
    public int amount;
    public bool respawn;
    public GameObject source;
    public Vector3 position;
    public float knockBackForce;

    public Damage(int amt, bool res) {
        amount = amt;
        respawn = res;
    }

    public void setSourceAndPos(GameObject s, Vector3 pos) {
        source = s;
        position = pos;
    }

}
