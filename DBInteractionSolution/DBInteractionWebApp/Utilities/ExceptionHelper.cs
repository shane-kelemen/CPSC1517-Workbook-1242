namespace DBInteractionWebApp.Utilities
{
    public static class ExceptionHelper
    {
        public static Exception GetInnerException(Exception ex)
        {
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }

            return ex;
        }
    }
}
