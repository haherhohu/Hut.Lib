namespace Hut
{
    public class HutFileTaskResult : IHutTaskResult
    {
        public int TaskID { get; set; }
        public HutTaskType TaskType { get { return HutTaskType.File; } }
        public HutTaskActionStatus Status { get; set; }
        public string Group { get; set; }
        public string Name { get; set; }
        public HutTaskActionResult Result { get; set; }
        public int ActionID { get; set; }
        public HutTaskActionType ActionType { get; set; }
        public string CompleteTime { get; set; }
    }
}