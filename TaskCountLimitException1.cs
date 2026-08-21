using System;
using Dz_3_Prosto_al;

namespace Dz_3_Prosto_al
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
}