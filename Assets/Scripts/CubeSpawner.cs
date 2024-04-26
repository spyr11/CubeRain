using System.Collections;
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
        StartCoroutine(SpawnObject(_spawnDelay));
    }

    private IEnumerator SpawnObject(float delay)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(delay);

        bool isExecute = true;

        while (isExecute)
        {
            _pool.Get();

            yield return waitForSeconds;
        }
    }

    private void ActionOnGet(Cube cube)
    {
        cube.Disabled += OnDisabled;

        cube.gameObject.SetActive(true);
        cube.gameObject.transform.position = GetPosition();
    }

    private void OnDisabled(Cube cube)
    {
        cube.Disabled -= OnDisabled;

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