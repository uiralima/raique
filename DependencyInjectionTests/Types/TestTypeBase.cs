using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyInjectionTests.Types
{
    public abstract class TestTypeBase
    {
        private int _count = 0;
        public int Count => _count;

        public void IncCount()
        {
            _count++;
        }
    }
}
