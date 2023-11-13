
using System;
using Godot;
using Dictionary = Godot.Collections.Dictionary;
using Array = Godot.Collections.Array;


public partial class ground_surface_variable : StaticBody3D
{
	 
	[Export] public float wear_rate = 1.0f;
	[Export] public float heat_rate = 1.0f;
	[Export] public float fore_friction = 0.0f;
	[Export] public float fore_stiffness = 0.0f;
	[Export] public float particle_looseness = 0.0f;
	[Export] public float ground_bump_height = 0.0f;
	[Export] public float ground_bump_frequency = 0.0f;
	[Export] public float ground_bump_frequency_random = 0.0f;
	[Export] public float ground_friction = 1.0f;
	[Export] public float ground_stiffness = 1.0f;
	[Export] public float ground_builduprate = 0.0f;
	[Export] public bool ground_dirt = false;
	[Export] public float drag = 0.0f;
	
	
	
	
	
}