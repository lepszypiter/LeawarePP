namespace LeawareTest.Application;

public record PagedResponse<T>(IReadOnlyCollection<T> Data, int TotalCount, int Page, int PageSize);
