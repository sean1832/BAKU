using System;
using System.Drawing;
using System.Reflection;
using Grasshopper.Kernel;

namespace Baku
{
    public class BakuInfo : GH_AssemblyInfo
    {
        public override string Name => GetInfo<AssemblyProductAttribute>().Product;
        public override string Version => GetInfo<AssemblyInformationalVersionAttribute>().InformationalVersion;
        public override Bitmap Icon => Icons.Baku;
        public override string Description => GetInfo<AssemblyDescriptionAttribute>().Description;
        public override Guid Id => new Guid("0a7e383d-a1c0-46d4-a22b-0691887d406f");
        public override string AuthorName => Config.Project.Author;
        public override string AuthorContact => Config.Project.Contact;

        T GetInfo<T>() where T : Attribute
        {
            var assembly = Assembly.GetExecutingAssembly();
            return assembly.GetCustomAttribute<T>();
        }
    }
}