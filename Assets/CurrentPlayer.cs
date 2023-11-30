using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentPlayer : MonoBehaviour
{
    public GameObject currentPlayer1;
    private void OnTriggerEnter(Collider other)
    {
        currentPlayer1 = other.gameObject;
        print(currentPlayer1.name);
    }

    private void OnTriggerExit(Collider other)
    {
        currentPlayer1 = null;
    }
}
