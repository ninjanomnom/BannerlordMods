using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using TaleWorlds.Library;

namespace GantryLib.ReflectionHelpers
{
    /// <summary>
    /// A type safe way of calling on private functions, keeping all reflection internalized
    /// </summary>
    /// <typeparam name="ownerType">The type that has the function to be called</typeparam>
    /// <typeparam name="signature">The function signature as a delegate</typeparam>
    public class PrivateFunc<ownerType, signature> where signature : Delegate
    {
        private readonly Dictionary<Type, signature> _cache = new Dictionary<Type, signature>();
        private readonly string _name;
        private const BindingFlags FLAGS = BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.NonPublic;

        public PrivateFunc(string funcName)
        {
            Debug.Assert(funcName != null, "PrivateFunc needs a function name to get.");

            var function = typeof(ownerType).GetMethod(funcName).CreateDelegate(typeof(signature)) as signature;

            Debug.Assert(function != null, "No function was found with the given signature.");

            _cache.Add(typeof(ownerType), function);
            _name = funcName;
        }
        
        /// <summary>
        /// Gets the cached delegate to call the private function
        /// </summary>
        /// <param name="instance">The instance to get the delegate for</param>
        /// <returns>A new delegate created from the instance passed in. Or old cached delegate</returns>
        public signature Get(ownerType instance)
        {
            var instanceType = instance.GetType();
            var function = _cache.GetValueSafe(instanceType);

            if(function != null)
            {
                return function;
            }

            function = instanceType.GetMethod(_name, FLAGS).CreateDelegate(typeof(signature)) as signature;
            _cache.Add(instanceType, function);
            return function;
        }
    }
}
