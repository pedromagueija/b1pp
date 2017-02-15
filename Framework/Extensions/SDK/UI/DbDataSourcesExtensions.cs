// <copyright filename="DbDataSourcesExtensions.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>
namespace B1PP.Extensions.SDK.UI
{
    using System;

    using SAPbouiCOM;

    public static class DbDataSourcesExtensions
    {
        public static double? GetDouble(this DBDataSources sources, string datasourceId, string columnId, int rowIndex)
        {
            DBDataSource source = sources.Item(datasourceId);

            return source.GetDouble(columnId, rowIndex);
        }

        public static int? GetInt(this DBDataSources sources, string datasourceId, string columnId, int rowIndex)
        {
            DBDataSource source = sources.Item(datasourceId);

            return source.GetInt(columnId, rowIndex);
        }

        public static string GetString(this DBDataSources sources, string datasourceId, string columnId, int rowIndex)
        {
            DBDataSource source = sources.Item(datasourceId);

            return source.GetString(columnId, rowIndex);
        }

        public static void Set(this DBDataSources sources, string datasourceId, string columnId, int rowIndex,
            int? value)
        {
            DBDataSource source = sources.Item(datasourceId);

            source.Set(columnId, rowIndex, value);
        }

        public static void Set(this DBDataSources sources, string datasourceId, string columnId, int rowIndex,
            string value)
        {
            DBDataSource source = sources.Item(datasourceId);

            source.Set(columnId, rowIndex, value);
        }

        public static void Set(this DBDataSources sources, string datasourceId, string columnId, int rowIndex,
            double? value)
        {
            DBDataSource source = sources.Item(datasourceId);

            source.Set(columnId, rowIndex, value);
        }

        public static void Set(this DBDataSources sources, string datasourceId, string columnId, int rowIndex,
            DateTime? value)
        {
            DBDataSource source = sources.Item(datasourceId);

            source.Set(columnId, rowIndex, value);
        }
    }
}