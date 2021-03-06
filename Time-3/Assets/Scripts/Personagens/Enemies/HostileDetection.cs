using UnityEngine;

public class HostileDetection : MonoBehaviour, IHostileDetection
{
	private HostileStats hostileStats;
	[SerializeField] private GameObject player;

	// TODO: base on stats
	[SerializeField] private LayerMask ignoreMask;

	private float fov;
	private float viewDistance;
	private float hearingDistance;

	private LayerMask raycastLayers;

	void Awake()
	{
		hostileStats = GetComponent<HostileStats>();

		// Ignore own mask
		raycastLayers = ~(ignoreMask | (1 << gameObject.layer));
		fov = hostileStats.GetFOV();
		hearingDistance = hostileStats.GetHearingDistance();
		viewDistance = hostileStats.GetViewDistance();

	}

	private void Start() 
	{
		if (player == null) {
			player = GameObject.FindGameObjectWithTag("Player");
		}
	}

	public bool isPlayerDetectable()
	{
		if(player != null)
		{
			Vector3 pPosition = player.GetComponentInParent<Transform>().position;

			// Player within distance?
			float distance = Vector2.Distance(pPosition, transform.position);

			if (distance <= viewDistance || distance <= hearingDistance) {

				// Player within fov?
				float angle = Vector2.Angle(pPosition - transform.position, transform.right);
				if (angle < fov/2 || distance <= hearingDistance) {

					// Player visible?
					RaycastHit2D hit = Physics2D.Raycast(
							transform.position,
							(pPosition - transform.position).normalized,
							distance, raycastLayers);
					Debug.DrawRay(transform.position, (pPosition - transform.position).normalized * distance, Color.red, 1);

					if (hit.collider.tag == "Player") {
						return true;
					}
				}
			}
		}

		return false;
	}
}
