using UnityEngine;
using UnityEngine.Pool;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private Collider _floor;
    [SerializeField] private Cube _cube;
    [SerializeField, Range(0, int.MaxValue)] private float _spawnDelay;

    private int _poolCapacity;
    private int _poolMaxSize;
    private ObjectPool<Cube> _pool;
    private Vector3 _spawnArea;

    private void Awake()
    {
        _poolCapacity = 10;
        _poolMaxSize = 10;

        _pool = new ObjectPool<Cube>(
               createFunc: () => Instantiate(_cube),
               actionOnGet: (cube) => ActionOnGet(cube),
               actionOnDestroy: (cube) => Destroy(cube.gameObject),
               defaultCapacity: _poolCapacity,
               maxSize: _poolMaxSize
           );
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetObject), 0f, _spawnDelay);
    }

    private void GetObject()
    {
        _pool.Get();
    }

    private void ActionOnGet(Cube cube)
    {
        cube.Hit += OnHit;

        cube.gameObject.SetActive(true);
        cube.gameObject.transform.position = GetPosition();
    }

    private void OnHit(Cube cube)
    {
        cube.Hit -= OnHit;

        _pool.Release(cube);
    }

    private Vector3 GetPosition()
    {
        Bounds bound = _floor.bounds;

        float positionX = Random.Range(bound.min.x, bound.max.x);
        float positionZ = Random.Range(bound.min.z, bound.max.z);

        return new Vector3(positionX, transform.position.y, positionZ);
    }
}