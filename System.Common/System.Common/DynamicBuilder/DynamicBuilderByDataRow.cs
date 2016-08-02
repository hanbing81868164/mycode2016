using System.Data;
using System.Reflection.Emit;
using System.Reflection;
using System.Linq;

namespace System
{
    public class DynamicBuilderByDataRow<T> : DynamicBuilderBase<T, DataRow>
    {
        public DynamicBuilderByDataRow() { }

        public override MethodInfo isDBNullMethod
        {
            get
            {
                return typeof(DataRow).GetMethod("IsNull", new Type[] { typeof(int) });
            }
            set
            {
                base.isDBNullMethod = value;
            }
        }
        public override ILGenerator GetILGenerator(ILGenerator generator, LocalBuilder result, DataRow dataRecord)
        {
            for (int i = 0; i < dataRecord.ItemArray.Length; i++)
            {
                //PropertyInfo propertyInfo = typeof(T).GetProperty(dataRecord.Table.Columns[i].ColumnName);
                PropertyInfo propertyInfo = typeof(T).GetProperties().FirstOrDefault(p => p.Name.ToLower() == dataRecord.Table.Columns[i].ColumnName.ToLower());

                Label endIfLabel = generator.DefineLabel();
                if (propertyInfo != null && propertyInfo.GetSetMethod() != null)
                {
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldc_I4, i);
                    generator.Emit(OpCodes.Callvirt, isDBNullMethod);
                    generator.Emit(OpCodes.Brtrue, endIfLabel);
                    generator.Emit(OpCodes.Ldloc, result);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldc_I4, i);
                    generator.Emit(OpCodes.Callvirt, getValueMethod);
                    generator.Emit(OpCodes.Unbox_Any, propertyInfo.PropertyType);
                    generator.Emit(OpCodes.Callvirt, propertyInfo.GetSetMethod());
                    generator.MarkLabel(endIfLabel);
                }
            }


            return generator;
        }
    }
}
