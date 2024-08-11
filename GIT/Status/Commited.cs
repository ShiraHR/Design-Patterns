using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIT.Status;

public class Commited : IState
{
    private static Commited instance;
    private Commited() { }
    public static Commited GetInstance()
    {
        instance ??= new Commited();
        return instance;
    }
    public void ChangeStatus(Component component)
    {
        Console.WriteLine($"{component.Owner}, there were changes in {component.Name}. please approve.");
        component.Status = UnderReview.GetInstance();
    }

    public string GetStatus()
    {
        return "Commited";
    }
}
