using System.Threading.Tasks;
using Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
    public class UnitUI : MonoBehaviour
    {
        public static readonly int SHOW_TIME = 5000;
        public static readonly int REAPPEAR_DELAY = 200;

        [SerializeField] private Animator _animator;
        [SerializeField] private bool _isAnimating = false;
        
        [Header("Info to show")]
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private Slider _health;
        [SerializeField] private TextMeshProUGUI _speed;
        [SerializeField] private TextMeshProUGUI _energy;
        
        private int _overlapCounter = 0;

        private static readonly int OnAppear = Animator.StringToHash("OnAppear");
        private static readonly int OnReappear = Animator.StringToHash("OnReappear");
        private static readonly int OnDisappear = Animator.StringToHash("OnDisappear");

        public async void ShowInfo(IUnitController unit)
        {
            _overlapCounter++;
            
            await Task.Run(AnimateAppearance);
            UpdateUnitBaseInfo(unit);
            await Task.Delay(SHOW_TIME);
            
            _overlapCounter--;
            if (_overlapCounter > 0) return;
            RemoveInfo();
        }

        private async void AnimateAppearance()
        {
            if (_isAnimating)
            {
                _animator.SetTrigger(OnReappear);
                await Task.Delay(REAPPEAR_DELAY);
            }
            else
            {
                _animator.SetTrigger(OnAppear);
                _isAnimating = true;
            }
        }

        private void UpdateUnitBaseInfo(IUnitController unit)
        {
            _image.sprite = unit.Image;
            _name.text = unit.Name;
            _health.value = unit.Health;
            _speed.text = unit.Initiative.ToString();
            _energy.text = unit.Speed.ToString();
        }

        public void RemoveInfo()
        {
            _animator.SetTrigger(OnDisappear);
            _isAnimating = false;
        }
    }
}