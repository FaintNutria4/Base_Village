public class RabitHealth : MobHealth
{
    public void Start()
    {
        MobsStateManager.GetInstance().GetRabitsList().Add(transform);
    }
    internal override void Die(Agent_System_Manager killer)
    {

        MobsStateManager.GetInstance().GetRabitsList().Remove(transform);

        base.Die(killer);


    }
}