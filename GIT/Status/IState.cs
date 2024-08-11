using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIT.Status;

public interface IState
{
    public void ChangeStatus(Component component);
    public string GetStatus();
}
