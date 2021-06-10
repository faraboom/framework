namespace Faraboom.Framework.Data
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class ListDataSource<T>
    {
        [JsonPropertyName("results")]
        public IList<ListDataItem<T>> Data { get; set; }

        [JsonPropertyName("pagination")]
        public Paging Pagination => new(Data?.Count == 10);

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
