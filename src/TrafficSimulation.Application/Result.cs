﻿namespace TrafficSimulation.Application
{
    public class Result<T>
    {
        public T? Response { get; private set; }

        public Exception? InnerException { get; private set; }

        public bool Succeeded { get; private set; }

        private Result() { }

        public static Result<T> Success(T response)
        {
            return new Result<T> { Response = response, Succeeded = true };
        }

        public static Result<T> Failure(Exception exception)
        {
            return new Result<T> { InnerException = exception, Succeeded = false };
        }
    }
}
