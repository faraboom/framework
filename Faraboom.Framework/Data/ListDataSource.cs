using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Faraboom.Framework.Data
{
    public class ListDataSource<T>
    {
        [JsonPropertyName("results")]
        public List<ListDataItem<T>> Data { get; set; }

        [JsonPropertyName("pagination")]
        public Paging Pagination => new Paging(Data?.Count == 10);

        public struct Paging
        {
            public Paging(bool moreRecords)
            {
                MoreRecords = moreRecords;
            }

            [JsonPropertyName("more")]
            public bool MoreRecords { get; }
        }
    }

    public class ListDataItem<T>
    {
        [JsonPropertyName("text")]
        public string Name { get; set; }

        [JsonPropertyName("id")]
        public T Value { get; set; }
    }
}
