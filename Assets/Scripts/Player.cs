using UnityEngine;

public class Player : MonoBehaviour
{
    public float thrustSpeed;
    public float turnSpeed;
    public float fireRate;
	public Missile missilePrefab;

	private Rigidbody2D _rigidBody;
    
    private bool _thrust;
    private bool _turnRight;
    private bool _turnLeft;
    private bool _fire;
	private float _nextFire;

	private void Awake()
	{
        _nextFire = 0.0f;
		_rigidBody = GetComponent<Rigidbody2D>();
	}

    // Update is called once per frame
    private void Update()
    {
        _thrust = Input.GetKey(KeyCode.W);
        _turnRight = Input.GetKey(KeyCode.D);
		_turnLeft = Input.GetKey(KeyCode.A);
        _fire = Input.GetKey(KeyCode.Space);
	}

	private void FixedUpdate()
	{
		if(_thrust) {
            _rigidBody.AddForce(transform.up * this.thrustSpeed);
		}


        if (!(_turnLeft && _turnRight))
        {
            if (_turnRight)
            {
                _rigidBody.MoveRotation(_rigidBody.rotation - this.turnSpeed);
            }
            if (_turnLeft)
            {
                _rigidBody.MoveRotation(_rigidBody.rotation + this.turnSpeed);
            }
        }

        if(_fire && Time.time > _nextFire)
        {
			_nextFire = Time.time + this.fireRate;
            Shoot();
		}
	}

    private void Shoot()
    {
		Missile missile = Instantiate(this.missilePrefab, transform.position, transform.rotation);
		missile.Fire(transform.up);
	}
}
