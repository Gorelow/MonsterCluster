using UnityEngine;

namespace Interfaces
{
    public interface ICardView 
    {
        public void MoveTo(Vector2 newPosition);
        public void TeleportTo(Vector2 newPosition);

        public void SetActive(bool active);
    }
}
