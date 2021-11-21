using Entities;
using UnityEngine;
using Zenject;

namespace Levels
{
    public class CharacterFactory : PlaceholderFactory<BaseCharacter, BaseCharacter>
    {
        public BaseCharacter Create(BaseCharacter prefab, Vector3 pos, Quaternion rot, Transform parent)
        {
            var player = base.Create(prefab);
            SetTransform(player.transform, pos, rot, parent);
            return player;
        }

        private void SetTransform(Transform obj, Vector3 pos, Quaternion rot, Transform parent)
        {
            obj.transform.position = pos;
            obj.transform.rotation = rot;
            obj.transform.SetParent(parent, true);
        }
    }
}