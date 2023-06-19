using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathManager : MonoBehaviour
{
    public class PathRequest {
        public NavMeshAgent agent;
        public Vector3 pathDestination;

        public PathRequest(NavMeshAgent _agent, Vector3 dest) {
            agent = _agent;
            pathDestination = dest;
        }
    }

    Queue<PathRequest> requestQueue = new Queue<PathRequest>();
    PathRequest currentRequest = null;
    NavMeshPath _path = null;

    public void queueForPath(PathRequest request) {
        print("Path Queued");
        if(!requestQueue.Contains(request))
            requestQueue.Enqueue(request);
    }

    private void FixedUpdate() {
        if(currentRequest == null && requestQueue.Count > 0) {
            print("Calculating Next Path");
            currentRequest = requestQueue.Dequeue();
        }
        if (_path == null && currentRequest != null) {
            print("Creating Path");
            _path = new NavMeshPath();
            currentRequest.agent.enabled = true;
            currentRequest.agent.CalculatePath(currentRequest.pathDestination, _path);
            List<Vector3> tempNodes = new List<Vector3>();
            print(currentRequest.pathDestination);
            foreach (Vector3 corner in _path.corners) {
                tempNodes.Add(corner);
            }

            if (tempNodes.Count > 0) {
                print("Sending Path");
                currentRequest.agent.SendMessage("RecievePath", tempNodes);
            }
            currentRequest = null;
            _path = null;
        }
    }



}
