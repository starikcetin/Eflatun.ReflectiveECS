# Reflective-ECS
A proof of concept for a reflection-based ECS system for C#.

This project is not complete and by no means production-ready. Use it at your own risk.

# Performance
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

# Licence
MIT license. Refer to the [LICENCE](https://github.com/starikcetin/Reflective-ECS/blob/master/LICENSE) file.

This project includes third party code which are not explicitly licenced. Refer to the [LICENCE NOTICE](https://github.com/starikcetin/Reflective-ECS/blob/master/Reflective%20ECS/ReflectiveECS/Optimization/FastInvoke/LICENCE%20NOTICE) file.

Copyright (c) 2018 S. Tarık Çetin
