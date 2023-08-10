using System.Text.Json.Serialization;

namespace HomeBanking.DTOS
{
    public class ClientLoanDTO
    {
        [JsonIgnore]
        public long Id { get; set; }
        public long LoanId { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public int Payments { get; set; }

    }
}
