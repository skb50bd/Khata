using System.Collections.Generic;
using System.Linq;

namespace Khata.Domain
{
    public class Refund : TrackedDocument, IWithdrawal
    {
        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        public int SaleId { get; set; }

        public virtual Sale Sale { get; set; }

        public virtual ICollection<SaleLineItem> Cart { get; set; }

        public decimal CashBack { get; set; }

        public decimal DebtRollback { get; set; }
        public decimal TotalBackPaid => CashBack + DebtRollback;

        public string Description { get; set; }

        public decimal TotalPrice => Cart?.Sum(li => li.NetPrice) ?? 0;

        public decimal ApparantLoss => Cart?.Sum(li => li.NetPrice - li.NetPurchasePrice) ?? 0;
        public decimal EffectiveLoss => TotalPrice - CashBack - DebtRollback;

        public decimal Amount => CashBack;

        public string TableName => nameof(Refund);

        public int? RowId => Id;
    }
}
