using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slip : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke(nameof(Disable), 5f);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}
