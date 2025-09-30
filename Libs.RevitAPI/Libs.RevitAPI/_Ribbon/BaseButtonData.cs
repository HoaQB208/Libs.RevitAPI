namespace Libs.RevitAPI._Ribbon
{
    /// <summary>
    /// The base infomation for the button in ribbon Revit.
    /// </summary>
    public abstract class BaseButtonData
    {
        /// <summary>
        /// The button name visible to the user.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// The button description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The long description.
        /// </summary>
        public string LongDescription { get; set; }

        /// <summary>
        /// The help url
        /// </summary>
        public string HelpUrl { get; set; } = "";

        /// <summary>
        /// The icon
        /// </summary>
        public byte[] Icon { get; set; }
    }
}
