export interface PaginationHeader {
  currentPage: number;
  itemsPerPage: number;
  totalItems: number;
  totalPages: number;
}

export interface Pagination {
  currentPage: number;
  itemsPerPage: number;
  totalItems: number;
  totalPages: number;
}

export interface PaginatedResult<T> {
  result: T;
  pagination: Pagination | null;
}

export interface PaginationParams {
  pageNumber: number;
  pageSize: number;
}

export interface UserSearchCriteria extends PaginationParams {
  excludeUsername?: string;
}
