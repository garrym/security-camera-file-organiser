using System;
using System.Threading;

namespace SecurityCameraFileOrganiser
{
    public static class Retry
    {
        public static void Attempt(Action action, int numberOfAttempts = 10, int sleepDuration = 100)
        {
            for (var attempts = 0; attempts < numberOfAttempts; attempts++)
            {
                try
                {
                    action();
                    break;
                }
                catch
                { }
                Thread.Sleep(sleepDuration);
            }
        }
    }
}