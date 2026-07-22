using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WF.SDK.Models
{
  [Serializable]
  public class ApiResult<T>
  {
    public bool Success { get; set; }
    public string ErrorString { get; set; }
    public string InfoString { get; set; }
    public T Result { get; set; }

    public ApiResult() { }

    public ApiResult(bool success, string error, T result) { this.Success = success; this.ErrorString = error; this.Result = result; }

    public ApiResult<U> ToFailureResult<U>()
    {
      if (this.Success) { throw new InvalidOperationException("An ApiResult<T> cannot be converted to an ApiResult<U> if the original result is successful."); }
      return new ApiResult<U>
      {
        Success = false,
        ErrorString = this.ErrorString,
        InfoString = this.InfoString
      };
    }
  }
}
