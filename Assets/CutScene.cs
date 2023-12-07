using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CutScene : MonoBehaviour
{
    private NavMeshAgent agent;
    private PlayerController controller;
    private Vector3 originalPosition;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<PlayerController>();
        originalPosition = transform.position;
    }

    public void JudgingStart(Vector3 position)
    {
        StartCoroutine(JudgingState(position));
    }

    public void JudgingEnd()
    {
        StartCoroutine(JudgedState(originalPosition));
    }

    private IEnumerator JudgingState(Vector3 position)
    {
        controller.enabled = false;

        while(Vector3.Distance(position, transform.position) < 0.1f)
        {
            agent.SetDestination(position);
            transform.LookAt(new Vector3(position.x, transform.position.y, position.z));
            yield return null;
        }

        transform.position = position;
    }
    
    private IEnumerator JudgedState(Vector3 position)
    {
        while(Vector3.Distance(position, transform.position) < 0.1f)
        {
            agent.SetDestination(position);
            transform.LookAt(new Vector3(position.x, transform.position.y, position.z));
            yield return null;
        }

        transform.position = position;
        controller.enabled = true;
    }
}
