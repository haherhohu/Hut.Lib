using System;
using System.Collections.Generic;
using System.Linq;

namespace Hut
{
    internal static class HutMediatorHelper
    {
        public static void Register<T1, T2>(T1 handlers, Enum token, dynamic doSomething) where T1 : Dictionary<Enum, T2>
        {
            if (handlers.Keys.Contains(token))
            {
                handlers[token] += doSomething;
            }
            else
                handlers.Add(token, doSomething);
        }

        public static void UnRegister<T1, T2>(T1 handlers, Enum token, dynamic doSomething) where T1 : Dictionary<Enum, T2>
        {
            if (handlers.Keys.Contains(token))
            {
                handlers[token] -= doSomething;
                if (handlers[token] == null)
                    handlers.Remove(token);
            }
        }
    }

    // Register 에서 해당 타입에 대해 등록
    // unregister 에서 해제
    // notify 에서 액션
    // Action<T> 가 a Action이 아니고 Action Chain 이므로 이렇게.
    public class HutBaseMediator<T>
    {
        private object LordsOfTheLockerroom = new object();
        protected Dictionary<Enum, T> handlers = new Dictionary<Enum, T>();

        public void Register(Enum token, T doSomething)
        {
            lock (LordsOfTheLockerroom)
            {
                HutMediatorHelper.Register<Dictionary<Enum, T>, T>(handlers, token, doSomething);
            }
        }

        public void UnRegister(Enum token, T doSomething)
        {
            lock (LordsOfTheLockerroom)
            {
                HutMediatorHelper.UnRegister<Dictionary<Enum, T>, T>(handlers, token, doSomething);
            }
        }
    }

    #region real mediators

    public class HutMediator : HutBaseMediator<Action>
    {
        public void Notify(Enum token)
        {
            if(handlers.Keys.Contains(token))
                handlers[token]();
        }
    }

    public class HutMediator<T> : HutBaseMediator<Action<T>>
    {
        public void Notify(Enum token, T argument)
        {
            if (handlers.Keys.Contains(token))
                handlers[token](argument);
        }
    }

    public class HutMediator<T1, T2> : HutBaseMediator<Action<T1, T2>>
    {
        public void Notify(Enum token, T1 arg1, T2 arg2)
        {
            if (handlers.Keys.Contains(token))
                handlers[token](arg1, arg2);
        }
    }

    public class HutMediator<T1, T2, T3> : HutBaseMediator<Action<T1, T2, T3>>
    {
        public void Notify(Enum token, T1 arg1, T2 arg2, T3 arg3)
        {
            if (handlers.Keys.Contains(token))
                handlers[token](arg1, arg2, arg3);
        }
    }

    public class HutMediator<T1, T2, T3, T4> : HutBaseMediator<Action<T1, T2, T3, T4>>
    {
        public void Notify(Enum token, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (handlers.Keys.Contains(token))
                handlers[token](arg1, arg2, arg3, arg4);
        }
    }

    public class HutMediator<T1, T2, T3, T4, T5> : HutBaseMediator<Action<T1, T2, T3, T4, T5>>
    {
        public void Notify(Enum token, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            if (handlers.Keys.Contains(token))
                handlers[token](arg1, arg2, arg3, arg4, arg5);
        }
    }

    public class HutMediator<T1, T2, T3, T4, T5, T6> : HutBaseMediator<Action<T1, T2, T3, T4, T5, T6>>
    {
        public void Notify(Enum token, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            if (handlers.Keys.Contains(token))
                handlers[token](arg1, arg2, arg3, arg4, arg5, arg6);
        }
    }

    public class HutMediator<T1, T2, T3, T4, T5, T6, T7> : HutBaseMediator<Action<T1, T2, T3, T4, T5, T6, T7>>
    {
        public void Notify(Enum token, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            if (handlers.Keys.Contains(token))
                handlers[token](arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }
    }

    public class HutMediator<T1, T2, T3, T4, T5, T6, T7, T8> : HutBaseMediator<Action<T1, T2, T3, T4, T5, T6, T7, T8>>
    {
        public void Notify(Enum token, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            if (handlers.Keys.Contains(token))
                handlers[token](arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }
    }

