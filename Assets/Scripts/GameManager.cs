using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[SerializeField] private Player player;
	[SerializeField] private AsteroidSpawner asteroidSpawner;
	[SerializeField] private ParticleSystem explosion;
	
	[SerializeField] private TMPro.TMP_Text scoreText;
	[SerializeField] private TMPro.TMP_Text livesText;
	[SerializeField] private TMPro.TMP_Text gameOverText;

	[SerializeField] private float respawnTime = 4.0f;
	[SerializeField] private float respawnInvulnerabilityTime = 3.0f;
	[SerializeField] private int maxLives = 4;
	[SerializeField] private int newLifeScore = 10000;
	[SerializeField] private int[] levelAsteroidAmount = new int[] { 4, 6, 8, 10, 11 };

	public int lives { get; private set; }
	public int score { get; private set; }

	private int _newLifeScoreCounter;
	private int _currentLevel;

	private void Start()
	{
		NewGame();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.Escape))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
		}
	}

	public void AsteroidDestroyed(Asteroid asteroid)
	{
		this.explosion.Play();
		this.explosion.transform.position = asteroid.transform.position;

		if (asteroid.size < asteroid.smallSize) {
			SetScore(this.score + 100);
		}
		else if (asteroid.size < asteroid.mediumSize) {
			SetScore(this.score + 50);
		}
		else {
			SetScore(this.score + 25);
		}

		Asteroid[] asteroids = FindObjectsOfType<Asteroid>();
		Debug.Log($"Number of asteroids: {asteroids.Length}");
		if (asteroids.Length == 1)
		{
			NextLevel();
		}
	}

	public void PlayerDied()
	{
		this.explosion.Play();
		this.explosion.transform.position = this.player.transform.position;

		SetLives(this.lives - 1);

		if (this.lives <= 0)
		{
			GameOver();
		}
		else
		{
			Invoke(nameof(Respawn), this.respawnTime);
		}
	}

	public void PlayerScored(int score)
	{
		this.score += score;
		this.scoreText.text = this.score.ToString();
	}

	private void NewGame()
	{
		gameOverText.gameObject.SetActive(false);

		/* Destroy all existing asteroids in the scene */
		Asteroid[] asteroids = FindObjectsOfType<Asteroid>();
		for (int i = 0; i < asteroids.Length; i++)
		{
			Destroy(asteroids[i].gameObject);
		}

		SetScore(0);
		SetLives(this.maxLives);
		Respawn();
		_newLifeScoreCounter = 0;
		_currentLevel = 0;
		asteroidSpawner.Spawn(levelAsteroidAmount[_currentLevel]);
	}

	private void NextLevel()
	{
		_currentLevel++;
		int asteroidSpawnAmount = 0;

		if(_currentLevel > (levelAsteroidAmount.Length-1))
		{
			asteroidSpawnAmount = levelAsteroidAmount[levelAsteroidAmount.Length-1];
		}
		else
		{
			asteroidSpawnAmount = levelAsteroidAmount[_currentLevel];
		}

		asteroidSpawner.Spawn(asteroidSpawnAmount);
		//asteroidSpawner.Invoke(nameof(asteroidSpawner.Spawn), this.respawnTime);
	}

	private void GameOver()
	{
		gameOverText.gameObject.SetActive(true);
		Invoke(nameof(NewGame), this.respawnTime);

		//Invoke(nameof(TurnOnCollisions), respawnInvulnerabilityTime);
		//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
	}

	private void SetScore(int score)
	{
		this.score = score;
		this.scoreText.text = score.ToString();

		if(this.score > 0)
		{
			/* Prevent divide by zero */
			if (this.score / this.newLifeScore > _newLifeScoreCounter)
			{
				/* If player has surpassed a score threshold that earns a new life */
				SetLives(this.lives + 1);
				_newLifeScoreCounter++;
			}
		}
		
	}

	private void SetLives(int lives)
	{
		this.lives = lives;
		this.livesText.text = lives.ToString();
	}

	private void Respawn()
	{
		this.player.transform.position = Vector2.zero;
		this.player.gameObject.layer = LayerMask.NameToLayer("IgnoreCollisions");
		this.player.gameObject.SetActive(true);

		Invoke(nameof(TurnOnCollisions), respawnInvulnerabilityTime);
	}

	private void TurnOnCollisions()
	{
		this.player.gameObject.layer = LayerMask.NameToLayer("Player");
	}
}
