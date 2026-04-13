using Newtonsoft.Json;
using SistemaPedidos.Domain.Interfaces;
using System;
using System.Collections.Generic;

namespace SistemaPedidos.Domain.Entities
{
    public abstract class EntidadeBase : IEntidadeBase
    {
        [JsonProperty]
        public Guid Id { get; protected set; }

        public abstract bool IsValid();

        protected Guid GenerateId()
        {
            return Guid.NewGuid();
        }

        [JsonIgnore]
        public List<string> ValidationErrors { get; protected set; } = new List<string>();
    }
}
