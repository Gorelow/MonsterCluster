using System;
using Basic;
using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace View
{
    [Serializable]
    public class AnimationStateToViewEffectDictionary : UnitySerializedDictionary<AnimationStates, ViewEffect> { }
    
    public class UnitView : View<IUnitController>
    {
        [FormerlySerializedAs("_animator")] [SerializeField] private Animator animator;

        [SerializeField] private AnimationStateToViewEffectDictionary stateEffects;

        protected override void InitAdditional() => UpdateUnit();

        protected override void SetConnectToControllerEvents(bool active)
        {
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

        private void PlayAnimation(AnimationStates animationState) => stateEffects[animationState].Play();

        private void UpdateUnit() => animator.runtimeAnimatorController = _controller.Animation;
    }
    
    public enum AnimationStates
    {
        Hurt,
        Dead,
        Moving,
        Healing
    }

    [Serializable]
    public struct ViewEffect
    {
        [SerializeField] private AudioSource audio;
        [SerializeField] private ParticleSystem particleSystem;

        public void Play()
        {
            if (audio != null) audio.Play();
            if (particleSystem != null)  particleSystem.Play();
        }
    }
}
//92~