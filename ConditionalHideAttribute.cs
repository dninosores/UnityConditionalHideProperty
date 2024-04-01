using UnityEngine;
using System;
using System.Collections;

//Original version of the ConditionalHideAttribute created by Brecht Lecluyse (www.brechtos.com)
//Modified by: ollyisonit

namespace ollyisonit.UnityEditorAttributes
{

	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
		AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
	public class ConditionalHideAttribute : PropertyAttribute
	{
		public (string field, object comparison)[] conditions;
		public enum FoldBehavior
		{
			And = 0,
			Or = 1
		}

		public FoldBehavior foldBehavior;

		public string displayName = null;


		/// <summary>
		/// Only displays field in-editor if conditions are met
		/// </summary>
		/// <param name="foldBehavior">Should all conditions need to be met or only one?</param>
		/// <param name="conditions">An array of condition, where each condition 
		/// is tuple of the name of a field on this object and the value it
		/// should be equal to in order for the condition to be satisfied.</param>
		public ConditionalHideAttribute(FoldBehavior foldBehavior, params (string, object)[] conditions)
		{
			this.foldBehavior = foldBehavior;
			this.conditions = conditions;
		}

		/// <summary>
		/// Only displays field in-editor if all conditions are met.
		/// </summary>
		/// <param name="conditions">An array of condition, where each condition is tuple of the
		/// name of a field on this object and the value it should 
		/// be equal to in order for the condition to be satisfied.</param>
		public ConditionalHideAttribute(params (string, object)[] conditions) : this(FoldBehavior.And, conditions)
		{

		}


		/// <summary>
		/// Only displays in-editor if the given field is equal to the given object.
		/// </summary>
		/// <param name="field">Field to use for determining whether this should display in-editor.</param>
		/// <param name="comparison">Will display in-editor if the supplied field is equal to this object.</param>
		public ConditionalHideAttribute(string field, object comparison)
		{
			conditions = new (string, object)[]
			{
			(field, comparison)
			};
			foldBehavior = FoldBehavior.And;
		}

		/// <summary>
		/// Only displays in-editor if the given field is equal to the given object.
		/// </summary>
		/// <param name="field">Field to use for determining whether this should display in-editor.</param>
		/// <param name="comparison">Will display in-editor if the supplied field is equal to this object.</param>
		/// <param name="displayName">The name that should be used for displaying in-editor.</param>
		public ConditionalHideAttribute(string field, object comparison, string displayName) : this(field, comparison)
		{
			this.displayName = displayName;
		}

		/// <summary>
		/// Only displays in-editor if some combination of conditions are met. Each field in the fields array corresponds to 
		/// an object value in the comparisons array.
		/// </summary>
		/// <param name="fields">Array of fields to check</param>
		/// <param name="comparisons">Array of objects to compare to the fields supplied in the fields array</param>
		/// <param name="foldBehavior">Do all conditions need to be met in order to display in-editor, or only one?</param>
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

		/// <summary>
		/// Only displays in-editor if some combination of conditions are met. Each field in the fields array corresponds to 
		/// an object value in the comparisons array.
		/// </summary>
		/// <param name="fields">Array of fields to check</param>
		/// <param name="comparisons">Array of objects to compare to the fields supplied in the fields array</param>
		/// <param name="displayName">Name to use when displaying in-editor</param>
		/// <param name="foldBehavior">Do all conditions need to be met in order to display in-editor, or only one?</param>
		public ConditionalHideAttribute(string[] fields, object[] comparisons, string displayName, FoldBehavior foldBehavior = FoldBehavior.And) :
			this(fields, comparisons, foldBehavior)
		{
			this.displayName = displayName;
		}

		/// <summary>
		/// Combines to booleans according to the set fold behavior and returns the result.
		/// </summary>
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



