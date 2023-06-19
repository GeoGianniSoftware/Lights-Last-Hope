using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathScript : MonoBehaviour
{
    PathManager PM;
    NavMeshAgent NMA;

    public List<Vector3> currentPath = null;
    public bool hasPath;
    Vector3 currentNodePos;
    Vector3 direction;
    float distanceToNode;

    private void Start() {
        NMA = GetComponent<NavMeshAgent>();
        PM = FindObjectOfType<PathManager>();
    }

    void RecievePath(List<Vector3> pathToRecieve) {
        print("Recieved Path!");
        currentPath = pathToRecieve;
        hasPath = true;
        print(pathToRecieve.Count);
        NMA.enabled = false;
    }

    private void Update() {
        if(!hasPath) {
            print("Asking for path/");
            NMA.enabled = true;
            PM.queueForPath(new PathManager.PathRequest(NMA, FindObjectOfType<ThidPersonMovement>().transform.position));
            

        }
        else if(hasPath){
            distanceToNode = Vector3.SqrMagnitude(currentNodePos - transform.position);

            if(distanceToNode > 1) {
                transform.position += (currentNodePos - transform.position).normalized * 20f * Time.deltaTime;
            }
            else {
                currentPath.RemoveAt(0);
                GetNextNode();
            }
        }
        else {
            
        }
    }

    private void GetNextNode() {
        if (currentPath.Count > 0) {
            currentNodePos = currentPath[0];
            direction = currentNodePos - transform.position;
            hasPath = true;
        }
        else {
            hasPath = false;

        }
    }
}
