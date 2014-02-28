// By M. Eugene Andrews
using System;
using System.Collections.Generic;

namespace HRUC {
    /// <summary>
    /// Provides several methods for creating standard comparer types.
    /// </summary>
    public static class CommonComparer {

        private class InternalComparer<T> : IComparer<T>, IEqualityComparer<T>, IComparable<T>, IEquatable<T> {

            private readonly Func<T, T, int> CompareMethod;
            private readonly Func<T, T, bool> EqualityEqualsMethod;
            private readonly Func<T, int> GetHashCodeMethod;
            private readonly Func<T, int> CompareToMethod;
            private readonly Func<T, bool> EquitableEqualsMethod;

            #region IComparer implementation

            public InternalComparer(Func<T, T, int> compareMethod) {
                CompareMethod = compareMethod;
            }

            int IComparer<T>.Compare(T x, T y) {
                ThrowIfNotImplemented(CompareMethod);
                return CompareMethod(x, y);
            }

            #endregion

            #region IEqualityComparer Implementation

            public InternalComparer(Func<T, T, bool> equalsMethod, Func<T, int> getHashCodeMethod) {
                EqualityEqualsMethod = equalsMethod;
                GetHashCodeMethod = getHashCodeMethod;
            }

            bool IEqualityComparer<T>.Equals(T x, T y) {
                ThrowIfNotImplemented(EqualityEqualsMethod);
                return EqualityEqualsMethod(x, y);
            }

            int IEqualityComparer<T>.GetHashCode(T obj) {
                ThrowIfNotImplemented(GetHashCodeMethod);
                return GetHashCodeMethod(obj);
            }

            #endregion

            #region IComparable Implementation

            public InternalComparer(Func<T, int> compareToMethod) {
                CompareToMethod = compareToMethod;
            }

            int IComparable<T>.CompareTo(T other) {
                ThrowIfNotImplemented(CompareToMethod);
                return CompareToMethod(other);
            }

            #endregion

            #region IEquitable Implementation

            public InternalComparer(Func<T, bool> equalsMethod) {
                EquitableEqualsMethod = equalsMethod;
            }

            bool IEquatable<T>.Equals(T other) {
                ThrowIfNotImplemented(EquitableEqualsMethod);
                return EquitableEqualsMethod(other);
            }

            #endregion

            private static void ThrowIfNotImplemented(Delegate method) {
                if (method == null) {
                    throw new NotImplementedException("This method is not implemented for this instance.");
                }
            }

        }

        /// <summary>
        /// Gets a standard <see cref="IComparer{T}"/> instance using the specified compare method.
        /// </summary>
        /// <typeparam name="T">The type to comparer.</typeparam>
        /// <param name="compareMethod">The method to use in the comparison.</param>
        /// <returns>A new <see cref="IComparer{T}"/> instance.</returns>
        public static IComparer<T> GetComparer<T>(Func<T, T, int> compareMethod) {
            return new InternalComparer<T>(compareMethod);
        }

        /// <summary>
        /// Gets a standard <see cref="IEqualityComparer{T}"/> instance using the specified equals and gethascode methods.
        /// </summary>
        /// <typeparam name="T">The type to compare.</typeparam>
        /// <param name="equalsMethod">The method used to determine equality.</param>
        /// <param name="getHashCodeMethod">The method used to create / determine the hascode for comparison.</param>
        /// <returns>A new <see cref="IEqualityComparer{T}"/> instance.</returns>
        public static IEqualityComparer<T> GetEqualityComparer<T>(Func<T, T, bool> equalsMethod, Func<T, int> getHashCodeMethod) {
            return new InternalComparer<T>(equalsMethod, getHashCodeMethod);
        }

        /// <summary>
        /// Gets a standard <see cref="IComparable{T}"/> instance using the specified compare method.
        /// </summary>
        /// <typeparam name="T">The type to compare.</typeparam>
        /// <param name="compareToMethod">The method used to compare.</param>
        /// <returns>A new <see cref="IComparable{T}"/> instance.</returns>
        public static IComparable<T> GetComparable<T>(Func<T, int> compareToMethod) {
            return new InternalComparer<T>(compareToMethod);
        }

        /// <summary>
        /// Gets a standard <see cref="IEquatable{T}"/> instance using the specified equals method.
        /// </summary>
        /// <typeparam name="T">The type to compare.</typeparam>
        /// <param name="equalsMethod">The method used to determine equality.</param>
        /// <returns>A new <see cref="IEquatable{T}"/> instance.</returns>
        public static IEquatable<T> GetEquitable<T>(Func<T, bool> equalsMethod) {
            return new InternalComparer<T>(equalsMethod);
        }
    }
}