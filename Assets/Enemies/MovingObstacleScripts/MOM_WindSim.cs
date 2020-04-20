using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MOM_WindSim : MOM_InitialVelocity
{
    [Label(preSpace: 10, "Wind Factors")]
    public Vector2 turbulence;

    //[Range(min: 0, max: 1)]
    public Vector2 windForce;

    //[Range(min: 0, max: 1)]
    public Vector2 variance;

    private Vector2 timeOffset;

    public override void Setup(float trajectory, float speed) {
        base.Setup(trajectory, speed);
        timeOffset = new Vector2(Random.Range(0f, 10f), Random.Range(0f, 10f));
    }

    protected override void FixedUpdate()
    {
        var wind = windForce * Mathf.PerlinNoise (
            Time.time * turbulence.x, turbulence.x
        );
        
        rigidbody.AddForce (
            vector * wind * speed,
            ForceMode2D.Force
        );
    }
}
