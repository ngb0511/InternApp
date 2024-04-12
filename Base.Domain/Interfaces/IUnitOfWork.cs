using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IDummyCodeRepository DummyCodes { get; }
        ILogRepository Logs { get; }
        ITimingPostRepository TimingPosts { get; }
        IUserAssignRepository UserAssigns { get; }
        void Complete();
    }

}
