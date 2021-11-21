namespace Entities
{
    public class Player : BaseCharacter
    {
        private int _currentLevel;

        private void OnLevelUp()
        {
            _currentLevel++;
            weapon.SetLevel(_currentLevel);
        }
    }
}