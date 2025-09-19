namespace Contracts
{
    public interface IHealthManager
    {
        public void Reset();
        public float AdjustCurrentHealth(float health);
        public bool IsDead();
        public float GetCurrentHealth();
        public float GetMaxHealth();
    }
}