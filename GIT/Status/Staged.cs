using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIT.Status;

public class Staged : IState
{
    private static Staged instance;
    private Staged() { }
    public static Staged GetInstance()
    {
        instance ??= new Staged();
        return instance;
    }
    public void ChangeStatus(Component component)
    {
        component.Status = Commited.GetInstance();
    }

    public string GetStatus()
    {
        return "Staged";
    }
}
