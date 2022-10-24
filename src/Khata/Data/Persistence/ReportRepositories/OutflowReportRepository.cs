using Data.Core;

using Domain;
using Domain.Reports;
using Domain.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

using static System.Decimal;

namespace Data.Persistence.Reports;

public class OutflowReportRepository 
    : IReportRepository<PeriodicalReport<Outflow>>
{
    private readonly IDateTimeProvider _dateTime;
    private readonly KhataContext  _db;
    private readonly KhataSettings _settings;

    public OutflowReportRepository(
        KhataContext db,
        IOptionsMonitor<KhataSettings> settings, 
        IDateTimeProvider dateTime)
    {
        _db       = db;
        _dateTime = dateTime;
        _settings = settings.CurrentValue;
    }

    private Task<Outflow?> GetOutflow(DateOnly fromDate) =>
        GetOutflow(fromDate.ToDateTime(TimeOnly.MinValue));
    
    private async Task<Outflow?> GetOutflow(DateTime fromDate)
    {
        var expensesQuery =
            _db.Set<Expense>()
                .Where(e => e.Metadata.CreationTime > fromDate && e.IsRemoved == false);

        var expensesCountTask = expensesQuery.CountAsync();
        var expensesSumTask   = expensesQuery.SumAsync(x => x.Amount);
        
        var purchasesQuery =
            _db.Set<Purchase>()
                .Where(e => e.Metadata.CreationTime > fromDate && e.IsRemoved == false);

        var purchasesCountTask = purchasesQuery.CountAsync();
        var purchasesSumTask   = purchasesQuery.SumAsync(x => x.Payment.Paid);
        
        var salaryPaymentsQuery =
            _db.Set<SalaryPayment>()
                .Where(e => e.Metadata.CreationTime > fromDate && e.IsRemoved == false);
        
        var salaryPaymentsCountTask = salaryPaymentsQuery.CountAsync();
        var salaryPaymentsSumTask   = salaryPaymentsQuery.SumAsync(x => x.Amount);
        
        var supplierPaymentsQuery =
            _db.Set<SupplierPayment>()
                .Where(e => e.Metadata.CreationTime > fromDate && e.IsRemoved == false);

        var supplierPaymentsCountTask = supplierPaymentsQuery.CountAsync();
        var supplierPaymentsSumTask   = supplierPaymentsQuery.SumAsync(x => x.Amount);
        
        var refundsQuery =
            _db.Set<Refund>()
                .Where(e => e.Metadata.CreationTime > fromDate && e.IsRemoved == false);

        var refundsCountTask = refundsQuery.CountAsync();
        var refundsSumTask = refundsQuery.SumAsync(x => x.CashBack);

        var withdrawalsQuery =
            _db.Set<Withdrawal>()
                .Where(e => e.Metadata.CreationTime > fromDate && e.TableName == nameof(Withdrawal));

        var withdrawalsCountTask = withdrawalsQuery.CountAsync();
        var withdrawalsSumTask = withdrawalsQuery.SumAsync(x => x.Amount);
        
        await Task.WhenAll(
            expensesSumTask,
            expensesCountTask,
            purchasesSumTask,
            purchasesCountTask,
            salaryPaymentsSumTask,
            salaryPaymentsCountTask,
            supplierPaymentsCountTask,
            supplierPaymentsSumTask,
            refundsCountTask,
            refundsSumTask,
            withdrawalsCountTask,
            withdrawalsSumTask
        );
        
        return new Outflow
        {
            EmployeePaymentCount  = await salaryPaymentsCountTask,
            EmployeePaymentAmount = Round(await salaryPaymentsSumTask, 2),
            ExpenseCount          = await expensesCountTask,
            ExpenseAmount         = Round(await expensesSumTask, 2),
            PurchaseCount         = await purchasesCountTask,
            PurchasePaid          = Round(await purchasesSumTask, 2),
            SupplierPaymentCount  = await supplierPaymentsCountTask,
            SupplierPaymentAmount = Round(await supplierPaymentsSumTask, 2),
            RefundCount           = await refundsCountTask,
            RefundAmount          = Round(await refundsSumTask, 2),
            WithdrawalCount       = await withdrawalsCountTask,
            WithdrawalAmount      = Round(await withdrawalsSumTask, 2)
        };
    }

    public async Task<PeriodicalReport<Outflow>> Get()
    {
        if (_settings.DbProvider == DbProvider.SQLServer)
        {
            var outflows =
                await _db.Set<Outflow>().ToListAsync();
            
            return new PeriodicalReport<Outflow>
            {
                Daily   = outflows[0],
                Weekly  = outflows[1],
                Monthly = outflows[2]
            };
        }

        var today   = _dateTime.Today;
        var daily   = await GetOutflow(today);
        var weekly  = await GetOutflow(today.StartOfTheWeek(DayOfWeek.Saturday));
        var monthly = await GetOutflow(today.StartOfTheMonth());

        return new PeriodicalReport<Outflow>
        {
            ReportDate = today,
            Daily      = daily,
            Weekly     = weekly,
            Monthly    = monthly
        };
    }
}