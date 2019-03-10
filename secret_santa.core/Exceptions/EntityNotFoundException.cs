using System;
using System.Collections.Generic;
using System.Text;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string message) : base(message)
    {

    }
}