using Godot;
using Array = Godot.Collections.Array;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualBasic;
using System.Linq;

/// <summary>
/// Class contains method definitions for physics calculations and their application to the 
/// rigidbody Car. 
/// </summary>
public partial class Car : RigidBody3D
{
	[ExportCategory("References")]
	[Export] public Drivetrain drivetrain;
	[Export] public string[] driving_wheels = { "fl", "fr", "bl", "br" };
	[Export] public string[] steering_wheels = { "fl", "fr" };
	List<Wheel> d_wheels = new List<Wheel>();
	List<Wheel> s_wheels = new List<Wheel>();
	[Export] public Controller controller;

	[ExportCategory("Car Configuration")]
	[Export] public float weight; // kilos
	[Export] public float drag_coefficient;
	[Export] public float frontal_area;
	[Export] public float air_density = 1.29f; // kg/m^3

	// Force applied to car when accelerating longi
	Vector3 f_traction = new Vector3(0, 0, 0);
	// Force applied to car by air resistance 
	Vector3 f_drag = new Vector3(0, 0, 0);
	// Force applied to car by tire rolling resistance
	Vector3 f_roll_resistance = new Vector3(0, 0, 0);
	// Force applied to car when braking
	Vector3 f_braking = new Vector3(0, 0, 0);

	// Overall force applied to car under movement
	Vector3 f_longi = new Vector3(0, 0, 0);

	Vector3 direction = new Vector3(0, 0, 0);

	public void MoveTest(double delta)
	{
		LinearVelocity += direction * 5f * (float)delta;
	}

	public bool DrivingWheelsInit()
	{
		foreach (string s in driving_wheels)
		{
			d_wheels.Add(GetNode<Wheel>(s));
		}
		if (d_wheels.Count() == driving_wheels.Count())
			return true;
		else
			return false;
	}

	public bool SteeringWheelsInit()
	{
		foreach (string s in steering_wheels)
		{
			s_wheels.Add(GetNode<Wheel>(s));
		}
		if (s_wheels.Count() == steering_wheels.Count())
			return true;
		else
			return false;
	}

	/// <summary>
	/// Initialize and define all references (drivetrain, wheels, controls)
	/// </summary>
	public override void _Ready()
	{
		base._Ready();
		// label = GetNode<Label>("Label");
		drivetrain = GetNode<Drivetrain>("drivetrain");
		GD.Print(drivetrain);
		controller = GetNode<Controller>("controller");
		GD.Print(controller);
		if (!SteeringWheelsInit())
			GD.PrintErr("Error initializing steering wheels!");
		if (!DrivingWheelsInit())
			GD.PrintErr("Error Initializing driving wheels!");


	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		direction = GlobalTransform.Basis.Z;
		if (controller.throttle > 0)
		{
			MoveTest(delta);
		}
	}
}
