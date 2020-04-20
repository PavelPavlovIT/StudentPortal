import { Group } from "./group";

export class PageGroup {
    currentPage: number = 0;
    pageSize: number = 0;
    totalRecords: number = 0;
    records: Group[] = [];
}