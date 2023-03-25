using UnityEngine;
using UnityEngine.Events;

public class Asteroid : MonoBehaviour
{
    public Sprite[] sprites;
    public AudioClip largeExplosionAudio;
	public AudioClip mediumExplosionAudio;
	public AudioClip smallExplosionAudio;

	public float size = 1.0f;
    public float minSize = 0.5f;
    public float maxSize = 1.5f;
    public float speed = 7.0f;
    public float maxLifetime = 10.0f;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidBody;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
	}

    private void Start()
    {
        _spriteRenderer.sprite = this.sprites[Random.Range(0, this.sprites.Length)];
        this.transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f);
        this.transform.localScale = Vector3.one * this.size;
        _rigidBody.mass = this.size;
    }

    public void SetTrajectory(Vector2 trajectory)
    {
        _rigidBody.AddForce(trajectory * this.speed);
        Destroy(this.gameObject, maxLifetime);
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.CompareTag("Missile"))
        {
            if (this.size > 1.2f)
            {
                CreateSplit();
                CreateSplit();
                CreateSplit();
                AudioSource.PlayClipAtPoint(this.largeExplosionAudio, this.transform.position);
            }
            else if (this.size > 0.9f)
            {
                CreateSplit();
                CreateSplit();
				AudioSource.PlayClipAtPoint(this.mediumExplosionAudio, this.transform.position);
            }
            else
            {
				AudioSource.PlayClipAtPoint(this.smallExplosionAudio, this.transform.position);
            }

			FindObjectOfType<GameManager>().AsteroidDestroyed(this);
            Destroy(this.gameObject);
        }
	}

    private void CreateSplit()
    {
        Vector2 position = this.transform.position;
        position += Random.insideUnitCircle * 0.5f;

        Asteroid asteroid = Instantiate(this, position, this.transform.rotation);
        asteroid.size = this.size - 0.3f;
        asteroid.SetTrajectory(Random.insideUnitCircle.normalized * this.speed * 2);
	}

}
