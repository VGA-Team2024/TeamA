using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Enum)] // Enum型にのみ適用可能な属性であることを示す
public class MyEnumCustomAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Class)] 
public class CustomDataAttribute : Attribute
{
}
[AttributeUsage(AttributeTargets.Class)] 
public class CustomDataSetAttribute : Attribute
{
}
