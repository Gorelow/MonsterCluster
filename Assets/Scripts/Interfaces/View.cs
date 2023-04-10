using UnityEngine;

namespace Interfaces
{
    public abstract class View<TBaseController> : MonoBehaviour, IView<TBaseController>
    {
        protected TBaseController _controller;
        private bool _isConnectedToController;

        public void Init(TBaseController controller)
        {
            _controller = controller;
            SetConnectionToController(true);
        }

        private void SetConnectionToController(bool active)
        {
            if (_controller == null || _isConnectedToController == active) return;

            _isConnectedToController = active;

            SetConnectToControllerEvents(active);
        }

        protected abstract void SetConnectToControllerEvents(bool active);

        private void OnEnable()
        {
            SetConnectionToController(true);
        }

        private void OnDisable()
        {
            SetConnectionToController(false);
        }
    }
}