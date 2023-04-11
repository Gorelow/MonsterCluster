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
        public static readonly string APPEAR_TRIGGER = "OnAppear";
        public static readonly string REAPPEAR_TRIGGER = "OnReappear";
        public static readonly string DISAPPEAR_TRIGGER = "OnDisappear";

        [SerializeField] private Animator _animator;
        [SerializeField] private bool _isAnimating = false;
        /// <summary>
        /// Для того чтобы показать инфу о персонаже нужно:
        /// 1. Изображение юнита
        /// 2. Здоровье юнита
        /// 3. Имя
        /// 4. Скорость
        /// 5. Энергия 
        /// </summary>
        [Header("Info to show")]
        [SerializeField] private Image _image;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private Slider _health;
        [SerializeField] private TextMeshProUGUI _speed;
        [SerializeField] private TextMeshProUGUI _energy;

        /// <summary>
        /// Для aнимирования требуется:
        /// Аниматор,
        /// Переменная показывающая показывает ли сейчас аниматор что-то или нет
        /// </summary>

        private int overlapCounter = 0;

        public async void ShowInfo(IUnitController unit)
        {
            overlapCounter++;

            if (_isAnimating)
            {
                _animator.SetTrigger(REAPPEAR_TRIGGER);
                await Task.Delay(REAPPEAR_DELAY);
            }
            else
            {
                _animator.SetTrigger(APPEAR_TRIGGER);
                _isAnimating = true;
            }
            _image.sprite = unit.Image;
            _name.text = unit.Name;
            _health.value = unit.Health;
            _speed.text = unit.Initiative.ToString();
            _energy.text = unit.Speed.ToString();

            await Task.Delay(SHOW_TIME);

            overlapCounter--;
            if (overlapCounter > 0) return;

            RemoveInfo();
        }

        public void RemoveInfo()
        {
            _animator.SetTrigger(DISAPPEAR_TRIGGER);
            _isAnimating = false;
        }
    }
}
