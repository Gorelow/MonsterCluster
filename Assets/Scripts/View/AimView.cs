using Interfaces;
using UnityEngine;

namespace View
{
    public class AimView : View<IAimController>
    {
        [SerializeField] private Animator _animator;

        [SerializeField] private GameObject _aim;

        private bool _isActive;

        protected override void SetConnectToControllerEvents(bool active)
        {
            if (active)
            {
                _controller.OnPositionClicked += PutAim;
            }
            else
            {
                _controller.OnPositionClicked -= PutAim;
            }
        }

        private void PutAim(Vector2 pos)
        {
            _aim.transform.position = pos;
        }
    }
}
