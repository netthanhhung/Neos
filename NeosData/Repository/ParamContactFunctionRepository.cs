using System;
using System.Collections.Generic;
using System.Text;
using FrameworkLite.Entity;

namespace Neos.Data
{
    public class ParamContactFunctionRepository : Repository<ParamContactFunction>
    {
        public static Comparison<ParamContactFunction> NameAscComparison = delegate(ParamContactFunction f1, ParamContactFunction f2)
        {
            return f1.FunctionName.CompareTo(f2.FunctionName);
        };

        public List<ParamContactFunction> FindAllWithAscSort()
        {
            List<ParamContactFunction> list = new List<ParamContactFunction>();
            list = this.FindAll() as List<ParamContactFunction>;
            list.Sort(NameAscComparison);

            return list;
        }
    }
}
