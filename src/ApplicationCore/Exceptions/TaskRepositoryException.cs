using System;
namespace ApplicationCore.Exceptions
{
    public class TaskRepositoryException : Exception
    {
        public TaskRepositoryException(string key, string message)
            : base(message)
        {
            Key = key;
        }

        public string Key { get; set; }
    }
}
