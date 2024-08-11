using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIT.Status;

public class ReadyToMerge : IState
{
    private static ReadyToMerge instance;
    private ReadyToMerge() { }
    public static ReadyToMerge GetInstance()
    {
        instance ??= new ReadyToMerge();
        return instance;
    }
    public void ChangeStatus(Component component)
    {
        component.Status = Merged.GetInstance();
    }

    public string GetStatus()
    {
        return "ReadyToMerge";
    }
}
