namespace ReflectiveECS.Core.Managers
{
    public class EntityIdManager
    {
        private int _availableId = 0;

        public int GetUniqueId()
        {
            return _availableId++;
        }
    }
}
