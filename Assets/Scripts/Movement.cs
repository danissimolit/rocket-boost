using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class Movement : MonoBehaviour
{
    // TODO : Make a lighter in rocket prefab!)

    [SerializeField] private float _boostForce = 70f;
    [SerializeField] private float _maxSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private AudioClip _engineSound;
    [SerializeField] private float _engineSoundVolume = 0.1f;
    [SerializeField] private bool _isBoostActive = false;
    [SerializeField] private ParticleSystem _leftFrontEngineParticle;
    [SerializeField] private ParticleSystem _leftBackEngineParticle;
    [SerializeField] private ParticleSystem _rightFrontEngineParticle;
    [SerializeField] private ParticleSystem _rightBackEngineParticle;
    
    private PlayerInput _playerInput;
    private Rigidbody _rb;
    private AudioSource _audio;

    private event Action _onBoost;
    
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rb = GetComponent<Rigidbody>();
        _audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        BoostInput();
        RotateInput();
    }
    private void BoostInput()
    {
        if (_playerInput.actions["Boost"].IsPressed())
        {
            _onBoost += Boost;
        }

        BoostFX();
    }

    private void BoostFX()
    {
        if (_playerInput.actions["Boost"].triggered)
        {
            _audio.PlayOneShot(_engineSound, _engineSoundVolume);
            _isBoostActive = true;
            PlayAllEngineParticles();
        }
        else if (_playerInput.actions["Boost"].WasReleasedThisFrame())
        {
            _audio.Stop();
            _isBoostActive = false;
            StopAllEnginePartilces();   
        }
    }

    private void RotateInput()
    {
        Vector2 input = _playerInput.actions["Move"].ReadValue<Vector2>();
        RotateFX(input.x);
        Rotate(input.x);

        //Debug.Log(input);
    }

    private void RotateFX(float input)
    {
        if (_isBoostActive)
        {
            return;
        }

        if (input > 0)
        {
            PlayLeftEnginesParticles();
            StopRightEnginesParticles();
        }
        else if (input < 0)
        {
            PlayRightEnginesParticles();
            StopLeftEnginesParticles();
        }
        else
        {
            StopRightEnginesParticles();
            StopLeftEnginesParticles();
        }
    }
    

    private void Rotate(float input)
    {
        if (input != 0)
        {
            Vector3 direction = input < 0 ? Vector3.forward : -Vector3.forward;

            _rb.freezeRotation = true;

            transform.Rotate(direction * _rotationSpeed * Time.deltaTime);

            _rb.constraints = RigidbodyConstraints.FreezeRotationY
                            | RigidbodyConstraints.FreezeRotationX
                            | RigidbodyConstraints.FreezePositionZ;
        }
    }

    private void FixedUpdate()
    {
        _onBoost?.Invoke();
        LimitVelocity();
    }

    private void Boost()
    {
        _rb.AddRelativeForce(Vector3.up * _boostForce * Time.fixedDeltaTime, ForceMode.Force);
        _onBoost -= Boost;
    }

    private void LimitVelocity()
    {
        //Debug.Log(_rb.velocity.y);

        Vector3 velocity = _rb.velocity;

        float gravityY = velocity.y < 0 ? velocity.y : 0f;
        if (gravityY < 0) velocity.y = 0;

        if (velocity.magnitude >= _maxSpeed)
        {
            Debug.Log("Speed limit!");

            velocity = Vector3.ClampMagnitude(velocity, _maxSpeed);

            if (gravityY < 0)
            {
                velocity.y = gravityY;
            }

            _rb.velocity = velocity;
        }
    }


    private void PlayAllEngineParticles()
    {
        _leftFrontEngineParticle.Play();
        _rightFrontEngineParticle.Play();
        _leftBackEngineParticle.Play();
        _rightBackEngineParticle.Play();
    }

    private void StopAllEnginePartilces()
    {
        _leftFrontEngineParticle.Stop();
        _rightFrontEngineParticle.Stop();
        _leftBackEngineParticle.Stop();
        _rightBackEngineParticle.Stop();
    }

    private void PlayLeftEnginesParticles()
    {
        if (!_leftFrontEngineParticle.isPlaying)
        {
            _leftFrontEngineParticle.Play();
            _leftBackEngineParticle.Play();
        }
    }

    private void PlayRightEnginesParticles()
    {
        if (!_rightFrontEngineParticle.isPlaying)
        {
            _rightFrontEngineParticle.Play();
            _rightBackEngineParticle.Play();
        }
    }

    private void StopLeftEnginesParticles()
    {
        if (_leftFrontEngineParticle.isPlaying)
        {
            _leftFrontEngineParticle.Stop();
            _leftBackEngineParticle.Stop();
        }
    }

    private void StopRightEnginesParticles()
    {
        if (_rightFrontEngineParticle.isPlaying)
        {
            _rightFrontEngineParticle.Stop();
            _rightBackEngineParticle.Stop();
        }
    }
}
