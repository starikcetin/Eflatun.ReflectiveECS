using System;
using System.Collections.Generic;
using ReflectiveECS.Core.ECS;

namespace ReflectiveECS.Core.Managers
{
    public class EntityGrouper
    {
        private readonly Dictionary<Type, HashSet<Entity>> _entityGroups = new Dictionary<Type, HashSet<Entity>>();

        public void Register(Entity e, Type cType)
        {
            AddSafe(e, cType);
        }

        public void Unregister(Entity e, Type cType)
        {
            RemoveSafe(e, cType);
        }

        public HashSet<Entity> GetGroup(Type cType)
        {
            return _entityGroups[cType];
        }

        public HashSet<Entity> GetGroupIntersect(params Type[] cTypes)
        {
            HashSet<Entity> result = _entityGroups[cTypes[0]];

            for (var i = 1; i < cTypes.Length; i++)
            {
                EnsureKeyPresent(cTypes[i]);
                result.IntersectWith(_entityGroups[cTypes[i]]);
            }

            return result;
        }

        private void RemoveSafe(Entity entity, Type cType)
        {
            if (EnsureKeyPresent(cType))
            {
                _entityGroups[cType].Remove(entity);
            }
        }

        private void AddSafe(Entity e, Type cType)
        {
            EnsureKeyPresent(cType);
            _entityGroups[cType].Add(e);
        }

        // returns if the key was already present
        private bool EnsureKeyPresent(Type cType)
        {
            if (!_entityGroups.ContainsKey(cType))
            {
                _entityGroups[cType] = new HashSet<Entity>();
                return false;
            }

            return true;
        }
    }
}
