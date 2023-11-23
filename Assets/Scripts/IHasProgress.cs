using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interface for objects that have progress
public interface IHasProgress
{

    // Event triggered when the progress changes
    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;

    // Custom EventArgs class for the OnProgressChanged event
    public class OnProgressChangedEventArgs : EventArgs
    {
        public float progressNormalized;
    }

}
