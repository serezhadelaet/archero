using Combat.Weapons;
using Helpers;
using UI;
using UnityEngine;
using Zenject;

namespace Entities
{
    public class Player : BaseCharacter
    {
        [SerializeField] private ParticleSystem healingEffect;
        [SerializeField] private PlayerProgressionFollower playerProgression;
        [SerializeField] private PlayerMovement movement;
        [SerializeField] private PlayerAttack attack;
        [SerializeField] private PlayerDash dash;
        
        private WinLoseOverlay _winLoseOverlay;
        public BaseWeapon GetWeapon() => weapon;

        [Inject]
        private void Construct(Joystick joystick, GameOverlay gameOverlay, WinLoseOverlay winLoseOverlay)
        {
            _winLoseOverlay = winLoseOverlay;
            movement.Init(joystick, navAgent, animations, dash);
            dash.Init(navAgent, joystick, gameOverlay);
            attack.Init(animations, this, targetLayer, playerProgression, dash);
            SetupOverlay(gameOverlay);
            SetHealth();
            SetWeapon();
            SetLevel();
        }
 
        private void SetupOverlay(GameOverlay gameOverlay)
        {
            gameOverlay.UpdateHealth((int)Health);
            playerProgression.Restart();
            gameOverlay.UpdateExp(playerProgression.GetExp());
            gameOverlay.UpdateLevel(playerProgression.GetLevel());
            
            OnHealthChanged += (health) => gameOverlay.UpdateHealth((int) health);
            playerProgression.OnProgress += () =>
            {
                gameOverlay.UpdateLevel(playerProgression.GetLevel());
                gameOverlay.UpdateExp(playerProgression.GetExp());
            };
        }

        public override void Heal(float hp)
        {
            base.Heal(hp);
            healingEffect.Play();
        }

        protected override void OnDead()
        {
            base.OnDead();
            _winLoseOverlay.Show();
        }

        protected override bool ShouldFollowTarget() => !IsMoving() && animations.IsAttacking() && CurrentTarget != null;
        private bool IsMoving() => Input.GetMouseButton(0);
    }
}