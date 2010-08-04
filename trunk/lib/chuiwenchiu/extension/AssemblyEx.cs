/// <summary>
    /// 
    /// </summary>
    /// <example>
    ///  Assembly.GetExecutingAssembly().GetTypes( t => true).ForEach( (Type x) => {
    ///      Console.WriteLine(x.Name);
    ///      
    ///      Console.WriteLine("\tProperty:");
    ///      x.GetProperties(prop => true).ForEach( pi =>
    ///      {
    ///          Console.WriteLine("\t\t" + pi.Name);
    ///      });
    ///      
    ///      Console.WriteLine("\tField:");
    ///      x.GetFields(prop => true).ForEach(pi =>
    ///      {
    ///          Console.WriteLine("\t\t" + pi.Name);
    ///      });

    ///      Console.WriteLine("\tMethod:");
    ///      x.GetMethods(prop => true).ForEach(pi =>
    ///      {
    ///          Console.WriteLine("\t\t" + pi.Name);
    ///      });

    ///      Console.WriteLine("\tEvent:");
    ///      x.GetEvents(prop => true).ForEach(pi =>
    ///      {
    ///          Console.WriteLine("\t\t" + pi.Name);
    ///      });
    ///  });
    /// </example>
    public static class AssemblyEx{
        /// <summary>
        /// 列舉指定 Assembly 的所有 Type
        /// </summary>
        /// <param name="asm"></param>
        /// <param name="func">過濾條件</param>
        /// <returns></returns>
        /// <example>
        /// Assembly.GetExecutingAssembly().GetTypes( t => true).ForEach( (Type x) => Console.WriteLine(x) );
        /// </example>
        public static IEnumerable<Type> GetTypes(this Assembly asm, Func<Type, bool> func)
        {
            return (from t in asm.GetTypes()
                    where func(t)
                    select t);

            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IEnumerable<MethodInfo> GetMethods(this Type t, Func<MethodInfo, bool> func)
        {
            return t.Gets<MethodInfo>(() => t.GetMethods(), func);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetProperties(this Type t, Func<PropertyInfo, bool> func)
        {            
            return t.Gets<PropertyInfo>(() => t.GetProperties(), func);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IEnumerable<FieldInfo> GetFields(this Type t, Func<FieldInfo, bool> func)
        {            
            return t.Gets<FieldInfo>(() => t.GetFields(), func);
        }

        public static IEnumerable<EventInfo> GetEvents(this Type t, Func<EventInfo, bool> func)
        {
            return t.Gets<EventInfo>(() => t.GetEvents(), func);
        }

        public static IEnumerable<T> Gets<T>(this Type t, Func<T[]> get_lists_func, Func<T, bool> func)
        {
            return (from v in get_lists_func() where func(v) select v);
        }
    }