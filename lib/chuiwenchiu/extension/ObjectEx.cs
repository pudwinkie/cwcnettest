    public static class ObjectEx{
        
        public static T Clone<T>(this T obj)
        {
            if (obj != null)
            {
                var br = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                using (var ms = new MemoryStream(512))
                {
                    br.Serialize(ms, obj);
                    ms.Seek(0, SeekOrigin.Begin);
                    return (T)br.Deserialize(ms);
                }
            }
            else
            {
                return default(T);
            }

        }
    }