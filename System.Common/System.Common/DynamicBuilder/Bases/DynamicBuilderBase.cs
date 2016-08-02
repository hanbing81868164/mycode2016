using System.Reflection.Emit;

namespace System
{
    public  class DynamicBuilderBase<T, DT> : DynamicBuilderBaseCore<T, DT>
    {
        public DynamicBuilderBase() { }

        public override DynamicBuilderBaseCore<T, DT> CreateBuilder(DT dataRecord)
        {
            DynamicBuilderBaseCore<T, DT> dynamicBuilder = new DynamicBuilderBaseCore<T, DT>();

            DynamicMethod method = new DynamicMethod("DynamicCreate", typeof(T), new Type[] { typeof(DT) }, typeof(T), true);
            ILGenerator generator = method.GetILGenerator();

            LocalBuilder result = generator.DeclareLocal(typeof(T));
            generator.Emit(OpCodes.Newobj, typeof(T).GetConstructor(Type.EmptyTypes));
            generator.Emit(OpCodes.Stloc, result);

            generator = GetILGenerator(generator, result, dataRecord);

            generator.Emit(OpCodes.Ldloc, result);
            generator.Emit(OpCodes.Ret);

            dynamicBuilder.handler = (Load)method.CreateDelegate(typeof(Load));

            dynamicBuilder.DataRecord = dataRecord;

            return dynamicBuilder;
        }
    }
}