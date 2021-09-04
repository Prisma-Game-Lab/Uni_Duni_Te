using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillBehaviour : MonoBehaviour
{
    private CharStats charStats;

    //Os vetores abaixo irao conter informacoes pertinentes para a manipulacao das habilidades -Arthur 
    public SkillBase[] skills = new SkillBase[2];
    private float[] cooldowns = new float[2];

    private float[] activeTimes = new float[2];
    

    private void Awake() 
    {
        charStats = GetComponent<CharStats>();
    }

    public void SetUp()
    {
        skills[0] = charStats.GetCombatSkill();
        skills[1] = charStats.GetExplorationSkill();

        for(int i = 0; i < 2; i++)
        {
            cooldowns[i] = skills[i].skillCD;
            activeTimes[i] = skills[i].activeTime;
        }
    }
    
    private void Start() 
    {
        //Iniciando os vetores
        SetUp();
    }

    private void Update() 
    {
        foreach(SkillBase skill in skills)
        {
            int index = System.Array.IndexOf(skills, skill);
            switch(skill.state)
            {
                case "active":
                    if(activeTimes[index] > 0)
                    {
                        activeTimes[index] -= Time.deltaTime;
                    }
                    else
                    {
                        skill.state = "cooldown";
                        cooldowns[index] = skill.skillCD;
                    }
                break;
                case "cooldown":
                    if(cooldowns[index] > 0)
                    {
                        cooldowns[index] -= Time.deltaTime;
                    }
                    else
                    {
                        skill.state = "ready";
                    }
                break;
            }
        }
    }

    public void ActivateSkill(SkillBase skill)
    {
        if(skill.state == "ready")
        {
            skill.TriggerSkill(gameObject);
            skill.state = "active";
            activeTimes[System.Array.IndexOf(skills, skill)] = skill.activeTime;
        }
    }


}
