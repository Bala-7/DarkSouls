public class EnemyChargeState : EnemyState
{
    protected float currentTimer = 0;

    public override void Enter(Enemy enemy)
    {
        _player = ThirdPersonControllerMovement.s;
        enemy.StopMovement();
        enemy.PlayAnimation("Charge");
        currentTimer = 0f;

        // TODO: Spawn bullet
    }

    public override EnemyState HandleInput(Enemy enemy, ref PlayerInput input)
    {
        throw new System.NotImplementedException();
    }

    public override EnemyState Update(Enemy enemy)
    {
        return null;
    }
}