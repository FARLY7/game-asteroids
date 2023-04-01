using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private AsteroidSpawner _asteroidSpawner;

    // Start is called before the first frame update
    void Start()
    {
        _asteroidSpawner.Spawn();
    }

    // Update is called once per frame
    void Update()
    {
		if(Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))
        {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}
	}
}
