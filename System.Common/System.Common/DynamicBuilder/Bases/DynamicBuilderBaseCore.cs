using System.Reflection;
using System.Reflection.Emit;

namespace System
{
    public class DynamicBuilderBaseCore<T, DT> 
    {
        private MethodInfo _getValueMethod = typeof(DT).GetMethod("get_Item", new Type[] { typeof(int) });
        private MethodInfo _isDBNullMethod = typeof(DT).GetMethod("IsDBNull", new Type[] { typeof(int) });
        private DT dataRecord = default(DT);

        public virtual DT DataRecord
        {
            get { return this.dataRecord; }
            set { this.dataRecord=value;}
        }

        public virtual MethodInfo getValueMethod
        {
            get { return this._getValueMethod; }
            set { this._getValueMethod = value; }
        }
        public virtual MethodInfo isDBNullMethod
        {
            get { return this._isDBNullMethod; }
            set { this._isDBNullMethod = value; }
        }


        public delegate T Load(DT dataRecord);
        //public delegate T Load();
        public Load handler;

        public DynamicBuilderBaseCore(){}

        protected virtual T Build(DT dataRecord)
        {
            return handler(dataRecord);
        }

        //public virtual T Build()
        //{
        //    return handler();
        //}

        public virtual T Build()
        {
            return this.Build(this.DataRecord);
        }

        public virtual ILGenerator GetILGenerator(ILGenerator generator, LocalBuilder result, DT dataRecord) { throw new NotSupportedException(); }

        public virtual DynamicBuilderBaseCore<T, DT> CreateBuilder(DT dataRecord) { throw new NotSupportedException(); }
    }
}
