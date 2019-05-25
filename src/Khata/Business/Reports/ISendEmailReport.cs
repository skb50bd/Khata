using System.Threading.Tasks;
using Domain.Reports;

namespace Business.Reports
{
    public interface ISendEmailReport
    {
        Task<bool> Send(string subject, string body);
        Task<Summary> GetReport();
    }
}