using UnityEngine;
using UnityEngine.Events;

public class Asteroid : MonoBehaviour
{
    public Sprite[] sprites;
    public AudioClip largeExplosionAudio;
	public AudioClip mediumExplosionAudio;
	public AudioClip smallExplosionAudio;

	public float size = 1.0f;
    public float minSize = 0.25f;
    public float maxSize = 1.5f;
    public float speed = 10.0f;
    //public float maxLifetime = 50.0f;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidBody;
	private Camera _camera;

    private bool _becameVisible;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody = GetComponent<Rigidbody2D>();
		_camera = FindObjectOfType<Camera>();
		_becameVisible = false;
	}

    private void Start()
    {
        _spriteRenderer.sprite = this.sprites[Random.Range(0, this.sprites.Length)];
        this.transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f);
        this.transform.localScale = Vector3.one * this.size;
        _rigidBody.mass = this.size;
    }

	private void Update()
	{
		if(_becameVisible == false)
		{
			float height = _camera.orthographicSize * 2;
			float width = height * _camera.aspect;

			float playerWidth = _spriteRenderer.bounds.size.x / 2;
			float playerHeight = _spriteRenderer.bounds.size.y / 2;

			/* X bounds */
			if ((transform.position.x < (width / 2) + playerWidth) &&
				(transform.position.x > (-width / 2) - playerWidth) &&
				(transform.position.y < (height / 2) + playerHeight) &&
				(transform.position.y > (-height / 2) - playerHeight))
			{
				Debug.Log("Became visible");
				_becameVisible = true;
			}
		}
		else
		{
			KeepWithinScreenBounds();
		}
	}

	public void SetTrajectory(Vector2 trajectory)
    {
        _rigidBody.AddForce(trajectory * this.speed);
        //Destroy(this.gameObject, maxLifetime);
    }

	private void KeepWithinScreenBounds()
	{
		float height = _camera.orthographicSize * 2;
		float width = height * _camera.aspect;

		float playerWidth = _spriteRenderer.bounds.size.x / 2;
		float playerHeight = _spriteRenderer.bounds.size.y / 2;

		/* X bounds */
		if (transform.position.x > (width / 2) + playerWidth)
		{
			transform.position = new Vector2(0 - (width / 2) - playerWidth, transform.position.y);
		}
		else if (transform.position.x < (-width / 2) - playerWidth)
		{
			transform.position = new Vector2(0 + (width / 2) + playerWidth, transform.position.y);
		}

		/* Y bounds */
		if (transform.position.y > (height / 2) + playerHeight)
		{
			transform.position = new Vector2(transform.position.x, 0 - (height / 2) - playerHeight);
		}
		else if (transform.position.y < (-height / 2) - playerHeight)
		{
			transform.position = new Vector2(transform.position.x, 0 + (height / 2) + playerHeight);
		}
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
        asteroid.size = this.size / 2.0f;
        asteroid.SetTrajectory(Random.insideUnitCircle.normalized * this.speed * 2);
	}

}
