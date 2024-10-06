namespace GeometryApp.Common.Filters;

public enum FilterOperator
{
    Equals = 1 << 0, // 1 = 1 = 0b0001
    Less = 1 << 1, // 2 = 2   = 0b0010
    More = 1 << 2, // 3 = 4   = 0b0100
    Not = 1 << 3 // 4 = 8     = 0b1001
}

public enum InternalFilterOperator
{
    Equals = 1 << 0, // 1 = 1 = 0b0001
    Less = 1 << 1, // 2 = 2   = 0b0010
    More = 1 << 2, // 3 = 4   = 0b0100
    Not = 1 << 3, // 4 = 8     = 0b1001

    Exists = 1 << 31
}
