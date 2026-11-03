using System;

namespace Dz
{
    public class TaskCountLimitException : Exception
    {
        public int MaxTasks { get; }


        public TaskCountLimitException(int maxTasks)
            : base($"Превышено максимальное количество задач равное {maxTasks}")
        {
            MaxTasks = maxTasks;
        }

    }
    public class TaskLengthLimitException : Exception
    {
        public int TaskLength { get; }
        public int MaxTaskLength { get; }

        public TaskLengthLimitException(int taskLength, int taskLengthLimit)
            : base($"Длина задачи '{taskLength}' превышает максимально допустимое значение {taskLengthLimit}")
        {
            TaskLength = taskLength;
            MaxTaskLength = taskLengthLimit;
        }

    }
    public class DuplicateTaskException : Exception
    {
        public string TaskName { get; }

        public DuplicateTaskException(string task)
            : base($"Задача '{task}' уже существует")
        {
            TaskName = task;
        }
    }
}