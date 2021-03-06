using UnityEngine;

[CreateAssetMenu(fileName = "PiratePaperBall", menuName = "BasicAttacks/PiratePaperBall")]
public class PirateBasicAttackBehaviour : SkillBase
{
	[SerializeField] private GameObject paperBallPrefab;
	[SerializeField] private float min_speed;
	[SerializeField] private float max_speed;
	[SerializeField] private float max_load_time;
	[SerializeField] private float SlowMultiplier = 0.1f;

	private float start = 0.0f;

	public override void TriggerSkill()
	{
		if (start != 0.0f) return;
		PlayerMovementBehaviour pMovementBehaviour = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementBehaviour>();
		pMovementBehaviour.SetSpeed(pMovementBehaviour.GetSpeed()*SlowMultiplier);
		start = Time.time;

	}

	public override void StopSkill()
	{
		float diff = Time.time - start;
		float ratio = diff / max_load_time;
		if (ratio > 1.0f) ratio = 1.0f;

		float speed = ratio * (max_speed - min_speed) + min_speed;


		GameObject player = GameObject.FindGameObjectWithTag("Player");
		GameObject paperBall = Instantiate(paperBallPrefab, player.transform.position,player.transform.rotation) as GameObject;
		paperBall.SendMessage("Init", speed);

		start = 0.0f;
		player.GetComponent<PlayerMovementBehaviour>().ResetSpeed();
	}


}
