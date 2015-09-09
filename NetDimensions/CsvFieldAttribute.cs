using System;
using System.Linq;

namespace NetDimensionsWrapper.NetDimensions
{
    /// <summary>
    /// This attribute is used by the NetDimensions wrapper, to map a specific property to a matching CSV field, with potentially a different name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    internal sealed class CsvFieldAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CsvFieldAttribute" /> class.
        /// </summary>
        /// <param name="csvFieldHeader">The header text of the matching CSV field.</param>
        public CsvFieldAttribute(string csvFieldHeader)
        {
            this.csvFieldHeader = csvFieldHeader;
        }

        /// <summary>
        /// Gets the header text of the matching CSV field.
        /// </summary>
        public string CsvFieldHeader
        {
            get { return this.csvFieldHeader; }
        }

        private readonly string csvFieldHeader;
    }
}