using GIT.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIT;

public abstract class Component : IUserActions
{
    public string Name { get; set; }
    public IState Status { get; set; }
    public string Owner { get; set; }
    public Component(string name, string owner)
    {
        Name = name;
        Owner = owner;
        Status = Draft.GetInstance();
    }
    #region getFunctions
    public abstract Component GetParent();
    public abstract MyFile GetFile(string name);
    public abstract Folder GetFolder(string name);
    #endregion
    #region otherFunctions
    public abstract void Merge(Component other);
    public abstract Component Clone(string name);
    public abstract void Undo();
    public abstract void Print(int num = 0);
    #endregion
    #region userFunctions
    public void Add()
    {
        if (Status.GetStatus() == "Draft")
        {
            Status.ChangeStatus(this);
            Console.WriteLine($"{this.Name} was added succefuly.");
        }
    }
    public void Commit()
    {
        if (Status.GetStatus() == "Staged")
        {
            Status.ChangeStatus(this);
            Console.WriteLine($"{this.Name} was commited succefuly.");
        }
    }
    public void Pull()
    {
        if (Status.GetStatus() == "Commited")
        {
            Status.ChangeStatus(this);
            Status.ChangeStatus(this);
        }
        var parent = this.GetParent();
        if (parent == null)
        {
            Console.WriteLine("you are already up-to-date.");
            return;
        }
        try
        {
            MyFile file = parent.GetFile(this.Name);
            if (file == null)
            {
                throw new Exception();
            }
            file.Merge(this);
        }
        catch
        {
            try
            {
                Folder folder = parent.GetFolder(this.Name);
                if (folder == null)
                {
                    throw new Exception();
                }
                folder.Merge(this);
            }
            catch
            {
                Branch branch = (Branch)this.GetParent();
                branch.Merge(this);
            }
        }
        Console.WriteLine($"{this.Name} was merged succesfuly");
    }
    public void Push()
    {
        if (Status.GetStatus() == "Merged")
        {
            Console.WriteLine($"{this.Name} was pushed succefuly.");
        }
    }
    #endregion
}
