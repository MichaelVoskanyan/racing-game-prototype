using Godot;
using Array = Godot.Collections.Array;
using Dictionary = Godot.Collections.Dictionary;
using System;
using System.Linq;

public partial class Drivetrain : Node
{
    #region Engine
    [ExportCategory("Engine Configuration")]
    [Export] public float torque_max;
    [Export] public float rpm_max;
    [Export] public string power_band_file_path = "main/power.json";
    public Dictionary power_band = new Dictionary();
    public float rpm_current = 1505f;
    #endregion

    #region Transmission
    [ExportCategory("Transmission Configuration")]
    [Export] public Array gear_ratios = new Array();
    [Export] public float reverse_ratio;
    [Export] public float diff_ratio;
    public int gear_current;
    #endregion

    #region Brakes
    [ExportCategory("Brake Configuration")]
    [Export] public float brake_force_max;
    [Export] public float brake_bias;   // bias towards front
    #endregion

    

    [Export] Wheel[] driving_wheels;

    

    public void ImportEngineTorqueCurve() {
        var jsonString = System.IO.File.ReadAllText(power_band_file_path);
        power_band = (Dictionary)Json.ParseString(jsonString);
        foreach(var i in power_band) {
            GD.Print("" + (float)i.Key + ' ' + i.Value);
        }
    }
    
    public float EngineTorque(float throttle) {
        Array k = (Array)power_band.Keys;
        Array v = (Array)power_band.Values;

        for(int i = 0; i < k.Count(); i++) {
            if(rpm_current > (float)k[i])
                continue;
            
            if (i > 0) {
                float p = (rpm_current - (float)k[i-1]) / ((float)k[i]-(float)k[i-1]);
                float pow = (float)v[i] - (float)v[i-1];
                pow *= p;
                return (pow + (float)v[i-1]) * throttle;
            }
            else {
                return ((rpm_current / (float)k[i]) * (float)v[i]) * throttle;
            }
        }
        
        return 0.0f;
    }

    public float EngineRPM(float v) {
        if (v > 0) {
            var rotation_rate = v / (driving_wheels[0].TireDiameter() * Mathf.Pi);
            return rotation_rate * (float)gear_ratios[gear_current] * diff_ratio * (60/(2 * Mathf.Pi));
        }
        else if (v < 0) {
            var rotation_rate = v / (driving_wheels[0].TireDiameter() * Mathf.Pi);
            return rotation_rate * reverse_ratio * diff_ratio * (60/(2 * Mathf.Pi));
        }
        else {
            // check if clutch or brakes
            // if manual and clutch engaged, rpm = 0
            // if automatic and no brakes, rpm stays same and force applied (externally)
            // if automatic and brakes, rpm stays same
        }
        return 0f;
    }

    public float BrakeForce(float brake) {
        return brake_force_max * brake;
    }

    public override void _Ready()
    {
        base._Ready();
        ImportEngineTorqueCurve();
        GD.Print("Current torque: " + EngineTorque(0.5f));
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        
    }

}
