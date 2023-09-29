using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeFlix.Catalog.Application.Exceptions;
public abstract class ApplicationException : Exception
{
    protected ApplicationException(string? message) : base(message)
    {

    }
}
