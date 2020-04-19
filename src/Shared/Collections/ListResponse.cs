using System.Collections.Generic;

namespace DevOpsLab.Shared.Collections
{
    public class ListResponse<T>
    {
        public IEnumerable<T> Items { get; set; } = new List<T>();

        public int Total { get; set; }
    }
}
