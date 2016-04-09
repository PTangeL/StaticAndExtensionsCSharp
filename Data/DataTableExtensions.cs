namespace StaticAndExtensionsCSharp.Data
{
    using Lists;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    public static class DataTableExtensions
    {
        /// <summary>
        /// Selects the duplicates.
        /// </summary>
        /// <param name="dataTable">The data table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public static IEnumerable<object> SelectDuplicates(this DataTable dataTable, string columnName)
        {
            IEnumerable<object> duplicates = Enumerable.Empty<object>();

            for (int iValue = 0; iValue < dataTable.Rows.Count; iValue++)
            {
                object lastValue = dataTable.Rows[iValue][columnName];
                if (duplicates.Contains(lastValue))
                    duplicates.Add(ref duplicates, lastValue);
            }
            return duplicates;
        }

        /// <summary>
        /// Selects the uniques.
        /// </summary>
        /// <param name="dataTable">The data table.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns></returns>
        public static IEnumerable<object> SelectUniques(this DataTable dataTable, string columnName)
        {
            IEnumerable<object> uniques = Enumerable.Empty<object>();

            for (int iValue = 0; iValue < dataTable.Rows.Count; iValue++)
            {
                object lastValue = dataTable.Rows[iValue][columnName];
                if (!uniques.Contains(lastValue))
                    uniques.Add(ref uniques, lastValue);
            }
            return uniques;
        }
    }
}