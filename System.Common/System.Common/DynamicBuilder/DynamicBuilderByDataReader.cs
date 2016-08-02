using System.Data;
using System.Reflection.Emit;
using System.Reflection;

namespace System
{
    public class DynamicBuilderByDataReader<T> : DynamicBuilderBase<T, IDataRecord>
    {
        public DynamicBuilderByDataReader() { }

        public override MethodInfo isDBNullMethod
        {
            get
            {
                return typeof(IDataRecord).GetMethod("IsDBNull", new Type[] { typeof(int) });
            }
            set
            {
                base.isDBNullMethod = value;
            }
        }
        public override ILGenerator GetILGenerator(ILGenerator generator, LocalBuilder result, IDataRecord dataRecord)
        {
            for (int i = 0; i < dataRecord.FieldCount; i++)
            {
                PropertyInfo propertyInfo = typeof(T).GetProperty(dataRecord.GetName(i));
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
                    generator.Emit(OpCodes.Unbox_Any, dataRecord.GetFieldType(i));
                    generator.Emit(OpCodes.Callvirt, propertyInfo.GetSetMethod());

                    generator.MarkLabel(endIfLabel);
                }
            }

            return generator;
        }
    }
}
