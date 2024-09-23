using UnityEngine;

[ExecuteInEditMode]
public class Wobble : MonoBehaviour
{
    [SerializeField]
    float _maxWobble = 0.03f;
    
    [SerializeField]
    float _wobbleSpeed = 1f;
    
    [SerializeField]
    float _recovery = 1f;
    
    Renderer _rend;
    Vector3 _lastPos;
    Vector3 _velocity;
    Vector3 _lastRot;  
    Vector3 _angularVelocity;
    
    float _wobbleAmountX;
    float _wobbleAmountZ;
    float _wobbleAmountToAddX;
    float _wobbleAmountToAddZ;
    float _pulse;
    float _time = 0.5f;
    int _wobbleXPropertyId;
    int _wobbleZPropertyId;
    
    // Use this for initialization
    void Start()
    {
        _rend = GetComponent<Renderer>();
        _wobbleXPropertyId = Shader.PropertyToID("_WobbleX");
        _wobbleZPropertyId = Shader.PropertyToID("_WobbleZ");
        _lastPos = transform.position;
        _lastRot = transform.rotation.eulerAngles;
    }

    void Update()
    {
        if (_rend == null) return;
        
        _time += Time.deltaTime;
        // decrease wobble over time
        _wobbleAmountToAddX = Mathf.Lerp(_wobbleAmountToAddX, 0, Time.deltaTime * (_recovery));
        _wobbleAmountToAddZ = Mathf.Lerp(_wobbleAmountToAddZ, 0, Time.deltaTime * (_recovery));

        // make a sine wave of the decreasing wobble
        _pulse = 2 * Mathf.PI * _wobbleSpeed;
        _wobbleAmountX = _wobbleAmountToAddX * Mathf.Sin(_pulse * _time);
        _wobbleAmountZ = _wobbleAmountToAddZ * Mathf.Sin(_pulse * _time);

        // send it to the shader
        if (Application.isPlaying)
        {
            _rend.material.SetFloat(_wobbleXPropertyId, _wobbleAmountX);
            _rend.material.SetFloat(_wobbleZPropertyId, _wobbleAmountZ);
        }
        else
        {
            _rend.sharedMaterial.SetFloat(_wobbleXPropertyId, _wobbleAmountX);
            _rend.sharedMaterial.SetFloat(_wobbleZPropertyId, _wobbleAmountZ);
        }
        
        // velocity
        _velocity = (_lastPos - transform.position) / Time.deltaTime;
        _angularVelocity = transform.rotation.eulerAngles - _lastRot;
        
        // add clamped velocity to wobble
        _wobbleAmountToAddX += Mathf.Clamp((_velocity.x + (_angularVelocity.z * 0.2f)) * _maxWobble, -_maxWobble, _maxWobble);
        _wobbleAmountToAddZ += Mathf.Clamp((_velocity.z + (_angularVelocity.x * 0.2f)) * _maxWobble, -_maxWobble, _maxWobble);

        // keep last position
        _lastPos = transform.position;
        _lastRot = transform.rotation.eulerAngles;
    }
}