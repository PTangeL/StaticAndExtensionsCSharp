using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace StaticAndExtensionsCSharpStandard.Database
{
    public static class DataSetExtensions
    {
        /// <summary>
        /// Get a value from the dataset.
        /// </summary>
        /// <param name="ds">The dataset.</param>
        /// <param name="table">The table in case of multiples.</param>
        /// <param name="row">The row.</param>
        /// <param name="column">The Column.</param>
        /// <returns></returns>
        public static object Get(this DataSet ds, int table, int row, string column)
        {
            if (table < 0 || table > ds.Tables.Count) return null;

            if (row >= 0 && row <= ds.Tables[table].Rows.Count)
            {
                return ds.Tables[table].Rows[row][column];
            }

            return null;
        }

        /// <summary>
        /// Get a value from the dataset.
        /// </summary>
        /// <param name="ds">The dataset.</param>
        /// <param name="column">The Column.</param>
        /// <param name="expectedValue">The value you expect to get from the column.</param>
        /// <returns></returns>
        public static DataSet Where(this DataSet ds, string column, object expectedValue)
        {
            Contract.Requires(ds != null);
            Contract.Requires(expectedValue != null);
            Contract.Requires(string.IsNullOrEmpty(column));

            Contract.Ensures(Contract.Result<DataSet>() != null);

            return Where(ds, $"{column} = '{expectedValue.ToString()}'", LoadOption.PreserveChanges);
        }

        /// <summary>
        /// Get a value from the dataset.
        /// </summary>
        /// <param name="ds">The dataset.</param>
        /// <param name="filterExpression">Example: columnName: Size, expectedValue: >= 230, then the value on the filter: columnName >= expectedValue</param>
        /// <param name="columnsOnFilter">Fill this with the column names used at filterExpressions</param>
        /// <returns></returns>
        public static DataSet Where(this DataSet ds, string filterExpression, LoadOption loadOptions, params string[] columnsOnFilter)
        {
            Contract.Requires(ds != null);
            Contract.Requires(columnsOnFilter != null);

            Contract.Ensures(Contract.Result<DataSet>() != null);

            var toReturn = ds.Clone();
            toReturn.Tables.Clear();

            foreach (DataTable table in ds.Tables)
            {
                if (!columnsOnFilter.Any(n => table.Columns.Contains(n)))
                    continue;

                var filteredRows = table.Select(filterExpression);

                if (!filteredRows.Any()) continue;

                filteredRows.CopyToDataTable(table, loadOptions);
                toReturn.Tables.Add(table);
            }

            return toReturn;
        }

        /// <summary>
        /// Gets the tables count.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <returns></returns>
        public static int GetTablesCount(this DataSet dataSet)
        {
            if (!dataSet.IsEmpty())
                return dataSet.Tables.Count;
            else
                return 0;
        }

        /// <summary>
        /// Determines whether the specified data set is empty.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <returns>
        ///     <c>true</c> if the specified data set is empty; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEmpty(this DataSet dataSet)
        {
            var isEmpty = false;

            if (dataSet == null)
            {
                isEmpty = true;
            }
            else
            {
                if (dataSet.Tables.Count == 0)
                {
                    isEmpty = true;
                }
                else
                {
                    isEmpty = dataSet.Tables[0].Rows.Count == 0 ? true : false;
                }
            }
            return isEmpty;
        }

        /// <summary>
        /// Determines whether the specified data set has constraints.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>
        ///     <c>true</c> if the specified data set has constraints; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasConstraints(this DataSet dataSet, string tableName)
        {
            Contract.Requires(dataSet != null);

            return dataSet.Tables[tableName].Constraints.Count > 0 ? true : false;
        }

        /// <summary>
        /// Determines whether the specified data set has errors.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>
        ///     <c>true</c> if the specified data set has errors; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasTableErrors(this DataSet dataSet, string tableName)
        {
            Contract.Requires(dataSet != null);

            return dataSet.Tables[tableName].HasErrors;
        }

        /// <summary>
        /// Gets the table errors.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public static string GetTableErrors(this DataSet dataset, string tableName)
        {
            Contract.Requires(dataset != null);

            Contract.Ensures(Contract.Result<string>() != null);
            
            var error = string.Empty;

            return !dataset.HasTableErrors(tableName) ? 
                error : 
                dataset.Tables[tableName].GetErrors().Aggregate(
                    error, (current1, row) => 
                        row.GetColumnsInError().Aggregate(
                            current1, (current, column) => 
                                current + string.Format("Error : {0}", row.GetColumnError(column))));
        }

        /// <summary>
        /// Gets the data set sizein bytes.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <returns></returns>
        public static int GetDataSetSizeinBytes(this DataSet dataSet)
        {
            if (dataSet != null)
            {
                var stream = new MemoryStream();
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream, dataSet);

                stream.Seek(0, 0);
                var result = stream.ToArray();

                stream.Close();

                return result.Length;
            }
            else
                return 0;
        }

        /// <summary>
        ///   delegates to other CopyToDataTable overload with a null FillErrorEventHandler.
        /// </summary>
        public static void CopyToDataTable<T>(this IEnumerable<T> source, DataTable table, LoadOption options)
            where T : DataRow
        {
            DataSetUtil.CheckArgumentNull(source, "source");
            DataSetUtil.CheckArgumentNull(table, "table");
            LoadTableFromEnumerable(source, table, options, null);
        }

        private static DataTable LoadTableFromEnumerable<T>(IEnumerable<T> source, DataTable table, LoadOption? options, FillErrorEventHandler errorHandler)
            where T : DataRow
        {
            if (options.HasValue)
            {
                switch (options.Value)
                {
                    case LoadOption.OverwriteChanges:
                    case LoadOption.PreserveChanges:
                    case LoadOption.Upsert:
                        break;

                    default:
                        throw DataSetUtil.InvalidLoadOption(options.Value);
                }
            }
            using (var rows = source.GetEnumerator())
            {
                // need to get first row to create table
                if (!rows.MoveNext())
                {
                    if (table == null)
                    {
                        throw DataSetUtil.InvalidOperation("The source contains no DataRows.");
                    }
                    else
                    {
                        return table;
                    }
                }

                DataRow current;
                if (table == null)
                {
                    current = rows.Current;
                    if (current == null)
                    {
                        throw DataSetUtil.InvalidOperation("The source contains a DataRow reference that is null.");
                    }

                    table = new DataTable {Locale = CultureInfo.CurrentCulture};
                    // We do not copy the same properties that DataView.ToTable does.
                    // If user needs that functionality, use other CopyToDataTable overloads.
                    // The reasoning being, the IEnumerator<DataRow> can be sourced from
                    // different DataTable, so we just use the "Default" instead of resolving the difference.

                    foreach (DataColumn column in current.Table.Columns)
                    {
                        table.Columns.Add(column.ColumnName, column.DataType);
                    }
                }

                table.BeginLoadData();
                try
                {
                    do
                    {
                        current = rows.Current;
                        if (current == null)
                        {
                            continue;
                        }

                        object[] values = null;
                        try
                        {   // 'recoverable' error block
                            switch (current.RowState)
                            {
                                case DataRowState.Detached:
                                    if (!current.HasVersion(DataRowVersion.Proposed))
                                    {
                                        throw DataSetUtil.InvalidOperation("The source contains a detached DataRow that cannot be copied to the DataTable.");
                                    }
                                    goto case DataRowState.Added;
                                case DataRowState.Unchanged:
                                case DataRowState.Added:
                                case DataRowState.Modified:
                                    values = current.ItemArray;
                                    if (options.HasValue)
                                    {
                                        table.LoadDataRow(values, options.Value);
                                    }
                                    else
                                    {
                                        table.LoadDataRow(values, true);
                                    }
                                    break;

                                case DataRowState.Deleted:
                                    throw DataSetUtil.InvalidOperation("The source contains a deleted DataRow that cannot be copied to the DataTable.");
                                default:
                                    throw DataSetUtil.InvalidDataRowState(current.RowState);
                            }
                        }
                        catch (Exception e)
                        {
                            if (!DataSetUtil.IsCatchableExceptionType(e))
                            {
                                throw;
                            }

                            FillErrorEventArgs fillError = null;
                            if (null != errorHandler)
                            {
                                fillError = new FillErrorEventArgs(table, values) {Errors = e};
                                errorHandler.Invoke(rows, fillError);
                            }
                            if (null == fillError)
                            {
                                throw;
                            }
                            else if (!fillError.Continue)
                            {
                                if (object.ReferenceEquals(fillError.Errors ?? e, e))
                                {   // if user didn't change exception to throw (or set it to null)
                                    throw;
                                }
                                else
                                {
                                    // user may have changed exception to throw in handler
                                    if (fillError.Errors != null) throw fillError.Errors;
                                }
                            }
                        }
                    } while (rows.MoveNext());
                }
                finally
                {
                    table.EndLoadData();
                }
            }

            Debug.Assert(true, "null DataTable");
            return table;
        }
    }
}