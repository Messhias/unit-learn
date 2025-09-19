namespace Contracts
{
    public interface IAIBrain
    {
        public void Jump(float force);
        public void AlertIfPlayerNearby(float distance);
        public void PauseAI(float timeInMilliseconds);
        public void UseWeapon();
        public void LookAtPlayer();
        public void MoveTowardsPlayer(float speed);
        public void MoveTowardsPlayerUsingNavMesh();
    }
}