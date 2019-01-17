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
  }
}
