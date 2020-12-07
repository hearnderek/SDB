using System;
using System.Collections.Generic;

namespace SDB
{
    public class Benumerator
    {
        public static Benumerator<Char> AsLongString(IEnumerable<string> lines)
        {
            return new Benumerator<Char>(_AsLongString(lines));
        }

        private static IEnumerator<Char> _AsLongString(IEnumerable<string> lines)
        {
            char? last = null;
            char cur;
            foreach (string line in lines)
            {
                foreach (char c in line)
                {
                    if (c == '\t')
                        cur = ' ';
                    else
                        cur = c;

                    if (cur == ' ' && last.HasValue && last.Value == ' ')
                        continue;

                    yield return cur;
                    last = cur;
                }

                // replacing line endings with standard spaces
                if (!(last.HasValue && last.Value == ' '))
                    yield return ' ';
            }
        }
    }

    public class Benumerator<T> : IEnumerator<T>
    {
        public int EnumeratedCount { get; private set; }

        private List<T> prev = new List<T>();

        bool savePoints = false;

        private IEnumerator<T> underlying;

        public T Current => underlying.Current;

        object System.Collections.IEnumerator.Current => underlying.Current;


        public Benumerator(IEnumerator<T> ts)
        {
            underlying = ts;
        }


        public Benumerator<T> Save()
        {
            savePoints = true;
            return Gen(prev.Count);
        }

        private Benumerator<T> Gen(int count)
        {
            return new Benumerator<T>(GenE(count));
        }

        private IEnumerator<T> GenE(int count)
        {
            int i = count;
            while (true)
            {
                if (i == EnumeratedCount)
                {
                    if (MoveNext())
                        yield return Current;
                    else
                        break;
                }
                else if(i+1 == EnumeratedCount)
                {
                    yield return Current;
                }
                else
                {
                    yield return prev[i];
                }

                i++;
            }
        }

        public void Dispose()
        {
            underlying.Dispose();
        }

        public bool MoveNext()
        {
            if (savePoints && EnumeratedCount > 0)
                prev.Add(Current);
            EnumeratedCount++;
            return underlying.MoveNext();
        }

        public void Reset()
        {
            underlying.Reset();
        }



    }

}
