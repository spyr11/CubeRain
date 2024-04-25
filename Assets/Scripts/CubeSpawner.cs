using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private Collider _floor;
    [SerializeField] private Cube _cube;
    [SerializeField, Range(0, int.MaxValue)] private float _spawnDelay;

    private int _destroyMinValue;
    private int _destroyMaxValue;
    private int _poolCapacity;
    private int _poolMaxSize;

    private ObjectPool<GameObject> _pool;

    private Vector3 _spawnArea;

    private void Awake()
    {
        _destroyMinValue = 2;
        _destroyMaxValue = 6;
        _poolCapacity = 10;
        _poolMaxSize = 10;

        _pool = new ObjectPool<GameObject>(
               createFunc: () => Instantiate(_cube.gameObject),
               actionOnGet: (cube) => ActionOnGet(cube),
               actionOnRelease: (cube) => cube.SetActive(false),
               actionOnDestroy: (cube) => Destroy(cube),
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

    private void ActionOnGet(GameObject cubeObject)
    {
        Cube cube = cubeObject.GetComponent<Cube>();
        cube.Hit += OnHit;

        cubeObject.SetActive(true);
        cubeObject.transform.position = SetArea();
    }

    private void OnHit(Cube cube)
    {
        StartCoroutine(DisableOnDelay(cube));
    }

    private IEnumerator DisableOnDelay(Cube cube)
    {
        cube.Hit -= OnHit;

        float destroyDelay = Random.Range(_destroyMinValue, _destroyMaxValue);

        yield return new WaitForSeconds(destroyDelay);

        _pool.Release(cube.gameObject);
    }

    private Vector3 SetArea()
    {
        Bounds bound = _floor.bounds;

        float positionX = Random.Range(bound.min.x, bound.max.x);
        float positionZ = Random.Range(bound.min.z, bound.max.z);

        return new Vector3(positionX, transform.position.y, positionZ);
    }
}
