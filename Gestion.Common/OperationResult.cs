using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Gestion.Common
{
    public class OperationResult
    {
        #region Factory

        public static OperationResult Success()
        {
            return new OperationResult(true, new string[] { });
        }

        public static OperationResult Failed(params string[] errors)
        {
            return new OperationResult(false, errors);
        }

        public static OperationResult Failed(Exception ex)
        {
            return Failed(ex.Message);
        }

        #endregion

        public bool Succeeded { get; private set; }
        public IEnumerable<string> Errors { get; private set; }

        private OperationResult(bool succeeded, IEnumerable<string> errors)
        {
            this.Succeeded = succeeded;
            this.Errors = errors;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
