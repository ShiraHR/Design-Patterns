using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIT.Status;

public class UnderReview : IState
{
    private static UnderReview instance;
    private UnderReview() { }
    public static UnderReview GetInstance()
    {
        instance ??= new UnderReview();
        return instance;
    }
    public void ChangeStatus(Component component)
    {
        component.Status = ReadyToMerge.GetInstance();
    }

    public string GetStatus()
    {
        return "UnderReview";
    }
}
