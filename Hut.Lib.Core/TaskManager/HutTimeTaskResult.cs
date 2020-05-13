namespace Hut
{
    public class HutTimeTaskResult : IHutTaskResult
    {
        public int TaskID { get; set; }
        public HutTaskType TaskType { get { return HutTaskType.Time; } }
        public HutTaskActionStatus Status { get; set; }
        public string Group { get; set; }
        public string Name { get; set; }
        public HutTaskActionResult Result { get; set; }
        public int ActionID { get; set; }
        public HutTaskActionType ActionType { get; set; }
        public string ExecuteTime { get; set; }
    }
}