using System.Data;

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
    }
}
