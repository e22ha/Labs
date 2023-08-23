using System.ComponentModel;
using UnityEditor;
using UnityEngine;


public static class MyEnum
{
    public static string ToDescriptionString(this States val)
    {
        DescriptionAttribute[] attributes = (DescriptionAttribute[])val
            .GetType()
            .GetField(val.ToString())
            .GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attributes.Length > 0 ? attributes[0].Description : string.Empty;
    }
}
    public enum States
    {
        [Description("idle")]
        idle = 0,
        [Description("isWalking")]
        run = 1,
        [Description("open")]
        open = 2,
        [Description("hit")]
        hit = 3,
        [Description("fall")]
        fall = 4
    };

