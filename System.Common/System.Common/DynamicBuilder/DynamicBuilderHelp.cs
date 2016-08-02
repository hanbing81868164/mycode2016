using System.Collections.Generic;
using System.Data;

namespace System
{
    public class DynamicBuilderHelper<Model>
    {
        public static IList<Model> GetList(IDataReader reader)
        {
            IList<Model> mylist = new List<Model>();
            while (reader.Read())
            {
                mylist.Add(DynamicBuilder<Model>.CreateBuilderDataReader(reader));
            }
            return mylist;
        }

        public static Model GetModel(IDataReader reader)
        {
            Model mymodel = default(Model);
            while (reader.Read())
            {
                mymodel = DynamicBuilder<Model>.CreateBuilderDataReader(reader);
            }
            return mymodel;
        }




        public static IList<Model> GetList(DataTable dt)
        {
            IList<Model> mylist = new List<Model>();
            foreach (DataRow dr in dt.Rows)
            {
                mylist.Add(GetModel(dr));
            }

            return mylist;
        }

        public static Model GetModel(DataRow dr)
        {
            return DynamicBuilder<Model>.CreateBuilderDataRow(dr);
        }
    }
}
