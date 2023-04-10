using Interfaces;
using UnityEngine;

public class UnitView : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] private AudioSource _hurtAudio;
    [SerializeField] private AudioSource _deathAudio;
    [SerializeField] private AudioSource _healAudio;
    [SerializeField] private AudioSource _moveAudio;
    [SerializeField] private ParticleSystem _hurtParticle;
    [SerializeField] private ParticleSystem _deathParticle;
    [SerializeField] private ParticleSystem _healParticle;
    [SerializeField] private ParticleSystem _moveParticle;

    private IUnitController _controller;
    private bool _isActive;

    public void Set(IUnitController controller)
    {
        _controller = controller;

        UpdateUnit();
        SetConnection(true);
    }

    private void SetConnection(bool active)
    {
        if (_controller == null || _isActive == active) return;

        _isActive = active;

        if (active)
        {
            _controller.OnGettingDamage += (int i) => PlayAnimation(AnimationStates.Hurt);
            _controller.OnDeath += () => PlayAnimation(AnimationStates.Dead);
            _controller.OnMove += (Vector2 v) => PlayAnimation(AnimationStates.Moving);
            _controller.OnGettingHealth += (int i) => PlayAnimation(AnimationStates.Healing);
        }
        else
        {
            _controller.OnGettingDamage -= (int i) => PlayAnimation(AnimationStates.Hurt);
            _controller.OnDeath -= () => PlayAnimation(AnimationStates.Dead);
            _controller.OnMove -= (Vector2 v) => PlayAnimation(AnimationStates.Moving);
            _controller.OnGettingHealth -= (int i) => PlayAnimation(AnimationStates.Healing);
        }
    }

    public void PlayAnimation(AnimationStates animation)
    {
        switch (animation)
        {
            case AnimationStates.Hurt: _hurtAudio?.Play(); _hurtParticle?.Play(); break;
            case AnimationStates.Dead: _deathAudio?.Play(); _deathParticle?.Play(); break;
            case AnimationStates.Moving: if (_moveAudio != null) _moveAudio.Play(); if (_moveParticle != null) _moveParticle?.Play(); break;
            case AnimationStates.Healing: if (_healAudio != null) _healAudio?.Play(); if (_healParticle != null) _healParticle?.Play(); break;
        }
    }

    private void OnEnable()
    {
        SetConnection(true);
    }

    private void OnDisable()
    {
        SetConnection(false);
    }

    public void UpdateUnit()
    {
        _animator.runtimeAnimatorController = _controller.Animation;
    }
    public enum AnimationStates
    {
        Hurt,
        Dead,
        Moving,
        Healing
    }
}


