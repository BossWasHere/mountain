using Mountain.Core.Utils;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mountain.Core
{
    public static class TaskAddons
    {
        public const string MainTaskName = "Main";
        public const string GenericTaskName = "Thread";
        private static readonly ConcurrentDictionary<int, string> TaskNames = new ConcurrentDictionary<int, string>();

        public static Task SetTaskName(this Task task, string name)
        {
            SetTaskName(task.Id, name);
            task.ContinueWith((t) => RemoveTaskName(t.Id), TaskContinuationOptions.ExecuteSynchronously);
            return task;
        }

        public static IEnumerable<KeyValuePair<int, string>> GetNamedTasks()
        {
            return TaskNames.ReadOnlyEnumerable();
        }

        public static void SetTaskName(int taskId, string name)
        {
            TaskNames.TryAdd(taskId, name);
        }

        public static string GetTaskName(int taskId)
        {
            if (!TaskNames.TryGetValue(taskId, out var name))
            {
                return GenericTaskName + " " + taskId;
            }

            return name;
        }

        private static bool RemoveTaskName(int taskId)
        {
            return TaskNames.TryRemove(taskId, out _);
        }
    }
}
