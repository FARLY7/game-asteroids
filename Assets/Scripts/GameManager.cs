using UnityEngine;

public class GameManager : MonoBehaviour
{
	public Player player;
	public ParticleSystem explosion;
	
	public float respawnTime = 3.0f;
	public float respawnInvulnerabilityTime = 3.0f;
	public int lives = 3;
	public int score = 0;

	public void AsteroidDestroyed(Asteroid asteroid)
	{
		this.explosion.Play();
		this.explosion.transform.position = asteroid.transform.position;

		if (asteroid.size < 0.75f)
		{
			this.score += 100;
		}
		else if (asteroid.size < 1.0f)
		{
			this.score += 50;
		}
		else
		{
			this.score += 25;
		}
	}

	public void PlayerDied()
	{
		this.explosion.Play();
		this.explosion.transform.position = this.player.transform.position;

		this.lives--;

		if (this.lives <= 0)
		{
			GameOver();
		}
		else
		{
			Invoke(nameof(Respawn), this.respawnTime);
		}
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

	private void GameOver()
	{
		this.lives = 3;
		this.score = 0;
	}
}
