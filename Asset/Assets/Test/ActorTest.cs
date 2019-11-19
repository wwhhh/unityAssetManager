using UnityEngine;
using ActorCore;
using Asset;

public class ActorTest : MonoBehaviour
{

    SkillConfig config;
    Actor actor;
    Actor enemy;

    void Start()
    {
        actor = ActorManager.I.AddActorAuthority();
        actor.stateController.ChangeStateIdle();

        enemy = ActorManager.I.AddActorEnemy();
        enemy.stateController.ChangeStateIdle();
    }

    [EasyButtons.Button("Attack")]
    void Attack()
    {
        actor.skillController.SkillId = 0;
        actor.stateController.ChangeStateSkill();
    }

    [EasyButtons.Button("Idle")]
    void Idle()
    {
        actor.Play("unarmed-idle", false);
    }

    [EasyButtons.Button("Hit")]
    void Hit()
    {
        actor.stateController.ChangeStateHit();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.J))
        {
            Attack();
        }
    }

}