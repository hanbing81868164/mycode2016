using System.Data;


namespace System
{
    public class DynamicBuilder<T>
    {
        private static DynamicBuilderByDataReader<T> DBDataReader = new DynamicBuilderByDataReader<T>();
        private static DynamicBuilderByDataRow<T> DBDataRow = new DynamicBuilderByDataRow<T>();

        public static T CreateBuilderDataReader(IDataRecord reader)
        {
            return DBDataReader.CreateBuilder(reader).Build();
        }

        public static T CreateBuilderDataRow(DataRow dr)
        {
            return DBDataRow.CreateBuilder(dr).Build();
        }
    }
}
