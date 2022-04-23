using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Paint
{
    internal sealed class MyBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            Type ttd = null;
            try
            {
                string toassname = assemblyName.Split(',')[0];
                Assembly[] asmblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (Assembly ass in asmblies)
                {
                    if (ass.FullName.Split(',')[0] == toassname)
                    {
                        ttd = ass.GetType(typeName);
                        break;
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return ttd;
        }
    }
}
