using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu()]
public class OvenRecipe : ScriptableObject
{
    public KitchenObjectSO input;
    public KitchenObjectSO output;
    public float bakingTimerMax;
}
