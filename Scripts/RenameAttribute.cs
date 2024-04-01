using UnityEngine;

namespace ollyisonit.UnityEditorAttributes
{
    // original code from https://answers.unity.com/questions/1487864/change-a-variable-name-only-on-the-inspector.html
    /// <summary>
    /// Changes display name of a field in-editor.
    /// </summary>
    public class RenameAttribute : PropertyAttribute
    {
        /// <summary>
        /// In-editor display name.
        /// </summary>
        public string NewName
        {
            get; private set;
        }

        /// <summary>
        ///  Changes display name of a field in-editor.
        /// </summary>
        /// <param name="name">Name to display in-editor for this field.</param>
        public RenameAttribute(string name)
        {
            NewName = name;
        }
    }
}
