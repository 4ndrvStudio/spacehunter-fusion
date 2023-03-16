using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Random theo thoi gian xuat hien tung mo dao`.
///  level 1: 30s;
///  level 2: 60s;
///  level 3: 120s;
///  Neu cho trong^' con` it' uu tien xuat hien mo~ dao` level cao khi dung' moc' thoi` gian
/// </summary>
public class ResourcesSpawner : MonoBehaviour
{

    //[SerializeField] private Transform[] _spawnerPoints;
    //[SerializeField] private MineralClass[] _minerals;
    //[SerializeField] private int _time;
    //[SerializeField] private int _spawnTime = 30;
    //[SerializeField] private int _spawnFactor = 1;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    InitialSpawnResources();
    //    StartCoroutine(TimeCalculate());
    //}

    //// set Initial Spawn
    //private void InitialSpawnResources()
    //{
    //    foreach (Transform _spawnerPoint in _spawnerPoints)
    //    {
    //        //turn off mesh 
    //        _spawnerPoint.GetComponent<MeshRenderer>().enabled = false;
    //        //random
    //        int _randomNum = Random.Range(1, 100);
    //        int _level = _randomNum < 55 ? 1 : _randomNum > 85 ? 3 : 2; // change Radom rate here
    //        SpawnResource(_spawnerPoint, _level);

    //    }
    //}
    ////spawn with time
    //private void SpawnWithTime()
    //{
    //    if (CheckEmptyPos().Count == 0) return;
    //    for (int i = _spawnFactor; i < 0; i--)
    //    {
    //        if (i == 3) continue; // moc thoi gian 90s chi~ spawn level 1 va 2;
    //        SpawnResource(CheckEmptyPos()[Random.Range(0, CheckEmptyPos().Count)], i);
    //        if (CheckEmptyPos().Count == 0) break; //break if empty Pos - can't spawn;
    //    }
    //}
    

    //private void SpawnResource(Transform _spawnerPoint, int _level)
    //{
    //    _spawnerPoint.transform.gameObject.SetActive(true);
    //    Bounds _bounds = _spawnerPoint.GetComponent<BoxCollider>().bounds;
    //    float _offsetX = Random.Range(-_bounds.extents.x, _bounds.extents.x);
    //    float _offsetZ = Random.Range(-_bounds.extents.z, _bounds.extents.z);
    //    Vector3 _randomRangeInPoint = new Vector3(_spawnerPoint.transform.position.x + _offsetX, _spawnerPoint.position.y, _spawnerPoint.transform.position.z + _offsetZ);

    //    if (_level == 4) _level = 3; // change time factor to level;
    //    foreach (MineralClass _mineral in _minerals)
    //    {
    //        if (_mineral.Type == (MineralType)_level)
    //        {
    //            GameObject _newMineral = Instantiate(_mineral.Prefab, _spawnerPoint.position, _mineral.Prefab.transform.localRotation);
    //            _newMineral.transform.parent = _spawnerPoint.gameObject.transform;
    //            _newMineral.transform.position = _randomRangeInPoint;
    //        }
    //    }
    //}

    //// Check Empty Pos 
    //private List<Transform> CheckEmptyPos()
    //{
    //    //get pos not spawned;
    //    List<Transform> _posCanSpawn = new List<Transform>();
    //    foreach (Transform _spawnerPoint in _spawnerPoints)
    //    {
    //        if (!_spawnerPoint.transform.gameObject.activeSelf)
    //        {
    //            _posCanSpawn.Add(_spawnerPoint);
    //        };
    //    }
    //    return _posCanSpawn;
    //}

    //// Test Time;
    //public IEnumerator TimeCalculate()
    //{
    //    bool _timeCal = true;
    //    while (_timeCal)
    //    {
    //        yield return new WaitForSeconds(1);
    //        if (_time >= 0)
    //        {
    //            _time++;
    //        }
    //        // spawn;
    //        if (_time >= _spawnTime)
    //        {
    //            SpawnWithTime();
    //            //Factor time
    //            if (_spawnFactor >= 4)
    //            {
    //                _spawnFactor = 1;
    //            }
    //            else _spawnFactor++;
    //            _time = 0;
    //        }
    //    }
    //}

}