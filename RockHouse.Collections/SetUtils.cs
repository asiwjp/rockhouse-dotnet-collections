﻿using System;
using System.Collections.Generic;

namespace RockHouse.Collections
{
    /// <summary>
    /// Provides utility methods for ISet.
    /// </summary>
    public static class SetUtils
    {
        /// <summary>
        /// Get the number of elements in the set.
        /// This method is null-safe.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <param name="set">An instance of a set. Or null.</param>
        /// <returns>The number of elements in the set, 0 if null.</returns>
        public static int Count<T>(ISet<T>? set)
        {
            return set?.Count ?? 0;
        }

        /// <summary>
        /// Returns an default set if the specified set is null or empty.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <param name="set">Target to determine if it is null or not.</param>
        /// <param name="defaultSet">The default set.</param>
        /// <returns>If the specified set is null or empty, defaultSet is returned. Otherwise, the original set is returned as is.</returns>
        public static ISet<T> DefaultIfEmpty<T>(ISet<T>? set, ISet<T> defaultSet)
        {
            return DefaultIfEmpty(set, () => defaultSet);
        }

        /// <summary>
        /// Returns an default set if the specified set is null or empty.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <param name="set">Target to determine if it is null or not.</param>
        /// <param name="factory">Factory functor to generate default set.</param>
        /// <returns>If the specified set is null or empty, the default value generated by the factory is returned. Otherwise, the original set is returned as is.</returns>
        public static ISet<T> DefaultIfEmpty<T>(ISet<T>? set, Func<ISet<T>> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            if (IsEmpty(set))
            {
                return factory();
            }
            return set;
        }

        /// <summary>
        /// Returns an default set if the specified set is null.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <param name="set">Target to determine if it is null or not.</param>
        /// <param name="defaultSet">The default set.</param>
        /// <returns>If the specified set is null, defaultSet is returned. Otherwise, the original set is returned as is.</returns>
        public static ISet<T> DefaultIfNull<T>(ISet<T>? set, ISet<T> defaultSet)
        {
            return DefaultIfNull(set, () => defaultSet);
        }

        /// <summary>
        /// Returns an default set if the specified set is null.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <param name="set">Target to determine if it is null or not.</param>
        /// <param name="factory">Factory functor to generate default set.</param>
        /// <returns>If the specified set is null, the default value generated by the factory is returned. Otherwise, the original set is returned as is.</returns>
        public static ISet<T> DefaultIfNull<T>(ISet<T>? set, Func<ISet<T>> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            if (set == null)
            {
                return factory();
            }
            return set;
        }

        /// <summary>
        /// Returns an empty set if the specified set is null.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <param name="set">Target to determine if it is null or not.</param>
        /// <returns>If the specified list is null, an empty HashSet is returned. Otherwise, the specified list are returned as is.</returns>
        public static ISet<T> EmptyIfNull<T>(ISet<T>? set)
        {
            if (set == null)
            {
                return new HashSet<T>();
            }
            return set;
        }

        /// <summary>
        /// Determines if the set is empty or not.
        /// This method is null-safe.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <param name="set">An instance of a set. Or null.</param>
        /// <returns>True if the set is null or the number of elements in the set is zero. otherwise False.</returns>
        public static bool IsEmpty<T>(ISet<T>? set)
        {
            return Count(set) == 0;
        }

        /// <summary>
        /// Determines if the set is empty or not.
        /// This method is null-safe.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <param name="set">An instance of a set. Or null.</param>
        /// <returns>True if the number of elements in the set is non-zero, false if the number of elements is zero or the set is null.</returns>
        public static bool IsNotEmpty<T>(ISet<T>? set)
        {
            return Count(set) != 0;
        }
    }
}
