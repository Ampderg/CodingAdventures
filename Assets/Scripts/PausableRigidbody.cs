using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausableRigidbody : MonoBehaviour
{
    private bool kinematicState;
    private Vector3 pauseVelocity;
    private Vector3 pauseAngular;
    private new Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        GameHandler.OnPauseChanged += OnPauseChanged;
        rigidbody = this.GetComponent<Rigidbody>();
    }

    private void OnPauseChanged(object sender, GameHandler.PauseEventArgs e)
    {
        if (e.IsPaused)
        {
            kinematicState = rigidbody.isKinematic;
            pauseVelocity = rigidbody.velocity;
            pauseAngular = rigidbody.angularVelocity;
            rigidbody.isKinematic = true;
        }
        else
        {
            rigidbody.isKinematic = kinematicState;
            rigidbody.velocity = pauseVelocity;
            rigidbody.angularVelocity = pauseAngular;
        }
    }

    public void SetKinematic(bool value)
    {
        this.kinematicState = value;
        if (!GameHandler.Paused)
            rigidbody.isKinematic = kinematicState;
    }
}
