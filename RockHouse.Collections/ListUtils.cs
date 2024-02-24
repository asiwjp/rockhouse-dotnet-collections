﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RockHouse.Collections
{
    /// <summary>
    /// Provides utility methods for IList.
    /// </summary>
    public static class ListUtils
    {
        /// <summary>
        /// Get the number of elements in the list.
        /// This method is null-safe.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <param name="list">An instance of a list. Or null.</param>
        /// <returns>The number of elements in the list, 0 if null.</returns>
        public static int Count<T>(IList<T>? list)
        {
            return list?.Count ?? 0;
        }

        /// <summary>
        /// Returns an default list if the specified list is null or empty.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <param name="list">Target to determine if it is null or not.</param>
        /// <param name="defaultList">The default list.</param>
        /// <returns>If the specified list is null or empty, defaultList is returned. Otherwise, the original list is returned as is.</returns>
        public static IList<T> DefaultIfEmpty<T>(IList<T>? list, IList<T> defaultList)
        {
            return DefaultIfEmpty(list, () => defaultList);
        }

        /// <summary>
        /// Returns an default list if the specified list is null or empty.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <param name="list">Target to determine if it is null or not.</param>
        /// <param name="factory">Factory functor to generate default list.</param>
        /// <returns>If the specified list is null or empty, the default value generated by the factory is returned. Otherwise, the original list is returned as is.</returns>
        public static IList<T> DefaultIfEmpty<T>(IList<T>? list, Func<IList<T>> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            if (IsEmpty(list))
            {
                return factory();
            }
            return list;
        }

        /// <summary>
        /// Returns an default list if the specified list is null.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <param name="list">Target to determine if it is null or not.</param>
        /// <param name="defaultList">The default list.</param>
        /// <returns>If the specified list is null, defaultList is returned. Otherwise, the original list is returned as is.</returns>
        public static IList<T> DefaultIfNull<T>(IList<T>? list, IList<T> defaultList)
        {
            return DefaultIfNull(list, () => defaultList);
        }

        /// <summary>
        /// Returns an default list if the specified list is null.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <param name="list">Target to determine if it is null or not.</param>
        /// <param name="factory">Factory functor to generate default list.</param>
        /// <returns>If the specified list is null, the default value generated by the factory is returned. Otherwise, the original list is returned as is.</returns>
        public static IList<T> DefaultIfNull<T>(IList<T>? list, Func<IList<T>> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            if (list == null)
            {
                return factory();
            }
            return list;
        }

        /// <summary>
        /// Returns an empty list if the specified list is null.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <param name="list">Target to determine if it is null or not.</param>
        /// <returns>If the specified list is null, an empty List is returned. Otherwise, the specified list are returned as is.</returns>
        public static IList<T> EmptyIfNull<T>(IList<T>? list)
        {
            if (list == null)
            {
                return new List<T>();
            }
            return list;
        }

        /// <summary>
        /// Determines if the list is empty or not.
        /// This method is null-safe.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <param name="list">An instance of a list. Or null.</param>
        /// <returns>True if the list is null or the number of elements in the list is zero. otherwise False.</returns>
        public static bool IsEmpty<T>(IList<T>? list)
        {
            return Count(list) == 0;
        }

        /// <summary>
        /// Determines if the list is empty or not.
        /// This method is null-safe.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <param name="list">An instance of a list. Or null.</param>
        /// <returns>True if the number of elements in the list is non-zero, false if the number of elements is zero or the list is null.</returns>
        public static bool IsNotEmpty<T>(IList<T>? list)
        {
            return Count(list) != 0;
        }

        /// <summary>
        /// Removes and returns the element at the specified index from the list.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <param name="list">An instance of a list.</param>
        /// <param name="index">Index of the element to be removed. If -1 is specified, the last element is removed.</param>
        /// <returns>Value of the element removed.</returns>
        /// <exception cref="ArgumentOutOfRangeException">index is less than -1.</exception>
        /// <exception cref="InvalidOperationException">If the list is empty.</exception>
        public static T Pop<T>(IList<T> list, int index = -1)
        {
            if (list.Count == 0)
            {
                throw new InvalidOperationException("list is empty.");
            }
            if (index < -1 || index >= list.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            if (index == -1)
            {
                index = list.Count - 1;
            }
            var popped = list[index];
            list.RemoveAt(index);
            return popped;
        }

        /// <summary>
        /// Removes and returns the element at the specified index from the list.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <param name="list">An instance of a list.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <param name="index">Index of the element to be removed. If -1 is specified, the last element is removed.</param>
        /// <returns>Returns the value of the element indicated by index. If the list is empty or index indicates after the last element in the list, defaultValue is returned.</returns>
        /// <exception cref="ArgumentOutOfRangeException">index is less than -1.</exception>
        public static T PopOrDefault<T>(IList<T> list, T defaultValue = default, int index = -1)
        {
            return PopOrDefault(list, () => defaultValue, index);
        }

        /// <summary>
        /// Removes and returns the element at the specified index from the list.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <param name="list">An instance of a list.</param>
        /// <param name="index">Index of the element to be removed. If -1 is specified, the last element is removed.</param>
        /// <param name="defaultValueFactory">Factory function to generate default values.</param>
        /// <returns>Returns the value of the element indicated by index. If the list is empty or index indicates after the last element in the list, defaultValue is returned.</returns>
        /// <exception cref="ArgumentOutOfRangeException">index is less than -1.</exception>
        public static T PopOrDefault<T>(IList<T> list, Func<T> defaultValueFactory, int index = -1)
        {
            if (defaultValueFactory == null)
            {
                throw new ArgumentNullException(nameof(defaultValueFactory));
            }
            if (index < -1)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            if (index == -1)
            {
                index = list.Count - 1;
            }
            if (list.Count == 0 || index >= list.Count)
            {
                return defaultValueFactory();
            }
            return Pop(list, index);
        }

        /// <summary>
        /// Adds a value to the end of the list.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <param name="list">An instance of a list.</param>
        /// <param name="values">Value to be added.</param>
        public static void Push<T>(IList<T> list, params T[] values)
        {
            foreach (var item in values)
            {
                list.Add(item);
            }
        }

        /// <summary>
        /// It shifts each element of the list forward one by one and returns the first element pushed out.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <param name="list">An instance of a list.</param>
        /// <returns>The value of the first element is returned.</returns>
        /// <exception cref="InvalidOperationException">If the list is empty.</exception>
        public static T Shift<T>(IList<T> list)
        {
            return Pop(list, 0);
        }

        /// <summary>
        /// It shifts each element of the list forward one by one and returns the first element pushed out.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <param name="list">An instance of a list.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The value of the first element is returned. If the list is empty, defaultValue is returned.</returns>
        public static T ShiftOrDefault<T>(IList<T> list, T defaultValue = default)
        {
            return PopOrDefault(list, () => defaultValue, 0);
        }

        /// <summary>
        /// It shifts each element of the list forward one by one and returns the first element pushed out.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <param name="list">An instance of a list.</param>
        /// <param name="defaultValueFactory">Factory function to generate default value.</param>
        /// <returns>The value of the first element is returned. If the list is empty, defaultValue is returned.</returns>
        public static T ShiftOrDefault<T>(IList<T> list, Func<T> defaultValueFactory)
        {
            return PopOrDefault(list, defaultValueFactory, 0);
        }

        /// <summary>
        /// Adds a value to the beginning of the list.
        /// </summary>
        /// <typeparam name="T">The type of elements.</typeparam>
        /// <param name="list">An instance of a list.</param>
        /// <param name="values">Value to be added.</param>
        public static void Unshift<T>(IList<T> list, params T[] values)
        {
            foreach (var value in values.Reverse())
            {
                list.Insert(0, value);
            }
        }
    }
}
