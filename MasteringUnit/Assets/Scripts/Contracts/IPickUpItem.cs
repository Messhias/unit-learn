using UnityEngine;

namespace Contracts
{
    public interface IPickUpItem
    {
        public void OnPickedUp(GameObject whoPickedUp);
    }
}