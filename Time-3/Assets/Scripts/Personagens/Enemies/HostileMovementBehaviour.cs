using UnityEngine;

public class HostileMovementBehaviour : MonoBehaviour
{
	[SerializeField] private float rotationSpeed = 5.0f;

	private CharStats charStats;
	private Rigidbody2D rb2D;

	private float speed;
	private Quaternion targetDir;
	private Vector3 targetPos;

	private void Awake()
	{
		charStats = GetComponent<CharStats>();
		rb2D = GetComponent<Rigidbody2D>();
	}

	private void Start()
	{
		speed = charStats.GetSpeed();
		targetPos = transform.position;
		targetDir = transform.rotation;
	}

	public void setRotSpeed(float rotationSpeed)
	{
		if (speed >= 0) {
			this.rotationSpeed = rotationSpeed;
		}
	}

	public float getRotSpeed()
	{
		return rotationSpeed;
	}

	public void UpdateMovement()
	{
		speed = charStats.GetSpeed();
		rb2D.MovePosition(Vector2.MoveTowards(transform.position, targetPos, speed * Time.fixedDeltaTime));
		transform.rotation = Quaternion.Lerp(transform.rotation, targetDir, rotationSpeed * Time.fixedDeltaTime);
	}

	public void MoveTo(Vector3 dest) => targetPos = dest;

	public void Face(float angle) => targetDir = Quaternion.AngleAxis(angle, Vector3.forward);

	public void Face(Vector3 target)
	{
		Vector2 lookDirection = target - transform.position;
		float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
		Face(angle);
	}

	public void FaceRandom()
	{
		float randomAngle = Random.Range(0.0f, 360.0f);
		Face(randomAngle);
	}

}
