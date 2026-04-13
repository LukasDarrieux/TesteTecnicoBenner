using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace SistemaPedidos.Infraestrutura.Repository
{
    public class BaseRepository<T> : IDisposable
    {
        protected string DirDB = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
        private readonly string FilePath;
        public BaseRepository(string fileName)
        {
            FilePath = Path.Combine(DirDB, fileName);
            
            if (!Directory.Exists(DirDB))
            {
                Directory.CreateDirectory(DirDB);
            }
            if (!File.Exists(FilePath))
            {
                File.Create(FilePath).Close();
            }
        }

        protected List<T> Load()
        {
            var json = File.ReadAllText(FilePath);

            if (string.IsNullOrWhiteSpace(json))
                return new List<T>();

            return JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
        }

        protected void Save(List<T> data)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);

            File.WriteAllText(FilePath, json);
        }

        public void Dispose()
        {
            
        }
    }
}
