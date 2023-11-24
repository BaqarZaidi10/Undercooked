// Required using directive for Unity classes and data structures
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class represents a ScriptableObject that defines properties for a frying recipe
[CreateAssetMenu()]
public class FryingRecipeSO : ScriptableObject
{

    // The kitchen object to be used as input in this frying recipe
    public KitchenObjectSO input;

    // The kitchen object to be produced as output after frying
    public KitchenObjectSO output;

    // The maximum time allowed for frying this recipe
    public float fryingTimerMax;
}
