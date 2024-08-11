using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIT;

public class GITUser
{
    public GITUser(string userName)
    {
        this.UserName = userName;
        this.Projects = new();
    }
    public string UserName { get; set; }
    public Dictionary<string, Branch> Projects { get; set; }
    public void AddBranch(string name)
    {
        this.Projects.Add(name, new Branch(name, UserName));
    }
    public void DeleteBranch(string name)
    {
        Projects.Remove(name);
    }
    public Branch GetBranch(string name)
    {
        try
        {
            return Projects[name];
        }
        catch
        {
            Branch res;
            foreach (var branch in Projects)
            {
                res = branch.Value.GetBranch(name);
                if (res != null)
                {
                    return res;
                }
            }
        }
        return null;
    }
    public void print()
    {
        foreach (var branch in Projects)
        {
            branch.Value.Print();
        }
    }
}
