using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Web;

namespace LeaveSystem.Web.Infrastructure
{
    [CompilerGenerated]
    [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [DebuggerNonUserCode]
    internal class Resources
    {
        private static ResourceManager resourceMan;
        private static CultureInfo resourceCulture;

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals((object)Resources.resourceMan, (object)null))
                    Resources.resourceMan = new ResourceManager("LeaveSystem.Web.Infrastructure.Resources", typeof(Resources).Assembly);
                return Resources.resourceMan;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        internal static CultureInfo Culture
        {
            get
            {
                return Resources.resourceCulture;
            }
            set
            {
                Resources.resourceCulture = value;
            }
        }

        internal static string DefaultError
        {
            get
            {
                return Resources.ResourceManager.GetString("DefaultError", Resources.resourceCulture);
            }
        }

        internal static string StoreNotIQueryableEntityStore
        {
            get
            {
                return Resources.ResourceManager.GetString("StoreNotIQueryableEntityStore", Resources.resourceCulture);
            }
        }

        internal static string EntityIdNotFound
        {
            get
            {
                return Resources.ResourceManager.GetString("EntityIdNotFound", Resources.resourceCulture);
            }
        }

        internal Resources()
        {
        }
    }
}