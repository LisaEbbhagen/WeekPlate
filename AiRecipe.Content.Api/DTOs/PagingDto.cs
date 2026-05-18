namespace AiRecipe.Content.Api.DTOs
{
    public class PagingDto
    {
        // 1. Pagination metadata: describes "where we are" in the list
        public record PaginationMeta(
            int Page,
            int PageSize,
            int TotalPages,
            int TotalCount,
            bool HasNext,
            bool HasPrevious
        );

        // 2. Envelope: holds both the metadata and the actual list
        // This is a generic wrapper (<T>) so it can be reused for any type of list.
        public record PagedResponse<T>(
            IEnumerable<T> Data,
            PaginationMeta Pagination
        );
    }
}
