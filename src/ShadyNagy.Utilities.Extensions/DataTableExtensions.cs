using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ShadyNagy.Utilities.Extensions
{
    public static class DataTableExtensions
    {

        public static bool ContainColumn(this DataTable table, string columnName)
        {
            if (table?.Columns == null)
            {
                return false;
            }

            var columns = table.Columns;
            return columns.Contains(columnName);
        }

        public static IEnumerable<DataColumn> GetColumns(this DataTable table)
        {
            if (table == null)
            {
                return new List<DataColumn>();
            }

            return table
                .Columns
                .Cast<DataColumn>()
                .ToList();
        }

        public static List<string> GetColumnsNames(this DataTable table)
        {
            if (table == null)
            {
                return new List<string>();
            }

            var columns = table.GetColumns();
            if (columns == null)
            {
                return new List<string>();
            }

            return columns
                .Select(x => x.Caption)
                .ToList();
        }
    }
}
