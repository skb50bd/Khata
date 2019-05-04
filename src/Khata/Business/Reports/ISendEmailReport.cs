using System.Threading.Tasks;

namespace Business.Reports
{
    public interface ISendEmailReport
    {
        Task<bool> Send(Email email);
    }
}