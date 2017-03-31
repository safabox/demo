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

    public class OperationResult<TResult>
    {
        #region Factory

        public static OperationResult<TResult> Success(TResult result)
        {
            return new OperationResult<TResult>(result);
        }

        public static OperationResult<TResult> Failed(params string[] errors)
        {
            return new OperationResult<TResult>(errors);
        }

        public static OperationResult<TResult> Failed(Exception ex)
        {
            return Failed(ex.Message);
        }

        #endregion

        public bool Succeeded { get; private set; }
        public IEnumerable<string> Errors { get; private set; }

        [JsonIgnore]
        public TResult Result { get; private set; }

        private OperationResult(IEnumerable<string> errors)
        {
            this.Succeeded = false;
            this.Errors = errors;
        }

        private OperationResult(TResult result)
        {
            this.Succeeded = true;
            this.Result = result;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
