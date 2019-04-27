using UnityEngine;
using System;
using System.Collections;

//Original version of the ConditionalHideAttribute created by Brecht Lecluyse (www.brechtos.com)
//Modified by: -

namespace CleverCode
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
    public class ConditionalShow : PropertyAttribute
    {
        private readonly string _conditionalSourceField;
        private readonly bool _asHide;
        private readonly bool _targetInt;
        private readonly int _intValue;

        // Use this for initialization
        public ConditionalShow(string conditionalSourceField)
        {
            _conditionalSourceField = conditionalSourceField;
            _asHide = false;
            _targetInt = false;
            _intValue = int.MinValue;
        }

        public ConditionalShow(string conditionalSourceField, bool asHide)
        {
            _conditionalSourceField = conditionalSourceField;
            _asHide = asHide;
            _targetInt = false;
            _intValue = int.MinValue;
        }

        public ConditionalShow(string conditionalSourceField, int intValue)
        {
            _conditionalSourceField = conditionalSourceField;
            _asHide = false;
            _targetInt = true;
            _intValue = intValue;
        }

        public ConditionalShow(string conditionalSourceField, int intValue, bool asHide)
        {
            _conditionalSourceField = conditionalSourceField;
            _asHide = asHide;
            _targetInt = true;
            _intValue = intValue;
        }

        public string ConditionalSourceField
        {
            get { return _conditionalSourceField; }
        }

        public bool AsHide
        {
            get { return _asHide; }
        }

        public bool TargetInt
        {
            get { return _targetInt; }
        }

        public int IntValue
        {
            get { return _intValue; }
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class AsPercentAttribute : PropertyAttribute
    {
        public float min;
        public float max;

        public AsPercentAttribute(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public AsPercentAttribute()
        {
            min = 0;
            max = 1;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class CheapReorderableAttribute : PropertyAttribute
    {

    }

    [AttributeUsage(AttributeTargets.Field)]
    public class EditorReadOnlyAttribute : PropertyAttribute
    {

    }

    [AttributeUsage(AttributeTargets.Field)]
    public class EnumFlagAttribute : PropertyAttribute
    {

    }
}