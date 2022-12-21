using System.Collections.Generic;

namespace Bookmyslot.SharedKernel.ValueObject
{
    public class Result<T> : BaseValueObject
    {
        public T Value { get; set; }
        public ResultType ResultType { get; set; }
        public List<string> Messages { get; set; }

        public Result()
        {
            this.ResultType = ResultType.Success;
        }


        public static Result<T> Success(T result)
        {
            var response = new Result<T> { ResultType = ResultType.Success, Value = result };

            return response;
        }

        public static Result<T> Empty(List<string> errorMessages)
        {
            var response = new Result<T> { ResultType = ResultType.Empty, Messages = errorMessages };

            return response;
        }


        public static Result<T> Error(List<string> errorMessages)
        {
            var response = new Result<T> { ResultType = ResultType.Error, Messages = errorMessages };

            return response;
        }

        public static Result<T> ValidationError(List<string> validationMessage)
        {
            var response = new Result<T>
            {
                ResultType = ResultType.ValidationError,
                Messages = validationMessage,
            };

            return response;
        }

    }
}
