using UnityEngine;

[System.Serializable]
public struct EngineStats
{
    [Tooltip("The thrust this engine delivers at full power.")]
    [Range(min: 0, max: 30)]
    public float thrust;

    [Tooltip("The speed at which this engine will cease to push.")]
    [Range(min: 0, max: 60)]
    public float maxSpeed;

    [Tooltip("For handling engines, the angular velocity " +
        "at which this engine will cease to push.")]
    [Range(min: 0, max: 300)]
    public float maxAngVel;

    [Tooltip("The amount of fuel this engine uses per second at full power.")]
    [Range(min: 0, max: 10)]
    public float fuelUse;

    [Tooltip("The time it takes to go from zero to full power.")]
    [Range(min: 0, max: 20)]
    public float weight;
}

public class RocketEngineScript : Throttleable
{
    [Label(preSpace: 10, "External Links")]
    public FuelScript tank;

    [Label(preSpace: 10, "Engine")]
    public Vector2 direction = Vector2.up;

    public EngineStats stats;

    [Space(10)]
    [Tooltip("Check this if the controls are set as " +
        "right = positive, and left = negative.")]
    public bool invertSpinCheck;

    public Vector2 forward {
        get => transform.TransformDirection(direction);
    }

    public override float Throttle {
        get => engineOutput;
        set => throttle = value;
    }

    [Space, ReadOnly]
    public float throttle = 0f;

    [ReadOnly]
    public float engineOutput = 0f;

    protected void Update()
    {
        // Lerp the desired engine output towards the current throttle setting.
        float desiredOutput = engineOutput.Move (
            towards: throttle, by: Time.deltaTime / stats.weight
        );

        // If we're using fuel, do that logic.
        if (tank != null && stats.fuelUse > 0f)
        {
            var outputMagnitude = desiredOutput.Abs();

            // Get our allotted fuel from the fuel tank.
            float fuel = tank.GetFuelOverTime(stats.fuelUse * outputMagnitude * Time.deltaTime);

            // The actual engine output is limited by how much fuel we have.
            engineOutput = Mathf.Min (
                outputMagnitude, (fuel / Time.deltaTime) /
                stats.fuelUse) * Mathf.Sign(desiredOutput
            );
        }

        else engineOutput = desiredOutput;
    }

    protected void FixedUpdate()
    {
        var forwardSpeed = Vector3.Dot(rigidbody.velocity, forward);
        var tooFast = stats.maxSpeed > 0f && forwardSpeed >= stats.maxSpeed;

        var tooDizy = stats.maxAngVel > 0f &&
            rigidbody.angularVelocity.Abs() > stats.maxAngVel &&
            Mathf.Sign(rigidbody.angularVelocity) == Mathf.Sign(
                engineOutput) * (invertSpinCheck ? -1 : 1);

        if (!tooFast && !tooDizy) {
            rigidbody.AddForceAtPosition (
                forward * engineOutput * stats.thrust,
                transform.position, ForceMode2D.Force
            );
        }
    }
}
