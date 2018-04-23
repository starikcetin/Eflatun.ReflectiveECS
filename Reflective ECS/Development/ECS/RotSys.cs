﻿using ReflectiveECS.Core;
using ReflectiveECS.Core.ECS;

namespace Development.ECS
{
    public class RotSys : ISystem
    {
        [Execute]
        public void Execute(RotComp rotComp)
        {
            rotComp.Angle += 10;
        }
    }
}