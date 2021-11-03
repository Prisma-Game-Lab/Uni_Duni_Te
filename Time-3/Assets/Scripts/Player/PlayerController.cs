using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

//Usar RequireComponent para adicionar componentes automaticamente ao jogador -Arthur
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CharStats))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PlayerMovementBehaviour))]
[RequireComponent(typeof(SwitchCharacter))]
[RequireComponent(typeof(PlayerSkillBehaviour))]
[RequireComponent(typeof(BasicAttackBehaviour))]
[RequireComponent(typeof(PlayerInventoryBehaviour))]
[RequireComponent(typeof(PlayerInteraction))]
public class PlayerController : MonoBehaviour, IDamageable<int>
{
	private HUDManager hudManager;
	private CharStats charStats;
	private PlayerMovementBehaviour pMovementBehaviour;
	private BasicAttackBehaviour pAttackBehaviour;
	private PlayerSkillBehaviour pSkillBehaviour;
	[SerializeField] private GameObject head;

	private void Awake()
	{
		charStats = GetComponent<CharStats>();
		pAttackBehaviour = GetComponent<BasicAttackBehaviour>();
		pMovementBehaviour = GetComponent<PlayerMovementBehaviour>();
		pSkillBehaviour = GetComponent<PlayerSkillBehaviour>();
		hudManager = GameObject.FindWithTag("HUD").GetComponent<HUDManager>();
	}

	public void Die()
	{
		// TODO: Animacao de morte
		// TODO: Respawn/Restart
		Debug.Log("Player morreu!");
		Scene scene = SceneManager.GetActiveScene();
		SceneManager.LoadScene(scene.name);
	}

	public void Heal()
	{
		charStats.SetCurrHp(charStats.GetMaxHp());
		if (hudManager != null)
			hudManager.setHealthNormalised(1.0f);
	}

	public void ApplyHealing(int healing)
	{
		charStats.IncCurrHp(healing);
		if (hudManager != null)
			hudManager.setHealth(charStats.GetCurrHp(), charStats.GetMaxHp());
	}

	public bool ApplyDamage(int damage)
	{
		damage = (int)(damage * (1.0f - charStats.GetDefense()));
		charStats.IncCurrHp(-damage);
		if (charStats.GetCurrHp() <= 0) {
			Die();
			return true;
		}

		if (hudManager != null)
			hudManager.setHealth(charStats.GetCurrHp(), charStats.GetMaxHp());
		return false;
	}

	/// Evento ativado pelo InputSystem para movimentacao
	public void onMovement(InputAction.CallbackContext context)
	{
		// Le o vetor de movimento bruto passado pelo InputSystem
		Vector2 rawMovementVector = context.ReadValue<Vector2>();
		pMovementBehaviour.UpdateMovementVec(rawMovementVector);
	}

	/// Evento ativado pelo InputSystem para ataques basicos
	public void onAttack(InputAction.CallbackContext context)
	{
		if (context.started) { // Press
			pAttackBehaviour.BasicAttack(true);
			
		} else if (context.canceled) { // Release
			pAttackBehaviour.BasicAttack(false);
			pMovementBehaviour.ResetSpeed();
		}
	}
	private IEnumerator Slowdown(){
		//Velocidade do player quando usa o atque básico
		pMovementBehaviour.SetSpeed(0.5f);
		//Delay com essa velocidade
		yield return new WaitForSeconds(0.4f);
	}
	public void onCombatSkillActivation(InputAction.CallbackContext context)
	{
		if (context.started){}
			StartCoroutine(Slowdown());
			pSkillBehaviour.ActivateSkill(charStats.GetCombatSkill());
	}

	public void onExplorationSkillActivation(InputAction.CallbackContext context)
	{
		if (context.started)
			pSkillBehaviour.ActivateSkill(charStats.GetExplorationSkill());
	}

	public void UpdateStats()
	{
		pMovementBehaviour.SetUp();
		pAttackBehaviour.SetUp();
		pSkillBehaviour.SetUp();
	}
	public void auxSlowdown(){
		Debug.Log("slowdown");
		StartCoroutine(Slowdown());
	}
}
