using System.ComponentModel.DataAnnotations.Schema;

namespace SpecFlowSampleProject
{
    public class SampleJE
    {
        private string LedgerId { get; set; }
        private string LedgerName { get; set; }
        private int RecordNum { get; set; }
        private decimal AmountDr { get; set; }
        private decimal AmountCr { get; set; }
        public SampleJE(string ledgerId, string ledgerName, int recordNum, decimal amountDr, decimal amountCr)
        {
            LedgerId = ledgerId;
            LedgerName = ledgerName;
            RecordNum = recordNum;
            AmountDr = amountDr;
            AmountCr = amountCr;
        }

        public override string ToString()
        {
            //print decimal with 2 decimal places
            return "{" +
                   LedgerId + '\'' +
                   "," + LedgerName + '\'' +
                   "," + RecordNum +
                   "," + AmountDr.ToString("0.00") +
                   "," + AmountCr.ToString("0.00") +
                   "}"; ;
        }
    }
}