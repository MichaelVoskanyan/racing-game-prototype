using Godot;
using Array = Godot.Collections.Array;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualBasic;
using System.Linq;
using System.IO;

/// <summary>
/// Class contains method definitions for physics calculations and their application to the 
/// rigidbody Car. 
/// </summary>
public partial class Car : RigidBody3D
{
#region References
	[ExportCategory("References")]
	[Export] public Drivetrain drivetrain;
	[Export] public string[] S_Wheels = { "fl", "fr", "bl", "br" };
	[Export] public RayCast3D cog;
	List<Wheel> wheels = new List<Wheel>();
	Wheel fl;
	Wheel fr;
	Wheel bl;
	Wheel br;
	[Export] public Controller controller;
#endregion

#region CarConfig
	[ExportCategory("Car Configuration")]
	[Export] public float weight; // kilos
	[Export] public float drag_coefficient;
	[Export] public float frontal_area;
	[Export] public float air_density = 1.29f; // kg/m^3
	[Export] public float wheelbase = 3.584f;	// R34 wheelbase in meters
	[Export] public float b = 1.049f;
	[Export] public float c = 2.535f;
#endregion
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

	public void MoveForwardTest(double delta)
	{
		ApplyCentralForce(direction * 50f);
	}

	public void MoveBackTest(double delta)
	{
		ApplyCentralForce(-direction * 50f);
	}

	public float WeightOnWheel(Wheel wheel) {
		string name = wheel.Name;
		if (name.Contains('f'))
			return c/wheelbase * weight - ((cog.Position - cog.GetCollisionPoint()).Length()/wheelbase) * Mass * f_longi.Length();
		else
			return b/wheelbase * weight + ((cog.Position - cog.GetCollisionPoint()).Length()/wheelbase) * Mass * f_longi.Length();
	}

	public bool InitWheels() {
		foreach(string s in S_Wheels) {
			GD.Print(s);
			wheels.Add(GetNode<Wheel>(s));
		}
		if (wheels.Count() == S_Wheels.Count()){
			fl = GetNode<Wheel>("fl");
			fr = GetNode<Wheel>("fr");
			bl = GetNode<Wheel>("bl");
			br = GetNode<Wheel>("br");
			return true;
		}
		
		
		return false;
	}
	
	/// <summary>
	/// Initialize and define all references (drivetrain, wheels, controls)
	/// </summary>
	public override void _Ready()
	{
		base._Ready();
		// label = GetNode<Label>("Label");
		// configuring weight
		weight = Mass * (float)ProjectSettings.GetSetting("physics/3d/default_gravity");
		drivetrain = GetNode<Drivetrain>("drivetrain");
		GD.Print(drivetrain);
		controller = GetNode<Controller>("controller");
		GD.Print(controller);
		cog = GetNode<RayCast3D>("COG");
		if(!InitWheels())
			GD.Print("Failed to initialize Wheels");


	}

	public override void _Process(double delta)
	{
		base._PhysicsProcess(delta);
		direction = GlobalTransform.Basis.Z;
		if (controller.throttle > 0)
		{
			MoveForwardTest(delta);
		} else if (controller.brake > 0) {
			MoveBackTest(delta);
		}
		// foreach(Wheel w in wheels) {
		// 	w.SuspensionPosition((float)delta);
		// 	ApplyForce(w.suspensionForce, w.GetCollisionPoint());
		// }
	}
}
