using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Basket.Common.Types
{
  public struct RopResult<A> : IEquatable<RopResult<A>>
    {
        private enum RopResultState : byte
        {
            Succeed,
            Error
        }

        internal readonly A Success;
        internal readonly Exception Error;

        private readonly RopResultState _state;

        public bool IsSucceed => _state == RopResultState.Succeed;

        public RopResult(A success)
        {
            Success = success;
            Error = default(Exception);
            _state = RopResultState.Succeed;
        }

        public RopResult(Exception error)
        {
            Success = default(A);
            Error = error;
            _state = RopResultState.Error;
        }

        public static RopResult<A> Ok(A success)
        {
            return new RopResult<A>(success);
        }

        public static RopResult<A> Failure(Exception error)
        {
            return new RopResult<A>(error);
        }

        public RopResult<C> Map<C>(Func<A, C> func)
        {
            if (_state == RopResultState.Error)
            {
                return RopResult<C>.Failure(Error);
            }
            return RopResult<C>.Ok(func(Success));
        }

        public async Task<RopResult<C>> MapAsync<C>(Func<A, Task<C>> func)
        {
            if (_state == RopResultState.Error)
            {
                return RopResult<C>.Failure(Error);
            }
            return RopResult<C>.Ok(await func(Success));
        }

        public RopResult<C> Bind<C>(Func<A, RopResult<C>> func)
        {
            if (_state == RopResultState.Error)
            {
                return RopResult<C>.Failure(Error);
            }
            return func(Success);
        }

        public async Task<RopResult<A>> BindErrorAsync(Func<Exception, Task<RopResult<A>>> func)
        {
            if (_state == RopResultState.Succeed)
            {
                return Ok(Success);
            }
            return await func(Error);
        }


        public RopResult<A> BindError(Func<Exception, RopResult<A>> func)
        {
            if (_state == RopResultState.Succeed)
            {
                return Ok(Success);
            }
            return func(Error);
        }

        public async Task<RopResult<C>> BindAsync<C>(Func<A, Task<RopResult<C>>> func)
        {
            if (_state == RopResultState.Error)
            {
                return RopResult<C>.Failure(Error);
            }
            return await func(Success);
        }

        public A IfFailure(Func<Exception, A> f)
        {
            if (_state == RopResultState.Error)
            {
                return f(Error);
            }
            return Success;
        }

        public A IfFailure(A val)
        {
            if (_state == RopResultState.Error)
            {
                return val;
            }
            return Success;
        }


        public async Task IfFailureAsync(Func<Exception, Task> f)
        {
            if (_state == RopResultState.Error)
            {
                await f(Error);
            }
        }

        public void IfFailureAsync(Action<Exception> f)
        {
            if (_state == RopResultState.Error)
            {
                f(Error);
            }
        }

        public void IfSucc(Action<A> f)
        {
            if (_state == RopResultState.Succeed)
            {
                f(Success);
            }
        }

        public B Match<B>(Func<A, B> Succ, Func<Exception, B> Fail)
        {
            if (_state == RopResultState.Succeed)
            {
                return Succ(Success);
            }

            return Fail(Error);
        }

        public void Match(Action<A> Succ, Action<Exception> Fail)
        {
            if (_state == RopResultState.Succeed)
            {
                Succ(Success);
            }
            else
            {
                Fail(Error);
            }
        }

        public Task<B> MatchAsync<B>(Func<A, Task<B>> Succ, Func<Exception, Task<B>> Fail)
        {
            if (_state == RopResultState.Succeed)
            {
                return Succ(Success);
            }
            else
            {
                return Fail(Error);
            }
        }

        public Task MatchAsync(Func<A, Task> Succ, Func<Exception, Task> Fail)
        {
            if (_state == RopResultState.Succeed)
            {
                return Succ(Success);
            }

            return Fail(Error);
        }

        public bool Equals(RopResult<A> other)
        {
            return EqualityComparer<A>.Default.Equals(Success, other.Success) && Equals(Error, other.Error) && _state == other._state;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is RopResult<A> other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = EqualityComparer<A>.Default.GetHashCode(Success);
                hashCode = (hashCode * 397) ^ (Error != null ? Error.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int)_state;
                return hashCode;
            }
        }

        public static implicit operator RopResult<A>(A value) =>
            new RopResult<A>(value);
    }

    public static class RopResult 
    {
        public static RopResult<Unit> UnitResult = RopResult<Unit>.Ok(Unit.Value);
    } 

    public static class RopResultExtensions
    {
        public static async Task<RopResult<B>> BindTask<A, B>(this Task<RopResult<A>> task, Func<A, Task<RopResult<B>>> func)
        {
            var result = await task;
            return await result.BindAsync(func);
        }

        public static async Task<RopResult<A>> BindErrorTask<A>(this Task<RopResult<A>> task, Func<Exception, RopResult<A>> func)
        {
            var result = await task;
            return result.BindError(func);
        }

        public static async Task<RopResult<A>> BindErrorTask<A>(this Task<RopResult<A>> task, Func<Exception, Task<RopResult<A>>> func)
        {
            var result = await task;
            return await result.BindErrorAsync(func);
        }

        public static async Task<RopResult<B>> MapTask<A, B>(this Task<RopResult<A>> task, Func<A, Task<B>> func)
        {
            var result = await task;
            return await result.MapAsync(func);
        }

        public static async Task<RopResult<B>> BindTask<A, B>(this Task<RopResult<A>> task, Func<A, RopResult<B>> func)
        {
            var result = await task;
            return result.Bind(func);
        }

        public static async Task<RopResult<B>> MapTask<A, B>(this Task<RopResult<A>> task, Func<A, B> func)
        {
            var result = await task;
            return result.Map(func);
        }

        public static async Task<B> MatchTask<A, B>(this Task<RopResult<A>> task, Func<A, Task<B>> Succ, Func<Exception, Task<B>> Fail)
        {
            var result = await task;
            return await result.MatchAsync(Succ, Fail);
        }

        public static async Task<B> MatchTask<A, B>(this Task<RopResult<A>> task, Func<A, B> Succ, Func<Exception, B> Fail)
        {
            var result = await task;
            return result.Match(Succ, Fail);
        }

        public static async Task MatchTask<A>(this Task<RopResult<A>> task, Func<A, Task> Succ, Func<Exception, Task> Fail)
        {
            var result = await task;
            await result.MatchAsync(Succ, Fail);
        }

        public static async Task MatchTask<A>(this Task<RopResult<A>> task, Action<A> Succ, Action<Exception> Fail)
        {
            var result = await task;
            result.Match(Succ, Fail);
        }
    }
}