namespace Hut
{
    public interface IHutTaskResult
    {
        int TaskID { get; set; }
        HutTaskType TaskType { get; }
        HutTaskActionStatus Status { get; set; }
        string Group { get; set; }
        string Name { get; set; }
        HutTaskActionResult Result { get; set; }
        int ActionID { get; set; }
        HutTaskActionType ActionType { get; set; }
    }
}