namespace Library.Data
{
    using System.Data;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;

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
            if (table >= 0 && table <= ds.Tables.Count)
            {
                if (row >= 0 && row <= ds.Tables[table].Rows.Count)
                {
                    return ds.Tables[table].Rows[row][column];
                }
            }

            return null;
        }

        /// <summary>
        /// Get a value from the dataset.
        /// </summary>
        /// <param name="ds">The dataset.</param>
        /// <param name="table">The table in case of multiples.</param>
        /// <param name="row">The row.</param>
        /// <param name="column">The Column.</param>
        /// <returns></returns>
        public static DataSet Where(this DataSet ds, string column, object expectedValue)
        {
            Contract.Requires(ds != null);
            Contract.Requires(string.IsNullOrEmpty(column));

            var toReturn = ds.Clone();
            
            foreach(DataTable table in ds?.Tables)
            {
                // Clear the rows.
                toReturn.Tables[table.TableName].Rows.Clear();

                foreach (DataRow row in table.Rows)
                    if (row[column].Equals(expectedValue))
                        toReturn.Tables[table.TableName].Rows.Add(row);
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
            bool isEmpty = false;

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
        public static bool HasConstraints(this DataSet dataSet, string tableName) => dataSet.Tables[tableName].Constraints.Count > 0 ? true : false;

        /// <summary>
        /// Determines whether the specified data set has errors.
        /// </summary>
        /// <param name="dataSet">The data set.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns>
        ///     <c>true</c> if the specified data set has errors; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasTableErrors(this DataSet dataSet, string tableName) => dataSet.Tables[tableName].HasErrors;

        /// <summary>
        /// Gets the table errors.
        /// </summary>
        /// <param name="dataset">The dataset.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public static string GetTableErrors(this DataSet dataset, string tableName)
        {
            string error = string.Empty;

            if (dataset.HasTableErrors(tableName))
            {
                foreach (DataRow row in dataset.Tables[tableName].GetErrors())
                {
                    foreach (DataColumn column in row.GetColumnsInError())
                    {
                        error += string.Format("Error : {0}"
                                                        , row.GetColumnError(column));
                    }
                }
            }
            return error;
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
                MemoryStream stream = new MemoryStream();
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream, dataSet);

                stream.Seek(0, 0);
                byte[] result = stream.ToArray();

                stream.Close();

                return result.Length;
            }
            else
                return 0;
        }
    }
}