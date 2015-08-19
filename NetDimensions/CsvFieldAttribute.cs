using System;
using System.Linq;

namespace SitefinityWebApp.NetDimensions
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    sealed class CsvFieldAttribute : Attribute
    {
        public CsvFieldAttribute(string csvFieldHeader)
        {
            this.csvFieldHeader = csvFieldHeader;
        }

        public string CsvFieldHeader
        {
            get { return csvFieldHeader; }
        }

        private readonly string csvFieldHeader;
    }
}