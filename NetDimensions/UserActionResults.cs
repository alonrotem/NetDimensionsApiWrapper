using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NetDimensionsWrapper.NetDimensions
{
    /// <summary>
    /// Contains the results of batch-action calls in the NetDimensions LMS: Whether the operation completed successfully, and any warnings/errors.
    /// </summary>
    public class UserActionResults
    {
        public UserActionResults()
        {
            this.Warnings = new List<string>();
            this.Errors = new List<string>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserActionResults" /> class with details from the LMS returned CSV results.
        /// </summary>
        /// <param name="csvResults">CSV results provided by the LMS.</param>
        public UserActionResults(string csvResults) : this()
        {
            string[] responseRows = csvResults.Split('\n');
            this.IsCompletedOk = responseRows[responseRows.Length - 1].IndexOf(NetDimensionsConstants.ActionResultNoErrorToken, StringComparison.CurrentCultureIgnoreCase) >= 0;

            //int lastRow = (isCompletedOk) ? responseRows.Length - 2 : responseRows.Length - 1;
            //loop from row 2 to the semi-last row.
            //Row 0 always says: "Please go to the last Column to view the error"
            //Row 1 always contains the CSV heder
            //Row 2 and on: user creation CSV data
            //Last row contains "No error occurred" only if no error occurred. Otherwise it contains just the last CSV row.
            for (int i = 2; i < responseRows.Length; i++)
            {
                if (!string.IsNullOrEmpty(responseRows[i]))
                {
                    string[] csvParts = Regex.Split(responseRows[i].Substring(1, responseRows[i].Length - 2), NetDimensionsConstants.CsvPattern);
                    if (csvParts.Length > 0 && 
                        (!string.IsNullOrWhiteSpace(csvParts[csvParts.Length - 1])) &&
                        (responseRows[i].IndexOf(NetDimensionsConstants.ActionResultNoErrorToken, StringComparison.CurrentCultureIgnoreCase) < 0))
                    {
                        if (csvParts[csvParts.Length - 1].ToUpper().StartsWith(NetDimensionsConstants.ActionResultWarningToken))
                        {
                            this.Warnings.Add(csvParts[csvParts.Length - 1].Substring(NetDimensionsConstants.ActionResultWarningToken.Length));
                        }
                        else
                        {
                            this.Errors.Add(csvParts[csvParts.Length - 1]);
	                    }
	                }
	            }
            }
        }

        public bool IsCompletedOk { get; private set; }
        public List<string> Warnings { get; private set; }
        
        public List<string> Errors { get; private set; }
    }
}