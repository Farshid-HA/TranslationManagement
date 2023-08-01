export interface ResponseApi<T>{
    pageCount: number,
    recordCount: number,
    page: number,
    data:T []
}