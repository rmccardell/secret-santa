using System;
using System.Collections.Generic;
using System.Threading;

namespace secret_santa.infrastructure.Extensions
{

    static class ListHelpers
    {

        public static void ShuffleList<E>(this IList<E> list)
        {
            if (list.Count > 1)
            {
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    E tmp = list[i];
                    int randomIndex = StaticRandom.Instance.Next(i + 1);

                    //Swap elements
                    list[i] = list[randomIndex];
                    list[randomIndex] = tmp;
                }
            }
        }

        public static T GetRandom<T> ( this IList<T> list){


                if(list == null || list.Count == 0)
                {
                    return default(T);
                }
                
                var randomNdx = StaticRandom.Instance.Next(0, list.Count - 1);
                var randomItem = list[randomNdx];
                return randomItem;

        }

    }


    //https://stackoverflow.com/questions/767999/random-number-generator-only-generating-one-random-number
    public static class StaticRandom
    {
        private static int seed;

        private static readonly ThreadLocal<Random> threadLocal = new ThreadLocal<Random>
            (() => new Random(Interlocked.Increment(ref seed)));

        static StaticRandom()
        {
            seed = Environment.TickCount;
        }

        public static Random Instance { get { return threadLocal.Value; } }
    }



}