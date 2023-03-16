using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawnManager : MonoBehaviour
{
	public GameObject prefabToSpawn;

	public int width = 1000;
	public int length = 1000;
	public int height = 1000;
	public int step = 100;
	public int randomFactor = 3;

	[SerializeField] private GameObject _asteroidContainer = default;

	private void Start()
	{
		//SpawnAsteroids();
	}

	void SpawnAsteroids()
    {		
		for (int h = 0; h < length / step; h++)
		{
			for (int i = 0; i < width / step; i++)
			{
				int canSpawn = Random.Range(0, randomFactor);
				if (canSpawn == 0)
				{
					GameObject asteriod = Instantiate(prefabToSpawn, new Vector3(((i * step) + transform.position.x) - (width / 2),
						transform.position.y + (Random.Range(-(height / 2), 
						(height / 2))), ((h * step) + transform.position.z) - (length / 2)), 
						Quaternion.Euler(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360))));

					asteriod.transform.SetParent(_asteroidContainer.transform);
				}
			}
		}

		Debug.Log(_asteroidContainer.transform.childCount);
	}
}
