using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J113D.Avalonia.Utilities.IO
{
    public interface IFileChangeTracker
    {
        public bool HasFileChanged { get; }

        public void StoreCurrentState();

        public void ResetCurrentState();

        public void CopyState(IFileChangeTracker source);
    }
}
