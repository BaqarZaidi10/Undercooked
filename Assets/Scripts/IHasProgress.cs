using System;
using UnityEngine;

// Interface for objects that have progress and can notify progress changes
public interface IHasProgress
{
    // Event triggered when progress changes
    event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;

    // Event arguments class for progress change events
    public class OnProgressChangedEventArgs : EventArgs
    {
        // The normalized progress value (typically in the range [0, 1])
        public float progressNormalized;
    }
}
