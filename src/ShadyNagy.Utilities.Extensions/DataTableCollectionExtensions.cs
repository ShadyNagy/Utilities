using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ShadyNagy.Utilities.Extensions
{
    public static class DataTableCollectionExtensions
    {

        public static DataTable GetTable(this DataTableCollection tables, string tableName) =>
            tables == null ? new DataTable() :
                tables
                .Cast<DataTable>()
                .FirstOrDefault(x => x.TableName.ToLower() == tableName.ToLower());

        public static DataTable GetTable(this DataTableCollection tables, int tableNumber) =>
            tables == null || tables.Count <= tableNumber ? new DataTable() : tables[tableNumber];

        public static IEnumerable<DataColumn> GetColumns(this DataTableCollection tables, string tableName) =>
            tables == null ? new List<DataColumn>() :
                tables
                .GetTable(tableName)
                .Columns
                .Cast<DataColumn>()
                .ToList();

        public static IEnumerable<DataColumn> GetColumns(this DataTableCollection tables, int tableNumber) =>
            tables == null ? new List<DataColumn>() :
                tables
                    .GetTable(tableNumber)
                    .Columns
                    .Cast<DataColumn>()
                    .ToList();

        public static IEnumerable<DataRow> GetRows(this DataTableCollection tables, string tableName) =>
            tables == null ? new List<DataRow>() :
                tables
                    .GetTable(tableName)
                    .Rows
                    .Cast<DataRow>()
                    .ToList();

        public static IEnumerable<DataRow> GetRows(this DataTableCollection tables, int tableNumber) =>
            tables == null ? new List<DataRow>() :
                tables
                    .GetTable(tableNumber)
                    .Rows
                    .Cast<DataRow>()
                    .ToList();

        public static IEnumerable<string> GetColumnsNames(this DataTableCollection tables, string tableName) =>
            tables
                .GetColumns(tableName)
                .Select(x => x.Caption)
                .ToList();

        public static IEnumerable<string> GetColumnsNames(this DataTableCollection tables, int tableNumber) =>
            tables
                .GetColumns(tableNumber)
                .Select(x => x.Caption)
                .ToList();
    }
}
