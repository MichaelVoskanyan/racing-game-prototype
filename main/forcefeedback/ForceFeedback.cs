using Godot;
using System;
using System.Runtime.InteropServices;
public partial class ForceFeedback : Node3D
{
    int wheelIdx;
    int force = 50;
    public override void _Ready()
    {
        base._Ready();
        if (LogitechGSDK.LogiSteeringInitialize(true)) {
            GD.Print("Wheel Initialized");
            if (LogitechGSDK.LogiIsPlaying(0, LogitechGSDK.LOGI_FORCE_SPRING))
                LogitechGSDK.LogiStopSpringForce(0);
            else
                LogitechGSDK.LogiPlaySpringForce(0, 0, 50, 50);

        }
        else {
            GD.Print("Init failed");
        }
    }

    public override void _Process(double delta)
    {
        // GD.Print("Inside Process");
        // if (Input.IsActionJustPressed("eStop")) {
        //     Stop();
        // }

        base._Process(delta);
        if (LogitechGSDK.LogiUpdate()) {
            // Check if moving

            // Check if stopped

            // Check bump
                // check if left or right side front wheels
                // check if rear wheels
            
            // Check if bumpy

            // check if no effects playing
        }
        
    }

    public void TestFFB() {
        GD.Print("Test FFB called");
        LogitechGSDK.LogiPlaySideCollisionForce(0, force);
    }

    public void Stop() {
        GD.Print("Emergency Stop");
        LogitechGSDK.LogiSteeringShutdown();
    }
}
