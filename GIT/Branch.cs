using GIT.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIT;

public class Branch : Component
{
    private Stack<Branch> history;
    private Branch branch = null;
    private Dictionary<string, Component> Children { get; set; }

    public Branch(string name, string owner, Branch branch = null) : base(name, owner)
    {
        Children = new();
        history = new();
        this.branch = branch;
    }
    #region getFunctions
    public Branch GetBranch(string name)
    {
        try
        {
            return (Branch)Children[name];
        }
        catch
        {
            Branch res;
            foreach (var branch in Children)
            {
                if (branch.GetType() != typeof(Branch)) { return null; }
                res = ((Branch)branch.Value).GetBranch(name);
                if (res != null)
                {
                    return res;
                }
            }
        }
        return null;
    }
    public override Folder GetFolder(string name)
    {
        Folder res = null;
        foreach (var branch in Children)
        {
            try
            {
                if (((Folder)branch.Value).Name == name)
                {
                    return (Folder)branch.Value;
                }
                return branch.Value.GetFolder(name);
            }
            catch
            {
                try
                {
                    res = ((Branch)branch.Value).GetFolder(name);
                }
                catch
                {

                }
            }
            if (res != null)
            {
                return res;
            }
        }

        return null;
    }
    public override MyFile GetFile(string name)
    {
        MyFile res;
        foreach (var branch in Children)
        {
            res = branch.Value.GetFile(name);
            if (res != null)
            {
                return res;
            }
        }

        return null;
    }
    public override Component GetParent()
    {
        return this.branch;
    }
    #endregion
    #region addFunctions
    private void Add(Component component)
    {
        Children.Add(component.Name, component);
        Status = Draft.GetInstance();
    }
    public void AddFile(string name, string content = null)
    {
        this.Add(new MyFile(name, this.Owner, this, content));
    }
    public void AddFolder(string name)
    {
        this.Add(new Folder(name, this.Owner, this));
    }
    public void AddBranch(string name)
    {
        this.Add(new Branch(name, this.Owner, this));
    }
    #endregion
    #region otherFunctions
    public override Component Clone(string name)
    {
        Branch branch = new(Name, this.Owner, this)
        {
            Status = Status,
            Children = new()
        };
        foreach (var item in Children)
        {
            try
            {
                if ((Branch)item.Value != null)
                {
                    continue;
                }
            }
            catch
            {
                branch.Children.Add(item.Key, item.Value.Clone(item.Value.Name));
            }
        }
        this.Children.Add(name, branch);
        branch.Status = Draft.GetInstance();
        Console.WriteLine($"{this.Name} cloned succesfuly");
        return this;
    }
    public void DeleteBranch(string name)
    {
        Children.Remove(name);
        Status = Draft.GetInstance();
    }
    public override void Merge(Component other)
    {
        if (other == null) return;
        if (this.Status.GetStatus() != "ReadyToMerge")
        {
            Console.WriteLine("you can not merge");
            return;
        }
        history.Push(new Branch(this.Name, this.Owner, this.branch) { Status = this.Status, Children = this.Children });
        try
        {
            foreach (var child in ((Branch)other).Children)
            {
                try
                {
                    this.Children[child.Key].Merge(child.Value);
                }
                catch (Exception)
                {
                    this.Children.Add(child.Key, child.Value);
                }
            }
        }
        catch { }
        Status.ChangeStatus(this);
    }
    public override void Undo()
    {
        try
        {
            Branch last = history.Pop();
            if (last == null) return;
            this.Status = last.Status;
            this.Children = last.Children;
        }
        catch
        {
            Console.WriteLine($"nothing to undo in {this.Name}");
        }
        foreach (var child in this.Children)
        {
            child.Value.Undo();
        }
    }
    public override void Print(int num = 0)
    {
        for (int i = 0; i < num; i++)
        {
            Console.Write('\t');
        }
        Console.Write($" I'm branch {this.Name} and my children are:\n");
        foreach (var child in Children)
        {
            child.Value.Print(num + 1);
        }
    }
    #endregion
}