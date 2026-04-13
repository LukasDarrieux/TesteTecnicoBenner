using System;
using System.Collections.Generic;

namespace SistemaPedidos.Aplicacao.CustomException
{
    public class ValidationException : Exception
    {
        public List<string> Validations { get; private set; }

        public ValidationException(List<string> validations)
        {
            Validations = validations;
        }
    }
}
