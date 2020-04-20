using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    new public Rigidbody2D rigidbody {
        get => _rigidbody ? _rigidbody : _rigidbody = GetComponent<Rigidbody2D>();
        set => _rigidbody = value;
    }

    private Animator _animator;
    public Animator animator {
        get => _animator ? _animator : _animator = GetComponent<Animator>();
        set => _animator = value;
    }

    public AudioSource rocketVolume;
    [Range(0,1)]
    public float volumeMultiplier;

    Controls controls;
    public Transform center;
    public Transform arm;
    public SpriteRenderer bodyRend;

    public Throttleable armThruster;

    public float engineAnimValScalar = 0.5f;
    public string enginePowerParameterName = "EnginePower";
    private int enginePowerIndex;

    [SerializeField, ReadOnly]
    private float enginePower;

    public float throttle
    {
        get => armThruster == null ? 0f :
            armThruster.Throttle;

        set {
            if (armThruster != null)
                armThruster.Throttle = value;
        }
    }
    
    Vector2 centerPos => center.position;
    float horizontalSpeed => Vector2.Dot(rigidbody.velocity, Vector2.right);

    void UpdateArmRotation(Vector2 target) {
        Vector3 vector = target - centerPos;
        enginePower = vector.magnitude;
        var angle = Vector2.SignedAngle(Vector2.right, vector) + 90;
        arm.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void Awake()
    {
        controls = new Controls();

        controls.InGame_Actions.AimScreen.performed += (val) => {
            OnReceiveMousePosition(val.ReadValue<Vector2>());
        };

        controls.InGame_Actions.Thrusters.performed += (val) => {
            OnReceiveThrottleAdjustment(val.ReadValue<float>());
        };

        controls.InGame_Actions.Thrusters.canceled += (val) => {
            OnReceiveThrottleAdjustment(val.ReadValue<float>());
        };

        enginePowerIndex = new List<AnimatorControllerParameter>(
            animator.parameters).Find(p => p.name == enginePowerParameterName).nameHash;
    }

    private void Update() {
        animator.SetFloat(enginePowerIndex, throttle.Abs() * engineAnimValScalar);
        if (rocketVolume) rocketVolume.volume = throttle.Abs() * volumeMultiplier;
        bodyRend.flipX = horizontalSpeed < 0f;
    }

    void OnEnable() {
        controls.Enable();
    }

    private void OnDisable() {
        controls.Disable();
        SceneManager.LoadScene(0);
    }

    void OnReceiveThrottleAdjustment(float newValue) {
        throttle = Mathf.Clamp01(newValue);
    }

    void OnReceiveMousePosition(Vector2 screenPos) {
        Vector2 point = Camera.main.ScreenToWorldPoint(screenPos);
        UpdateArmRotation(point);
    }
}
