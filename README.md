# Eflatun.ReflectiveECS
A proof of concept for a reflection-based ECS system for C#.

**This project is not complete and by no means production-ready. Use it at your own risk.**

# Summary
Systems in an ECS architecture usually fetch and filter entities/components on their own. I wanted to reverse this responsibility and design a centralized invoker that is responsible for invoking systems with the component types they want. When implemented naively, this causes double declaration of dependencies of a system, one time for the Filter object and a second time for the parameters of the execute method.

To eliminate this double-declaration and still keep the filtering responsibility away from systems, I decided to eliminate the filterer object and instead fetch the filter information from the parameters of the executer method of the System. This resulted in a syntax like this:

```cs
public class ExampleSystem : ISystem
{
    [Execute]
    public void ExecuteMethod(ComponentFoo foo, ComponentBar bar, ComponentBaz baz)
    {
        // system logic
    }
}
```

This single method marked with `[Execute]` is all it takes to declare a system with the component dependencies.

Optionally, an Execute method may take the entity itself as the first parameter (and first parameter only):

```cs
public class ExampleSystem : ISystem
{
    [Execute]
    public void ExecuteMethod(Entity entity, ComponentFoo foo, ComponentBar bar, ComponentBaz baz)
    {
        // system logic
    }
}
```

Or you can eliminate all component dependencies and write a system that runs for all existing entities, as follows:

```cs
public class ExampleSystem : ISystem
{
    [Execute]
    public void ExecuteMethod(Entity entity)
    {
        // system logic
    }
}
```

# How
Reflection! The `SystemsRunner` class uses reflection to fetch the component list of the method marked with `Execute` attribute in a system, extracts the types of these parameters and caches this information along with a delegate to invoke the `Execute` method of the said system. Afterwards, it uses this information to filter the entities that has all of the requested components and invokes the method with each of these matching entities.

# Performance

(comma is the fraction seperator and dot is the thousands seperator)

Stress testing
```
Frame #0 - 4263352 ticks
Frame #1 - 4237646 ticks
Frame #2 - 4254241 ticks
Frame #3 - 4253610 ticks
Frame #4 - 4216189 ticks
Frame #5 - 4231133 ticks
Frame #6 - 4224437 ticks
Frame #7 - 4193977 ticks
Frame #8 - 4213998 ticks
Frame #9 - 4238601 ticks
Frame #10 - 4238586 ticks
Frame #11 - 4235248 ticks
Frame #12 - 4200499 ticks
Frame #13 - 4260630 ticks
Frame #14 - 4270294 ticks
Frame #15 - 4241092 ticks
Frame #16 - 4226117 ticks
Frame #17 - 4238532 ticks
Frame #18 - 4255557 ticks
Frame #19 - 4249101 ticks
------------
Total        84.742.840 ticks
Frame avg    4.237.142 ticks
FPS          2,360

Entities     100.000
Comp per Ent 10
Systems      50

Frames       20
------------
```


A more usual setting
```
Frame #0 - 49635 ticks
Frame #1 - 37129 ticks
Frame #2 - 31091 ticks
Frame #3 - 32160 ticks
Frame #4 - 30094 ticks
Frame #5 - 30926 ticks
Frame #6 - 33303 ticks
Frame #7 - 31246 ticks
Frame #8 - 31481 ticks
Frame #9 - 32152 ticks
Frame #10 - 30982 ticks
Frame #11 - 30294 ticks
Frame #12 - 29921 ticks
Frame #13 - 31116 ticks
Frame #14 - 30549 ticks
Frame #15 - 31595 ticks
Frame #16 - 35947 ticks
Frame #17 - 31617 ticks
Frame #18 - 30950 ticks
Frame #19 - 30700 ticks
------------
Total        652.888 ticks
Frame avg    32.644 ticks
FPS          306,331

Entities     1.000
Comp per Ent 10
Systems      50

Frames       20
------------
```

# License
MIT license. Refer to the [LICENSE](https://github.com/starikcetin/Eflatun.ReflectiveECS/blob/master/LICENSE) file.

This project includes third party code which are not explicitly licensed. Refer to the [LICENSE NOTICE](https://github.com/starikcetin/Eflatun.ReflectiveECS/blob/master/Reflective%20ECS/ReflectiveECS/Optimization/FastInvoke/LICENSE%20NOTICE) file.

Copyright (c) 2018 S. Tarık Çetin
