using System;

namespace ReflectiveECS.Examples.ECS
{
    public class DebugEntitiesSystem : ISystem
    {
        [Execute(getEntityItself: true)]
        public void Execute(Entity entity)
        {
            Console.WriteLine($"entity {entity.ID}");
            foreach (var component in entity.Components)
            {
                Console.WriteLine("\t" + component.GetType().Name);

                var fields = component.GetType().GetFields();
                foreach (var fieldInfo in fields)
                {
                    Console.WriteLine($"\t\t{fieldInfo.Name} = {fieldInfo.GetValue(component)}");
                }
            }
            Console.WriteLine();
        }
    }
}
