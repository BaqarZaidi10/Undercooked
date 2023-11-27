using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class represents a ScriptableObject that defines properties for a cutting recipe
[CreateAssetMenu()]
public class CuttingRecipeSO : ScriptableObject
{

    // The kitchen object to be used as input in this cutting recipe
    public KitchenObjectSO input;

    // The kitchen object to be produced as output after cutting
    public KitchenObjectSO output;

    // The maximum progress required for cutting this recipe
    public int cuttingProgressMax;
}
