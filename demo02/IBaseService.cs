﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo02
{
    interface IBaseService<T> where T : class
    {
        void Add(T model);
        void Remove(T model);
        void Save<T1>(T1 model);

    }
}
