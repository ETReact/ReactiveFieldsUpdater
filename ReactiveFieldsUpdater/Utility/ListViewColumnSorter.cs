using System;
using System.Collections;
using System.Windows.Forms;

namespace ReactiveFieldsUpdater
{
    /// <summary>
    /// Sorts ListView items by the clicked column; clicking the same column again reverses the order.
    /// </summary>
    internal class ListViewColumnSorter : IComparer
    {
        public int Column { get; private set; }
        public SortOrder Order { get; private set; } = SortOrder.Ascending;

        public int Compare(object x, object y)
        {
            var a = x as ListViewItem;
            var b = y as ListViewItem;
            if (a == null || b == null)
                return 0;

            var textA = Column < a.SubItems.Count ? a.SubItems[Column].Text : string.Empty;
            var textB = Column < b.SubItems.Count ? b.SubItems[Column].Text : string.Empty;
            var result = string.Compare(textA, textB, StringComparison.CurrentCultureIgnoreCase);

            return Order == SortOrder.Descending ? -result : result;
        }

        public static void Attach(ListView listView)
        {
            var sorter = new ListViewColumnSorter();
            listView.Sorting = SortOrder.None;
            listView.ListViewItemSorter = sorter;
            listView.ColumnClick += (s, e) =>
            {
                if (e.Column == sorter.Column)
                {
                    sorter.Order = sorter.Order == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
                }
                else
                {
                    sorter.Column = e.Column;
                    sorter.Order = SortOrder.Ascending;
                }

                listView.Sort();
            };
        }
    }
}
