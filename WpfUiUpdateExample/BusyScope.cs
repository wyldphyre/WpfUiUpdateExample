using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfUiUpdateExample
{
    public sealed class BusyScope : IDisposable
    {
        private readonly Cursor originalCursor = null;

        public BusyScope()
        {
            this.originalCursor = Mouse.OverrideCursor;
            Mouse.OverrideCursor = Cursors.Wait;
        }

        public void Dispose()
        {
            Mouse.OverrideCursor = this.originalCursor;
        }
    }
}
