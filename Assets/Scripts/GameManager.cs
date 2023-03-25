using UnityEngine;

public class GameManager : MonoBehaviour
{
	[SerializeField] private Player player;
	[SerializeField] private ParticleSystem explosion;

	[SerializeField] private TMPro.TMP_Text scoreText;
	[SerializeField] private TMPro.TMP_Text livesText;

	[SerializeField] private float respawnTime = 3.0f;
	[SerializeField] private float respawnInvulnerabilityTime = 3.0f;
	[SerializeField] private int maxLives = 4;

	public int lives { get; private set; }
	public int score { get; private set; }

	private void Start()
	{
		NewGame();
	}

	public void AsteroidDestroyed(Asteroid asteroid)
	{
		this.explosion.Play();
		this.explosion.transform.position = asteroid.transform.position;

		if (asteroid.size < 0.75f) {
			SetScore(this.score + 100);
		}
		else if (asteroid.size < 1.0f) {
			SetScore(this.score + 50);
		}
		else {
			SetScore(this.score + 25);
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
		/* Destroy all existing asteroids in the scene */
		Asteroid[] asteroids = FindObjectsOfType<Asteroid>();
		for (int i = 0; i < asteroids.Length; i++)
		{
			Destroy(asteroids[i].gameObject);
		}

		SetScore(0);
		SetLives(this.maxLives);
		Respawn();
	}

	private void GameOver()
	{
		Invoke(nameof(NewGame), this.respawnTime);
	}

	private void SetScore(int score)
	{
		this.score = score;
		this.scoreText.text = score.ToString();
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
