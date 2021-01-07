using System.Diagnostics;

namespace Smart.Platform.Diagnostics.Tracing
{
    public class TraceManager
    {
        #region Constructors
        #endregion

        #region Public Methods
        #endregion

        #region Protected Methods
        #endregion

        #region Private Methods
        #endregion

        public TraceManager()
        {
        }

        public static void Write(string message, string category)
        {
            // TODO: Deprecated in .NET Core
            //System.Diagnostic.Trace.Write(message, category);
        }
    }
}
