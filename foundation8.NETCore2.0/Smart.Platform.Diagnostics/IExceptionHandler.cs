using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smart.Platform.Diagnostics
{
    public interface IExceptionHandler
    {
        void Handle(Exception ex);
    }
}
