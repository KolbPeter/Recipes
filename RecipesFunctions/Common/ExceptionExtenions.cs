using System;

namespace RecipesFunctions.Common
{
    public static class ExceptionExtenions
    {
        public static string InnerstException(this Exception ex) =>
            ex.InnerException?.InnerstException() ?? ex.Message;
    }
}