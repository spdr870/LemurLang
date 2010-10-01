using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading;

namespace LemurLang.Test.Tools
{
    public class CultureContext : IDisposable
    {
        private CultureInfo _oldCulture;
        
        public CultureContext(CultureInfo cultureInfo)
        {
            _oldCulture = Thread.CurrentThread.CurrentCulture;

            Thread.CurrentThread.CurrentCulture = cultureInfo;
        }

        public void Dispose()
        {
            Thread.CurrentThread.CurrentCulture = _oldCulture;
        }
    }
}
