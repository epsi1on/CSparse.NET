﻿using CSparse.IO;
using CSparse.Storage;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CSparse.Tests
{
    static class ResourceLoader
    {
        private const string NS = "CSparse.Tests.{Type}.Data";

        private static Dictionary<string, object> cache = new Dictionary<string,object>();

        public static CompressedColumnStorage<T> Get<T>(string resource)
            where T : struct, IEquatable<T>, IFormattable
        {
            try
            {
                string type = (typeof(T) == typeof(double)) ? "Double" : "Complex";

                string path = NS.Replace("{Type}", type) + "." + resource;

                object obj;

                if (cache.TryGetValue(path, out obj))
                {
                    return (CompressedColumnStorage<T>)obj;
                }

                var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);

                var matrix = MatrixMarketReader.ReadMatrix<T>(stream);

                cache.Add(path, matrix);

                return matrix;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
