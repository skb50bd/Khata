using System;
using System.Threading.Tasks;
using Brotal;
using Business.PageFilterSort;
using Domain;
using DTOs;

namespace Business.Abstractions;

public interface IVoucharService
{
    Task<VoucharDto> Add(Vouchar model);
    Task<VoucharDto> Delete(int id);
    Task<bool> Exists(int id);
    Task<VoucharDto> Get(int id);
    Task<IPagedList<VoucharDto>> Get(PageFilter pf, DateTime? from = null, DateTime? to = null);
    Task<VoucharDto> Remove(int id);
    Task<VoucharDto> SetPurchase(int invoiceId, int saleId);
    Task<VoucharDto> SetSupplierPayment(int invoiceId, int debtPaymentId);
    Task<int> Count(DateTime? from, DateTime? to);
}