using Godot;
using Array = Godot.Collections.Array;
using System;
using System.Linq;

/// <summary>
/// Class contains method definitions for physics calculations and their application to the 
/// rigidbody Car. 
/// </summary>
public partial class Car : RigidBody3D
{
    [ExportCategory("References")]
    [Export] public Drivetrain drivetrain;
    [Export] public Array wheels = new Array();
    [Export] public Controller controls;

    [ExportCategory("Car Configuration")]
    [Export] public float weight; // kilos
    [Export] public float drag_coefficient;
    [Export] public float frontal_area;
    [Export] public float air_density = 1.29f; // kg/m^3
    
    // Force applied to car when accelerating longi
    Vector3 f_traction = new Vector3(0,0,0);
    // Force applied to car by air resistance 
    Vector3 f_drag = new Vector3(0,0,0);
    // Force applied to car by tire rolling resistance
    Vector3 f_roll_resistance = new Vector3(0,0,0);
    // Force applied to car when braking
    Vector3 f_braking = new Vector3(0,0,0);

    // Overall force applied to car under movement
    Vector3 f_longi = new Vector3(0,0,0);

    Vector3 direction = new Vector3(0,0,0);


    // Determines user steering input and resulting steering force
    void Steering() {

    }

    /// <summary>
    /// Calls drivetrain engine force and applies to rigidbody
    /// </summary>
    void EngineForce() {

    }

    /// <summary>
    /// Calls drivetrain brake force and applies to rigidbody
    /// </summary>
    void BrakeForce() {

    }

    /// <summary>
    /// Initialize and define all references (drivetrain, wheels, controls)
    /// </summary>
    public override void _Ready()
    {
        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
    }
}
