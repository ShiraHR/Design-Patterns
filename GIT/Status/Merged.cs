using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIT.Status;

public class Merged : IState
{
    private static Merged instance;
    private Merged() { }
    public static Merged GetInstance()
    {
        instance ??= new Merged();
        return instance;
    }
    public void ChangeStatus(Component component)
    {
        throw new NotImplementedException();
    }

    public string GetStatus()
    {
        return "Merged";
    }
}
