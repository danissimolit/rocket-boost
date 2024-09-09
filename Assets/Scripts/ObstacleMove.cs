using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMove : MonoBehaviour
{
    private Vector3 _startingPos;
    [SerializeField] private Vector3 _movingPosition;
    [SerializeField] private float _period;

    private const float _tau = Mathf.PI * 2; 

    private void Start()
    {
        _startingPos = transform.position;
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (_period <= Mathf.Epsilon) return;

        float cycles = Time.time / _period; 

        float rawSinWave = Mathf.Sin(cycles * _tau);

        float movementFactor = SinWaweToFactor(rawSinWave);

        transform.position = _startingPos + _movingPosition * movementFactor;
    }


    private float SinWaweToFactor(float sinWave)
    {
        return (sinWave + 1f) / 2f;
    }
}
