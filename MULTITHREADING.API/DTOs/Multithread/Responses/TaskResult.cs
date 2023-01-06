namespace MULTITHREADING.API.DTOs.Multithread.Responses
{
    public class TaskResult
    {
        public bool TaskStatus { get; set; }
        public string? TaskMessage { get; set; }
        public string? TaskStackTrace { get; internal set; }
    }
}
