using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIT.Status;

public class Draft : IState
{
    private static Draft instance;
    private Draft()
    {

    }
    public static Draft GetInstance()
    {
        instance ??= new Draft();
        return instance;
    }
    public void ChangeStatus(Component component)
    {
        component.Status = Staged.GetInstance();
    }

    public string GetStatus()
    {
        return "Draft";
    }
}
