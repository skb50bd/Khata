using System.Threading.Tasks;
using Domain.Reports;

namespace Business.Reports
{
    public interface ISendEmailReport
    {
        Task<bool> Send(Email email);
        Task<Summary> GetReport();
    }
}