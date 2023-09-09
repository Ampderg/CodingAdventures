using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameHandler
{
    public static bool Paused;
    public static EventHandler<PauseEventArgs> OnPauseChanged;

    public class PauseEventArgs : EventArgs
    {
        public bool IsPaused { get; set; }
    }
}
