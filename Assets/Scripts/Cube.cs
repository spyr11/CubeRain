using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Cube : MonoBehaviour
{
    private Renderer _renderer;
    private Color _defaultColor;
    private int _destroyMinValue;
    private int _destroyMaxValue;
    private bool _hasHit;

    public event Action<Cube> Disabled;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();

        _defaultColor = _renderer.material.color;

        _destroyMinValue = 2;
        _destroyMaxValue = 6;
    }

    private void OnEnable()
    {
        _hasHit = false;

        SetDefaultColor();
    }

    private void OnDisable()
    {
        Disabled?.Invoke(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent<Platform>(out _))
        {
            if (_hasHit == false)
            {
                _hasHit = true;

                StartCoroutine(OnHit());
            }
        }
    }

    private IEnumerator OnHit()
    {
        ChangeColor();

        float destroyDelay = UnityEngine.Random.Range(_destroyMinValue, _destroyMaxValue);

        yield return new WaitForSeconds(destroyDelay);

        gameObject.SetActive(false);
    }

    private void ChangeColor()
    {
        _renderer.material.color = UnityEngine.Random.ColorHSV(); ;
    }

    private void SetDefaultColor()
    {
        _renderer.material.color = _defaultColor;
    }
}
