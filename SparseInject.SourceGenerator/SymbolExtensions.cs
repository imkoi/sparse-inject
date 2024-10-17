using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace SparseInject.SourceGenerator
{
    public static class SymbolExtensions
    {
        public static bool CanBeCallFromInternal(this ISymbol symbol)
        {
            return symbol.DeclaredAccessibility >= Accessibility.Internal;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer)
        {
            return DistinctByIterator(source, keySelector, comparer);
        }

        static IEnumerable<TSource> DistinctByIterator<TSource, TKey>(IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? comparer)
        {
            using IEnumerator<TSource> enumerator = source.GetEnumerator();

            if (enumerator.MoveNext())
            {
                var set = new HashSet<TKey>(comparer);
                do
                {
                    TSource element = enumerator.Current;
                    if (set.Add(keySelector(element)))
                    {
                        yield return element;
                    }
                }
                while (enumerator.MoveNext());
            }
        }
    }
}
