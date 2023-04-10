using System;
using UnityEngine;

namespace Interfaces
{
    public interface IAimController
    {
        public event Action<Vector2> OnPositionClicked;
    }
}
