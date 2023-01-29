namespace SpecFlowSampleProject
{
    public class Result
    {
        private string LedgerId { get; set; }
        private string Remarks { get; set; }
        private string Status { get; set; }
        
        public Result(string ledgerId, string remarks, string status)
        {
            LedgerId = ledgerId;
            Remarks = remarks;
            Status = status;
        }
    }
}