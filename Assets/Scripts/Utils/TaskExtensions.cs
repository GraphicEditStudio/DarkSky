using System.Threading.Tasks;
using UnityEngine;

namespace Utils
{
    public static class TaskExtensions
    {
        public static void DoNotAwait(this Task task)
        {
            task.ContinueWith((t) =>
            {
                if (task.IsFaulted)
                {
                    Debug.Log(task.Exception);
                }
            }, TaskContinuationOptions.ExecuteSynchronously);
        }
    }
}