using System;
using System.Threading.Tasks;
using Basic;
using Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[Serializable]
public class CardActionTypeImageDictionary : UnitySerializedDictionary<CardActionType, Image> { }

public class CardComboView : View<ICardComboController>
{
    public static readonly int CHANGE_DELAY = 250;

    [SerializeField] private Animator _animator;

    [SerializeField] private CardActionTypeImageDictionary _cardActionImage;
    [FormerlySerializedAs("_cost")] [SerializeField] private Slider _price;
    [FormerlySerializedAs("_unavalability")] [SerializeField] private Image _unavalabilityImage;
    [SerializeField] private TextMeshProUGUI _unitEnergy;

    private static readonly int OnAppear = Animator.StringToHash("OnAppear");
    private static readonly int OnChange = Animator.StringToHash("OnChange");
    private static readonly int OnDisappear = Animator.StringToHash("OnDisappear");

    private static readonly int ChangeCard = Animator.StringToHash("ChangeCard");

    protected override void SetConnectToControllerEvents(bool active)
    {
        if (active)
        {
            _controller.OnChangeActive += SetAppear;
            _controller.OnTypeCardChange += ChangeTypeCardImage;
            _controller.OnAvailability += ShowAvailability;
            _controller.OnChangeComboPrice += ChangeEnergyPrice;
        }
        else
        {
            _controller.OnChangeActive -= SetAppear;
            _controller.OnTypeCardChange -= ChangeTypeCardImage;
            _controller.OnAvailability -= ShowAvailability;
            _controller.OnChangeComboPrice -= ChangeEnergyPrice;
        }
    }

    protected override void InitAdditional()
    {
        _unitEnergy.text = _controller.AvailableEnergy.ToString();
    }

    private void SetAppear(bool active) => _animator.SetTrigger(active ? OnAppear : OnDisappear);
    
    private void ShowAvailability(bool active) => _unavalabilityImage.enabled = !active;

    private void ChangeEnergyPrice(int value) => _price.value = value;
    
    private async void ChangeTypeCardImage(CardActionType type, ICardController val)
    {
        _animator.SetInteger(ChangeCard, (int)type);
        _animator.SetTrigger(OnChange);

        await Task.Delay(CHANGE_DELAY);
        
        _cardActionImage[type].sprite = val?.Sprite;
    }
}