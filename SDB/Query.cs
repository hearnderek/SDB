﻿using System;

namespace SDB
{
    public class Query
    {
        public static Query Parse(Benumerator<char> en)
        {
            var s = en.Save();

            en.MoveNext();
            if (en.Current == ' ')
                en.MoveNext();

            if (en.Current == 'C')
            {
                return CreateTable.Parse(s);
            }
            else if (en.Current == 'F')
            {
                return Get.Parse(s);
            }
            else if (en.Current == 'I')
            {
                return Insert.Parse(s);
            }
            else
            {
                throw new Exception("Unrecognized first command");
            }
        }

    }
}