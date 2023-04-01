using UnityEngine;
//using UnityEngine.Device;


public class Player : MonoBehaviour
{
    public float thrustSpeed;
    public float turnSpeed;
    public float fireRate;
	public Missile missilePrefab;

	private Rigidbody2D _rigidBody;
	private AudioSource _audioSource;
	private SpriteRenderer _spriteRenderer;
	private Camera _camera;

	public AudioClip thrustAudio;
	public AudioClip fireAudio;
    
    private bool _thrust;
    private bool _turnRight;
    private bool _turnLeft;
    private bool _fire;
	private float _nextFire;

	
	private void Awake()
	{
        _nextFire = 0.0f;
		_rigidBody = GetComponent<Rigidbody2D>();
		_audioSource = GetComponent<AudioSource>();
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_camera = FindObjectOfType<Camera>();
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
			transform.position = new Vector2(0 - (width/2) - playerWidth, transform.position.y);
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

	private void Update()
    {
        _thrust     = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        _turnRight  = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
		_turnLeft   = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        _fire       = Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0);

		if (_fire && Time.time > _nextFire)
		{
			_nextFire = Time.time + this.fireRate;
			Shoot();
		}

		KeepWithinScreenBounds();
	}

	private void FixedUpdate()
	{
		if(_thrust)
		{
            _rigidBody.AddForce(transform.up * this.thrustSpeed);
			_audioSource.PlayOneShot(this.thrustAudio, 0.25f);
		}

        if (!(_turnLeft && _turnRight))
        {
            if (_turnRight)
            {
                _rigidBody.AddTorque(-this.turnSpeed);
            }
            if (_turnLeft)
            {
				_rigidBody.AddTorque(this.turnSpeed);
            }
        }
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.CompareTag("Asteroid"))
		{
			_rigidBody.velocity = Vector2.zero;
			_rigidBody.angularVelocity = 0.0f;
			this.gameObject.SetActive(false);

			FindObjectOfType<GameManager>().PlayerDied();
		}
	}

	private void Shoot()
    {
		Missile missile = Instantiate(this.missilePrefab, transform.position, transform.rotation);

		//float mag = _rigidBody.velocity.magnitude;
		//if(mag < 1)
		//{
		//	mag = 1.0f;
		//}

		missile.Fire(transform.up, _rigidBody.velocity);

		//Debug.Log($"Vector: {_rigidBody.velocity.}");

		_audioSource.PlayOneShot(this.fireAudio, 0.04f);
	}
}
