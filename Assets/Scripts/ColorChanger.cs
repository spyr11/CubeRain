using UnityEngine;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Cube))]
public class ColorChanger : MonoBehaviour
{
    private Renderer _renderer;

    private Cube _cube;

    private Color _defaultColor;

    private bool _isChanged;

    void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _cube = GetComponent<Cube>();

        _defaultColor = _renderer.material.color;

        _isChanged = false;
    }

    void OnEnable()
    {
        SetDefault();

        _cube.Hit += OnHit;
    }

    void OnDisable()
    {
        _cube.Hit -= OnHit;
    }

    private void OnHit(Cube target)
    {
        TryChange();
    }

    private void TryChange()
    {
        if (_isChanged == false)
        {
            _renderer.material.color = Random.ColorHSV(); ;
        }

        _isChanged = true;
    }

    private void SetDefault()
    {
        if (_renderer.material.color != _defaultColor)
        {
            _renderer.material.color = _defaultColor;

            _isChanged = false;
        }
    }
}