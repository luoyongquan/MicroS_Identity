﻿using System.Threading.Tasks;

namespace Core.Threading
{
    public static class TaskCache
    {
        public static Task CompletedTask { get; } = Task.FromResult(0);
    }
}
