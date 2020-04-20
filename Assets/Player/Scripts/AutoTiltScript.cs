using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTiltScript : Throttleable
{
    public RocketEngineScript engine;

    [Range(min: 0, max: 1)]
    public float stabilization = 0;

    public float maxTilt = 45;
    public float speedCap = 10;

    float horizontalSpeed => Vector2.Dot(rigidbody.velocity, Vector2.right);

    public float targetAngle => speedCap == 0 ? 0f :
        Mathf.Clamp(horizontalSpeed / speedCap, -1f, 1f) * -maxTilt;

    public override float Throttle {
        get => engine == null ? 0f : engine.Throttle;
        set { if (engine != null) engine.Throttle = value; }
    }

    private void Update()
    {
        if (stabilization > 0)
        {
            // Get the rigidbody's rotation as a value normalized between -1 and 1.
            float wrongWay = targetAngle - rigidbody.rotation;
            while (wrongWay < -180) wrongWay += 360;
            while (wrongWay > 180) wrongWay -= 360;
            wrongWay /= 180f;

            wrongWay *= stabilization * 2;

            engine.Throttle = Mathf.Clamp(engine.invertSpinCheck ? -wrongWay : wrongWay, -1f, 1f);
        }
    }
}