    public class HutMediator<T1, T2, T3, T4, T5, T6, T7, T8, T9> : HutBaseMediator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9>>
    {
        public void Notify(Enum token, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            if (handlers.Keys.Contains(token))
                handlers[token](arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }
    }

    public class HutMediator<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : HutBaseMediator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>
    {
        public void Notify(Enum token, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            if (handlers.Keys.Contains(token))
                handlers[token](arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }
    }

    public class HutMediator<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : HutBaseMediator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>
    {
        public void Notify(Enum token, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
        {
            if (handlers.Keys.Contains(token))
                handlers[token](arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
        }
    }

    public class HutMediator<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : HutBaseMediator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>
    {
        public void Notify(Enum token, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
        {
            if (handlers.Keys.Contains(token))
                handlers[token](arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
        }
    }

    public class HutMediator<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : HutBaseMediator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>
    {
        public void Notify(Enum token, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)
        {
            if (handlers.Keys.Contains(token))
                handlers[token](arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
        }
    }

    public class HutMediator<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : HutBaseMediator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>>
    {
        public void Notify(Enum token, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14)
        {
            if (handlers.Keys.Contains(token))
                handlers[token](arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
        }
    }

    public class HutMediator<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : HutBaseMediator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>>
    {
        public void Notify(Enum token, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15)
        {
            if (handlers.Keys.Contains(token))
                handlers[token](arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
        }
    }

    public class HutMediator<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> : HutBaseMediator<Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>>
    {
        public void Notify(Enum token, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16)
        {
            if (handlers.Keys.Contains(token))
                handlers[token](arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
        }
    }

    #endregion real mediators

    #region singleton mediators

    public class HutSingleMediator : HutSingleton<HutMediator>
    {
    }

    public class HutSingleMediator<T> : HutSingleton<HutMediator<T>>
    {
    }

    public class HutSingleMediator<T1, T2> : HutSingleton<HutMediator<T1, T2>>
    {
    }

    public class HutSingleMediator<T1, T2, T3> : HutSingleton<HutMediator<T1, T2, T3>>
    {
    }

    public class HutSingleMediator<T1, T2, T3, T4> : HutSingleton<HutMediator<T1, T2, T3, T4>>
    {
    }

    public class HutSingleMediator<T1, T2, T3, T4, T5> : HutSingleton<HutMediator<T1, T2, T3, T4, T5>>
    {
    }

    public class HutSingleMediator<T1, T2, T3, T4, T5, T6> : HutSingleton<HutMediator<T1, T2, T3, T4, T5, T6>>
    {
    }

    public class HutSingleMediator<T1, T2, T3, T4, T5, T6, T7> : HutSingleton<HutMediator<T1, T2, T3, T4, T5, T6, T7>>
    {
    }

    public class HutSingleMediator<T1, T2, T3, T4, T5, T6, T7, T8> : HutSingleton<HutMediator<T1, T2, T3, T4, T5, T6, T7, T8>>
    {
    }

    public class HutSingleMediator<T1, T2, T3, T4, T5, T6, T7, T8, T9> : HutSingleton<HutMediator<T1, T2, T3, T4, T5, T6, T7, T8, T9>>
    {
    }

    public class HutSingleMediator<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : HutSingleton<HutMediator<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>
    {
    }

    public class HutSingleMediator<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : HutSingleton<HutMediator<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>
    {
    }

    public class HutSingleMediator<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : HutSingleton<HutMediator<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>
    {
    }

    public class HutSingleMediator<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : HutSingleton<HutMediator<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>
    {
    }

    public class HutSingleMediator<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : HutSingleton<HutMediator<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>>
    {
    }

    public class HutSingleMediator<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : HutSingleton<HutMediator<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>>
    {
    }

    public class HutSingleMediator<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> : HutSingleton<HutMediator<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>>
    {
    }

    #endregion singleton mediators
}