using UnityEngine;
using System;
using System.Collections;

//Original version of the ConditionalHideAttribute created by Brecht Lecluyse (www.brechtos.com)
//Modified by: dninosores

namespace dninosores.UnityEditorAttributes
{

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
        AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
    public class ConditionalHideAttribute : PropertyAttribute
    {
        public (string field, object comparison)[] conditions;
        public enum FoldBehavior
        {
            And,
            Or
        }

        public FoldBehavior foldBehavior;

        public string displayName = null;


        // Use this for initialization
        public ConditionalHideAttribute(FoldBehavior foldBehavior, params (string, object)[] conditions)
        {
            this.foldBehavior = foldBehavior;
            this.conditions = conditions;
        }

        public ConditionalHideAttribute(params (string, object)[] conditions) : this(FoldBehavior.And, conditions)
        {

        }

        public ConditionalHideAttribute(string field, object comparison)
        {
            conditions = new (string, object)[]
            {
            (field, comparison)
            };
            foldBehavior = FoldBehavior.And;
        }

        public ConditionalHideAttribute(string field, object comparison, string displayName) : this(field, comparison)
        {
            this.displayName = displayName;
        }

        public ConditionalHideAttribute(string[] fields, object[] comparisons, FoldBehavior foldBehavior = FoldBehavior.And)
        {
            this.foldBehavior = foldBehavior;
            if (fields == null)
                throw new NullReferenceException("Fields[] cannot be null");
            if (comparisons == null)
                throw new NullReferenceException("Comparisons[] cannot be null");
            if (fields.Length != comparisons.Length)
                throw new ArgumentException("Field and comparison arrays must be same length!");

            conditions = new (string, object)[fields.Length];

            for (int i = 0; i < fields.Length; i++)
            {
                conditions[i] = (fields[i], comparisons[i]);
            }
        }

        public ConditionalHideAttribute(string[] fields, object[] comparisons, string displayName, FoldBehavior foldBehavior = FoldBehavior.And) :
            this(fields, comparisons, foldBehavior)
        {
            this.displayName = displayName;
        }

        public bool Combine(bool left, bool right)
        {
            switch (foldBehavior)
            {
                case (FoldBehavior.And):
                    return left && right;
                case (FoldBehavior.Or):
                    return left || right;
                default:
                    throw new NotImplementedException("No case for fold behavior " + foldBehavior);
            }
        }

    }
}



