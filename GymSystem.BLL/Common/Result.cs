using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystem.BLL.Common
{
    public sealed record Result(bool success,string? error = null, ResultKind ResultKind = ResultKind.OK )
    {
        public static Result OK() => new(true);
        public static Result Fail(string error, ResultKind kind = ResultKind.Conflict) => new(false, error, kind);
        public static Result NotFound(string error = "NotFound") => new(false, error, ResultKind.NoFound);
        public static Result ValidationFailed(string error = "Validation Failed") => new(false, error, ResultKind.ValidationFailed);

    }

    public sealed record Result<T>(bool success, T? value, string? error = null, ResultKind ResultKind = ResultKind.OK)
    {
        public static Result<T> OK(T value) => new(true,value);
        public static Result<T> Fail(string error, ResultKind kind = ResultKind.Conflict) => new(false,default, error, kind);
        public static Result<T> NotFound(string error = "NotFound") => new(false,default, error, ResultKind.NoFound);
        public static Result<T> ValidationFailed(string error = "Validation Failed") => new(false, default,error, ResultKind.ValidationFailed);

    }
}
