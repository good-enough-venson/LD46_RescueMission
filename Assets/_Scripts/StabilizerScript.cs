using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabilizerScript : Throttleable
{
    public RocketEngineScript engine;

    [Range(min: 0, max: 1)]
    public float stabilization = 0;
    public float defaultAngle = 0;
    public bool useDefaultAngle = true;

    [Space, SerializeField, ReadOnly]
    private float overrideThrottle = 0;

    [SerializeField, ReadOnly]
    public float lastSetAngle = 0;

    public float targetAngle => useDefaultAngle ?
        defaultAngle : lastSetAngle;

    public override float Throttle
    {
        get => engine == null ? 0f : engine.Throttle;

        set {
            if (engine != null) engine.Throttle = value;
            if ((overrideThrottle = value.Abs()) <= 0.01f)
                lastSetAngle = rigidbody.rotation;
        }
    }

    private void Update()
    {
        if (stabilization > 0 && overrideThrottle < 0.01f)
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
